using Callcenter.Api.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Core;

namespace Callcenter.Api.Configuration
{
    public static class DbContextsExtensions
    {
        public static IServiceCollection AddAppDbContexts(this IServiceCollection services, IConfiguration config)
        {
            var mainConnectionString = config.GetConnectionString("Callcenter") ?? Environment.GetEnvironmentVariable("DB_CONN");

            services.AddDbContextPool<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(mainConnectionString);
                options.UseOpenIddict();
            });

            return services;
        }
    }
}
