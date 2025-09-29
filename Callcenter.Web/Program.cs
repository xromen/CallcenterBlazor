using System.Globalization;
using Blazored.LocalStorage;
using Callcenter.Web.Mappings;
using Callcenter.Web.Services;
using Callcenter.Web.Services.Authentication;
using Callcenter.Web.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using MudBlazor.Translations;
using Refit;

namespace Callcenter.Web;

public class Program
{
    public static async Task Main(string[] args)
    {
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("ru-RU");
        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("ru-RU");
        
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        var apiUri = builder.Configuration["ApiUri"] ?? throw new Exception("ApiUri configuration not found");

        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddMudServices(configuration =>
        {
            configuration.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopRight;
            configuration.SnackbarConfiguration.PreventDuplicates = false;
        });
        builder.Services.AddMudTranslations();

        builder.Services.AddBlazoredLocalStorage();
        builder.Services.AddLocalization();
        builder.Services.AddAuthorizationCore();
        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
        builder.Services.AddScoped<AuthenticationService>();
        builder.Services.AddScoped<RefreshTokenService>();

        builder.Services.AddTransient<RefreshTokenHandler>();
        builder.Services.AddHttpClient<AuthenticationService>(client => client.BaseAddress = new Uri(apiUri));
        builder.Services.AddHttpClient<JwtParser>(client => client.BaseAddress = new Uri(apiUri));
        builder.Services.AddRefitClient<IApiClient>()
            .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri(apiUri.TrimEnd('/'));
                    c.Timeout = TimeSpan.FromDays(1);
                }
            )
            .AddHttpMessageHandler<RefreshTokenHandler>();

        builder.Services.AddScoped<ProblemDetailsHandler>();
        builder.Services.AddScoped<DeclarationsService>();
        builder.Services.AddScoped<NewsService>();
        builder.Services.AddScoped<AccountsService>();
        builder.Services.AddScoped<ReportsService>();
        
        MapsterConfig.RegisterMappings();

        await builder.Build().RunAsync();
    }
}