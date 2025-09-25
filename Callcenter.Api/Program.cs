using Callcenter.Api.Configuration;
using Callcenter.Api.Data.Dapper;
using Callcenter.Api.Mappings;
using Callcenter.Api.Middlewares;
using Dapper;
using DotNetEnv;
using Minio;

namespace Callcenter.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddMinio(configureClient =>
            configureClient
                .WithEndpoint("localhost:9000")
                .WithCredentials("admin", "secret123")
                .WithSSL(false)
                .Build()
            );
        
        SqlMapper.AddTypeHandler(new DateOnlyHandler());
        
        Env.Load();
        var connectionString = Environment.GetEnvironmentVariable("DB_CONN");
        builder.Configuration["ConnectionStrings:Callcenter"] ??= connectionString;
        
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