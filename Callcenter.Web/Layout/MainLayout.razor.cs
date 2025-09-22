using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace Callcenter.Web.Layout;

public partial class MainLayout : LayoutComponentBase
{
    [CascadingParameter]
    private Task<AuthenticationState> AuthStateTask { get; set; } = null!;
    
    [Inject]
    public ILocalStorageService LocalStorageService { get; set; }

    private string? _userFullName;
    private string? _userGroup;
    private string? _userOrganisation;
    
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
        var authState = await AuthStateTask;

        _zoomedIn = await LocalStorageService.GetItemAsync<bool?>("zoomed") ?? false;
        
        _userFullName = authState.User.FindFirst("FullName")?.Value;
        _userGroup = authState.User.FindFirst("Group")?.Value;
        _userOrganisation = authState.User.FindFirst("Organisation")?.Value;
        
        await base.OnInitializedAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // var authState = await AuthStateTask;
            //
            // var zoomedInRes = await ProtectedSessionStore.GetAsync<bool?>("zoomed");
            // _zoomedIn = zoomedInRes.Value ?? false;
            //
            // _userFullName = authState.User.FindFirst("FullName")?.Value;
            // _userGroup = authState.User.FindFirst("Group")?.Value;
            // _userOrganisation = authState.User.FindFirst("Organisation")?.Value;
            
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