using Callcenter.Api.Configuration;
using Callcenter.Api.Data.Dapper;
using Callcenter.Api.Mappings;
using Callcenter.Api.Middlewares;
using Dapper;
using DotNetEnv;
using Minio;
using OfficeOpenXml;

namespace Callcenter.Api;

public class Program
{
    public static void Main(string[] args)
    {
        ExcelPackage.License.SetNonCommercialOrganization("HKFOMS");
        
        var builder = WebApplication.CreateBuilder(args);
        
        SqlMapper.AddTypeHandler(new DateOnlyHandler());
        
        MapsterConfig.RegisterMappings();

        builder.Configuration
            .AddEnvironmentVariables();

        builder.Services
            .AddAppMinio(builder.Configuration)
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