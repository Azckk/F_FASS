using Common.NETCore;
using Common.NETCore.Extensions;
using Common.NETCore.Helpers;
using FASS.Web.Api.Attributes;
using FASS.Web.Api.Extensions.Configure;
using FASS.Web.Api.Hubs.Monitor;
using FASS.Web.Api.Hubs.Screen;
using FASS.Web.Api.Models;
using FASS.Web.Api.Services;
using Serilog;
using System.Diagnostics;
using StackExchange.Redis;
using System.IO;
using System.Net.Sockets;


static bool IsPortOpen(string host, int port)
{
    try
    {
        using var client = new TcpClient();
        var result = client.BeginConnect(host, port, null, null);
        var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(500));

        if (!success)
            return false;

        client.EndConnect(result);
        return true;
    }
    catch
    {
        return false;
    }
}

static string FindDevelopDirectory()
{
    var dir = new DirectoryInfo(Directory.GetCurrentDirectory());

    while (dir != null)
    {
        // 判断是不是 Develop 目录（你这个项目结构）
        if (Directory.Exists(Path.Combine(dir.FullName, "SourceCode")) &&
            Directory.Exists(Path.Combine(dir.FullName, "DataBase")))
        {
            return dir.FullName;
        }

        dir = dir.Parent;
    }

    throw new Exception("未找到 Develop 目录");
}

static void StartRedisIfNotRunning(IConfiguration configuration)
{
    if (!configuration.GetValue<bool>("RedisServer:AutoStart"))
        return;

    var developDir = FindDevelopDirectory();

    var workDir = Path.GetFullPath(Path.Combine(
        developDir,
        configuration["RedisServer:WorkDirectory"]));

    var exePath = Path.GetFullPath(Path.Combine(
        developDir,
        configuration["RedisServer:ExePath"]));

    if (IsPortOpen("127.0.0.1", 6379))
    {
        Console.WriteLine("Redis 端口 6379 已可用");
        return;
    }

    Console.WriteLine("Redis 未运行，正在启动 Redis...");

    var psi = new ProcessStartInfo
    {
        FileName = "cmd.exe",
        Arguments = $"/k \"cd /d \"{workDir}\" && \"{exePath}\" redis.conf\"",
        UseShellExecute = true
    };

    Process.Start(psi);
}

static void WaitRedisReady(string connectionString)
{
    while (true)
    {
        try
        {
            using var redis = ConnectionMultiplexer.Connect(connectionString);
            if (redis.IsConnected)
            {
                Console.WriteLine("Redis 连接成功");
                return;
            }
        }
        catch
        {
            Console.WriteLine("等待 Redis 就绪...");
            Thread.Sleep(1000);
        }
    }
}

var builder = WebApplication.CreateBuilder(args);
StartRedisIfNotRunning(builder.Configuration);
WaitRedisReady("127.0.0.1:6379");
var appSettings = builder.Configuration.Get<AppSettings>();
builder.Services.AddSingleton(Guard.NotNull(appSettings));
builder.Services.AddSerilog((services, logger) => logger.ReadFrom.Configuration(builder.Configuration));
builder.Services
    .AddControllers(options =>
    {
        options.Filters.Add(typeof(ResultAttribute));
        options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.AddDefaultOptions();
    });
builder.Services.AddHttpContextAccessor();
builder.Services.AddSignalR();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwashbuckle();
builder.Services.AddCurrent();
builder.Services.AddAuth(appSettings);

builder.Services.AddBoot(builder.Configuration, appSettings);
builder.Services.AddHostedService<AppHostService>();
//if (!builder.Environment.IsDevelopment())
//{
//    builder.Services.AddBoot(builder.Configuration, appSettings);
//    builder.Services.AddHostedService<AppHostService>();
//}
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAny", builder =>
    {
        builder
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()
        .WithExposedHeaders("X-Pagination");
    });
});
builder.Services.AddOpenApi();
var app = builder.Build();
app.UseException();
app.UseSerilogRequestLogging();
app.UseRouting();
app.UseSwashbuckle();
app.UseCurrent();
app.UseAuth();
// Legacy behavior (kept for reference):
// app.Services.UseBoot();
if (!app.Environment.IsDevelopment())
{
    app.Services.UseBoot();
}
app.UseCors("AllowAny");
app.MapControllers();
app.MapHub<RuntimeHub>("/Monitor/RuntimeHub");
app.MapHub<DataHub>("/Screen/DataHub");
app.MapHub<AlarmHub>("/Monitor/AlarmHub");
if (!app.Environment.IsDevelopment())
{
    app.Urls.AddRange(appSettings.Server.Urls.ToArray());
    app.Lifetime.ApplicationStarted.Register(() => BrowserHelper.OpenBrowser($"{appSettings.Server.Urls.First()}/swagger"));
}
app.Run();
