using Blazored.LocalStorage;
using Callcenter.Web.Components;
using MudBlazor;
using MudBlazor.Services;
using MudBlazor.Translations;

namespace Callcenter.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.WebHost.UseStaticWebAssets();

            builder.Services.AddMudServices(configuration =>
            {
                configuration.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopRight;
                configuration.SnackbarConfiguration.PreventDuplicates = false;
            });
            builder.Services.AddMudTranslations();
            builder.Services.AddBlazoredLocalStorage();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
