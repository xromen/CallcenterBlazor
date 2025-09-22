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
                    .WithOrigins(
                        "http://localhost:5249", 
                        "http://localhost:5004",
                        "http://192.168.1.128:5004")
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
