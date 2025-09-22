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
        
        builder.Services
            .AddAppSerilog()
            .AddAppCors(builder.Configuration)
            .AddAppDbContexts(builder.Configuration)
            .AddAppOpenIddict(builder.Configuration)
            .AddAppSwagger()
            .AddAppProblemDetails()
            .AddAppControllers()
            .AddAppServices();

        // Add services to the container.
        // builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
        // builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        // builder.Services.AddEndpointsApiExplorer();
        // builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        // if (app.Environment.IsDevelopment())
        // {
        //     app.UseSwagger();
        //     app.UseSwaggerUI();
        // }
        //
        // app.UseHttpsRedirection();
        //
        // app.UseAuthorization();

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