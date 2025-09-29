using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.Grafana.Loki;
using Callcenter.Api.Data;
using Callcenter.Api.Models;
using Callcenter.Api.Services;

namespace Callcenter.Api.Configuration
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddLocalization();
            services.AddHttpContextAccessor();
            services.AddMemoryCache();
            services.AddScoped<RequestEnvironment>();
            services.AddScoped<AuthService>();
            services.AddScoped<AccountsService>();
            services.AddScoped<FileStorageService>();
            services.AddScoped<ExcelService>();
            services.AddScoped<DeclarationService>();
            services.AddScoped<DictionariesService>();
            services.AddScoped<QuestionsService>();
            services.AddScoped<NewsService>();
            services.AddScoped<ReportsService>();

            return services;
        }
    }
}
