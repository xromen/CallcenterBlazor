using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Callcenter.Api.Configuration
{
    public static class ControllersExtensions
    {
        public static IServiceCollection AddAppControllers(this IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            return services;
        }

        public static IApplicationBuilder UseAppControllers(this WebApplication app)
        {
            app.MapControllers();
            app.MapDefaultControllerRoute();

            return app;
        }
    }
}