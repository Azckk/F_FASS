using FASS.Scheduler.Services;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;

namespace FASS.Scheduler.Extensions.Configure
{
    public static class SwaggerExtension
    {
        public static IServiceCollection AddSwashbuckle(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = AppHostService.EntryAssembly.Name,
                    Version = AppHostService.EntryAssembly.Version?.ToString()
                });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Description = "Value {Bearer Token}",
                    Name = "Authorization",
                    BearerFormat = "JWT"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });
                options.OrderActionsBy(o => o.RelativePath);
            });
            return services;
        }

        public static IApplicationBuilder UseSwashbuckle(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });
            //app.UserAutoHttpMethod();
            return app;
        }

        public static IApplicationBuilder UserAutoHttpMethod(this IApplicationBuilder app, string defaultHttpMethod = "POST")
        {
            var apiDescriptionGroupCollectionProvider = app.ApplicationServices.GetRequiredService<IApiDescriptionGroupCollectionProvider>();
            var apiDescriptionGroupsItems = apiDescriptionGroupCollectionProvider.ApiDescriptionGroups.Items;
            foreach (var apiDescriptionGroup in apiDescriptionGroupsItems)
            {
                foreach (var apiDescription in apiDescriptionGroup.Items)
                {
                    if (string.IsNullOrEmpty(apiDescription.HttpMethod))
                    {
                        var actionName = apiDescription.ActionDescriptor.RouteValues["action"];
                        if (actionName?.StartsWith("get", StringComparison.OrdinalIgnoreCase) ?? false)
                        {
                            defaultHttpMethod = "GET";
                        }
                        else if (actionName?.StartsWith("put", StringComparison.OrdinalIgnoreCase) ?? false)
                        {
                            defaultHttpMethod = "PUT";
                        }
                        else if (actionName?.StartsWith("delete", StringComparison.OrdinalIgnoreCase) ?? false)
                        {
                            defaultHttpMethod = "DELETE";
                        }
                        apiDescription.HttpMethod = defaultHttpMethod;
                    }
                }
            }
            return app;
        }
    }
}