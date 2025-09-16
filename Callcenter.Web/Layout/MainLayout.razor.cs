using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Callcenter.Web.Layout;

public partial class MainLayout : LayoutComponentBase
{
    [Inject]
    public ILocalStorageService LocalStorageService { get; set; }
    
    private bool _zoomedIn = false;

    private const string _zoomedInStyle = @"
        .layout-container {
            width: 70%;
            margin-top: 5%;
        }

        .layout-paper {
            background-color: #e9ebf0;
            display: flex;
            min-height: 88vh;
        }
        
    ";
    private const string _zoomedOutStyle = @"
        .layout-container {
            width: 90%;
            margin-top: 3%;
        }

        .layout-paper {
            background-color: #e9ebf0;
            display: flex;
            min-height: 93vh;
        }
    ";

    protected override async Task OnInitializedAsync()
    {
        
        await base.OnInitializedAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _zoomedIn = await LocalStorageService.GetItemAsync<bool?>("zoomed") ?? false;
            StateHasChanged();
        }
            
        await base.OnAfterRenderAsync(firstRender);
    }
    
    private MudTheme _theme = new()
    {
        PaletteLight = new()
        {
            Dark = "#2f323b",
            DarkLighten = "#4c94e6",
            Warning = "#daba4d",
        }
    };

    private async Task ZoomToggledChanged(bool value)
    {
        _zoomedIn = value;
        
        await LocalStorageService.SetItemAsync("zoomed", value);
    }
}