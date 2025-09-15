using CallcenterBlazor.Components;
using MudBlazor;
using MudBlazor.Services;
using MudBlazor.Translations;

namespace CallcenterBlazor
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
