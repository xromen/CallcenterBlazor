using Blazored.LocalStorage;
using Callcenter.Shared;
using Callcenter.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using MudBlazor;

namespace Callcenter.Web.Layout;

public partial class MainLayout : LayoutComponentBase, IDisposable
{
    [CascadingParameter]
    private Task<AuthenticationState> AuthStateTask { get; set; } = null!;
    
    [Inject]
    public ILocalStorageService LocalStorageService { get; set; }
    
    [Inject]
    public AccountsService AccountsService { get; set; }
    
    [Inject]
    public ProblemDetailsHandler ProblemDetailsHandler { get; set; }
    
    [Inject]
    public NavigationManager NavigationManager { get; set; }

    private string? _userFullName;
    private string? _userGroup;
    private string? _userOrganisation;
    private List<UserNotificationDto> _userNotifications = new();
    private bool _notificationsVisible = false;
    
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
    
    private MudTheme _theme = new()
    {
        PaletteLight = new()
        {
            Dark = "#2f323b",
            DarkLighten = "#4c94e6",
            Warning = "#daba4d",
        }
    };

    public void Dispose()
    {
        NavigationManager.LocationChanged -= LocationChangedAsync;
    }

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthStateTask;

        _zoomedIn = await LocalStorageService.GetItemAsync<bool?>("zoomed") ?? false;
        
        _userFullName = authState.User.FindFirst("FullName")?.Value;
        _userGroup = authState.User.FindFirst("Group")?.Value;
        _userOrganisation = authState.User.FindFirst("Organisation")?.Value;

        NavigationManager.LocationChanged += LocationChangedAsync;
        
        await base.OnInitializedAsync();
    }

    private async void LocationChangedAsync(object? sender, LocationChangedEventArgs e)
    {
        await LoadNotifications();
        _notificationsVisible = false;
        StateHasChanged();
    }

    private async ValueTask LoadNotifications()
    {
        var result = await AccountsService.GetUserNotifications();

        if (!result.Success)
        {
            ProblemDetailsHandler.Handle(result.Error!);
        }
        else
        {
            _userNotifications = result.Data!;
        }
    }

    private async Task ZoomToggledChanged(bool value)
    {
        _zoomedIn = value;
        
        await LocalStorageService.SetItemAsync("zoomed", value);
    }

    private async Task NotificationClick(UserNotificationDto notification)
    {
        var result = await AccountsService.ReadUserNotification(notification.Id);

        if (!result.Success)
        {
            ProblemDetailsHandler.Handle(result.Error!);
            return;
        }

        _notificationsVisible = false;
        _userNotifications.Remove(notification);
        NavigationManager.NavigateTo($"/declaration/{notification.DeclarationId}", forceLoad: true);
    }
}