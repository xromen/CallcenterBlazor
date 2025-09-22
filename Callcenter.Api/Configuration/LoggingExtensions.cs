using Serilog;
using Serilog.Core;
using Serilog.Sinks.Grafana.Loki;
using Serilog.Sinks.SystemConsole.Themes;

namespace Callcenter.Api.Configuration
{
    public static class LoggingExtensions
    {
        public static IServiceCollection AddAppSerilog(this IServiceCollection services)
        {
            LoggingLevelSwitch levelSwitch = new LoggingLevelSwitch();
            levelSwitch.MinimumLevel = Serilog.Events.LogEventLevel.Information;

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(levelSwitch)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", Serilog.Events.LogEventLevel.Warning)
                .WriteTo.Console(theme: ConsoleTheme.None)
                .WriteTo.File($"./Logs/log-.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.GrafanaLoki(
                        "http://192.168.1.128:3100",
                        new List<LokiLabel> { new LokiLabel() { Key = "app", Value = "CallcenterApp.Api" } },
                        credentials: null)
                .Enrich.FromLogContext()
                .CreateLogger();

            services.AddSerilog();
            services.AddSingleton(levelSwitch);

            return services;
        }

        public static IApplicationBuilder UseAppSerilog(this IApplicationBuilder app)
        {
            app.UseSerilogRequestLogging();
            return app;
        }
    }
}
