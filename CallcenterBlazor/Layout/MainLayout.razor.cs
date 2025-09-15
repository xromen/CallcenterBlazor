using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CallcenterBlazor.Layout;

public partial class MainLayout : LayoutComponentBase
{
    private MudTheme _theme = new()
    {
        PaletteLight = new()
        {
            Dark = "#2f323b",
            DarkLighten = "#4c94e6",
            Warning = "#daba4d",
        }
    };
}