using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.Grafana.Loki;
using Callcenter.Api.Data;
using Callcenter.Api.Middlewares;
using Callcenter.Api.Models;

namespace Callcenter.Api.Configuration
{
    public static class ProblemDetailsExtensions
    {
        public static IServiceCollection AddAppProblemDetails(this IServiceCollection services)
        {
            services.AddProblemDetails(options =>
            {
                options.CustomizeProblemDetails = context =>
                {
                    context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
                    context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
                };
            });

            services.AddExceptionHandler<ExceptionHandler>();

            return services;
        }

        public static IApplicationBuilder UseAppProblemDetails(this IApplicationBuilder app)
        {
            app.UseExceptionHandler();
            app.UseStatusCodePages();

            return app;
        }
    }
}
