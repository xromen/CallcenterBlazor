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
                        "http://localhost:5006",
                        "http://192.168.1.128:5006",
                        "http://192.168.1.16:5006",
                        "http://192.168.1.17:5006",
                        "http://7.12.24.7:5006"
                        )
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
