using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Callcenter.Api.Configuration
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddAppSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new()
                {
                    Title = "Callcenter API",
                    Version = "v1",
                    Description = "API для веб приложения Callcenter",
                });

                options.CustomSchemaIds(x => x.FullName);

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);

                options.ResolveConflictingActions(descriptions => descriptions.First());

                options.AddSecurityDefinition("oauth2", new()
                {
                    Description = "OAuth2.0 Authorization",
                    Flows = new()
                    {
                        Password = new()
                        {
                            TokenUrl = new("/connect/token", UriKind.Relative),
                        },
                    },
                    In = ParameterLocation.Header,
                    Name = HeaderNames.Authorization,
                    Type = SecuritySchemeType.OAuth2,
                });

                options.AddSecurityRequirement(new()
                {
                    {
                        new()
                        {
                            Reference = new()
                            {
                                Id = "oauth2",
                                Type = ReferenceType.SecurityScheme,
                            },
                            In = ParameterLocation.Cookie,
                            Type = SecuritySchemeType.OAuth2,
                        },
                        new List<string>()
                    },
                });
            });

            return services;
        }

        public static IApplicationBuilder UseAppSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }
    }
}