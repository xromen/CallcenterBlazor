using Callcenter.Api.Configuration;
using Callcenter.Api.Mappings;
using Callcenter.Api.Middlewares;

namespace Callcenter.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        MapsterConfig.RegisterMappings();

        builder.Configuration
            .AddEnvironmentVariables();

        builder.Services
            .AddAppSerilog()
            .AddAppCors(builder.Configuration)
            .AddAppDbContexts(builder.Configuration)
            .AddAppOpenIddict(builder.Configuration)
            .AddAppSwagger()
            .AddAppProblemDetails()
            .AddAppControllers()
            .AddAppServices();

        var app = builder.Build();

        app.UseAppSerilog()
            .UseAppProblemDetails()
            .UseAppSwagger()
            .UseAppCors();
        app.UseAppOpenIdDict();

        app.UseAppControllers();

        app.UseMiddleware<LogRequestBodyMiddleware>();
        app.UseMiddleware<AuthMiddleware>();
        
        app.Run();
    }
}