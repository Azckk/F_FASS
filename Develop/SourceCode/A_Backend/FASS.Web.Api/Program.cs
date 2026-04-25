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

var builder = WebApplication.CreateBuilder(args);
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
