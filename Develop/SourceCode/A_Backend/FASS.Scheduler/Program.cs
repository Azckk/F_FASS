using Common.NETCore;
using Common.NETCore.Extensions;
using Common.NETCore.Helpers;
using FASS.Scheduler.Attributes;
using FASS.Scheduler.Extensions.Configure;
using FASS.Scheduler.Models;
using FASS.Scheduler.Services;
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
app.UseAuth();
app.Services.UseBoot();
app.UseCors("AllowAny");
app.MapControllers();
if (!app.Environment.IsDevelopment())
{
    app.Urls.AddRange(appSettings.Server.Urls.ToArray());
    app.Lifetime.ApplicationStarted.Register(() => BrowserHelper.OpenBrowser($"{appSettings.Server.Urls.First()}/swagger"));
}
app.Run();
