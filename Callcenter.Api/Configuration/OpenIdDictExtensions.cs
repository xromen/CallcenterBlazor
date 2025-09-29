using Callcenter.Api.Data;
using Callcenter.Api.Data.Entities;
using OpenIddict.Validation.AspNetCore;
using Callcenter.Api.Models;
using Callcenter.Api.Services;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;

namespace Callcenter.Api.Configuration
{
    public static class OpenIdDictExtensions
    {
        public static IServiceCollection AddAppOpenIddict(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = OpenIddictConstants.Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = OpenIddictConstants.Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = OpenIddictConstants.Claims.Role;
                options.ClaimsIdentity.EmailClaimType = OpenIddictConstants.Claims.Email;
            });
            
            services.AddIdentity<User, UserGroup>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 4;
                    options.Password.RequireUppercase = false;
                })
                .AddUserStore<AppUserStore>()
                .AddRoleStore<AppRoleStore>()
                .AddDefaultTokenProviders();
            
            services.AddScoped<IPasswordHasher<User>, MyPasswordHasher<User>>();
            
            services.Configure<TokenSettings>(config.GetSection(nameof(TokenSettings)));

            var tokenSettings = config.GetSection(nameof(TokenSettings)).Get<TokenSettings>()
                                ?? new TokenSettings();

            services.AddOpenIddict()
                .AddCore(options =>
                {
                    options.UseEntityFrameworkCore()
                        .UseDbContext<ApplicationDbContext>();
                })
                .AddServer(options =>
                {
                    options.SetAuthorizationEndpointUris("connect/authorize")
                        .SetIntrospectionEndpointUris("connect/introspect")
                        .SetLogoutEndpointUris("connect/logout")
                        .SetTokenEndpointUris("connect/token")
                        .SetVerificationEndpointUris("connect/verify")
                        .SetUserinfoEndpointUris("connect/userinfo")
                        .SetCryptographyEndpointUris("connect/jwks");

                    options.AllowPasswordFlow()
                        .AllowRefreshTokenFlow();

                    options.SetAccessTokenLifetime(tokenSettings.AccessTokenLifetime);
                    options.SetRefreshTokenLifetime(tokenSettings.RefreshTokenLifetime);

                    options.AcceptAnonymousClients();

                    options.AddDevelopmentEncryptionCertificate()
                        .AddDevelopmentSigningCertificate();

                    options.UseAspNetCore()
                        .EnableAuthorizationEndpointPassthrough()
                        .EnableLogoutEndpointPassthrough()
                        .EnableTokenEndpointPassthrough()
                        .EnableUserinfoEndpointPassthrough()
                        .EnableStatusCodePagesIntegration()
                        .DisableTransportSecurityRequirement();
                })
                .AddValidation(options =>
                {
                    options.UseLocalServer();
                    options.UseAspNetCore();
                });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("admin", policy =>
                    policy.RequireAssertion(context =>
                    {
                        var groupClaim = context.User.FindFirst("Group")?.Value;
                        if (string.IsNullOrEmpty(groupClaim))
                            return false;

                        return groupClaim.Contains("Администр", StringComparison.OrdinalIgnoreCase) ||
                               groupClaim.Contains("Инжене", StringComparison.OrdinalIgnoreCase) ||
                               groupClaim.Contains("Руковод", StringComparison.OrdinalIgnoreCase);
                    }));
            });

            return services;
        }

        public static IApplicationBuilder UseAppOpenIdDict(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}
