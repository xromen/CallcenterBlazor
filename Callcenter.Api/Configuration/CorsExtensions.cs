namespace Callcenter.Api.Configuration
{
    public static class CorsExtensions
    {
        public static IServiceCollection AddAppCors(this IServiceCollection services, IConfiguration config)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    policyBuilder => policyBuilder
#if DEBUG
                        .WithOrigins("http://localhost:5249")
#else
                    .WithOrigins(config["CORS_ORIGIN"] ?? "http://localhost:5003")
#endif
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
            return services;
        }

        public static IApplicationBuilder UseAppCors(this IApplicationBuilder app)
        {
            app.UseCors("AllowSpecificOrigin");

            return app;
        }
    }
}
