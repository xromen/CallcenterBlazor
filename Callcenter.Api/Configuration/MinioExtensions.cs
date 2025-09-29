using Callcenter.Api.Models;
using Minio;

namespace Callcenter.Api.Configuration;

public static class MinioExtensions
{
    public static IServiceCollection AddAppMinio(this IServiceCollection services, IConfiguration config)
    {
        var minioConfig = config
            .GetSection("Minio")
            .Get<MinioConfiguration>() ?? throw new Exception("Minio configuration not found");
        
        services.AddMinio(configureClient =>
            configureClient
                .WithEndpoint(minioConfig.Endpoint)
                .WithCredentials(minioConfig.Login, minioConfig.Password)
                .WithSSL(false)
                .Build()
        );
        return services;
    }
}