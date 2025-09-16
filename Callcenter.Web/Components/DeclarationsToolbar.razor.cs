using Microsoft.AspNetCore.Components;

namespace Callcenter.Web.Components;

public partial class DeclarationsToolbar : ComponentBase
{
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    
    [Parameter] public RenderFragment? ChildContent { get; set; }
    
    private List<DeclarationsTab> _tabs = [
        new()
        {
            Name = "Мои обращения",
            Href = "/declarations"
        },
        new()
        {
            Name = "Все обращения",
            Href = "/declarations_all"
        },
        new()
        {
            Name = "Дубликаты",
            Href = "/declarations_double"
        },
    ];

    protected override void OnInitialized()
    {
        base.OnInitialized();
        
        var relativeUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);

        foreach (var tab in _tabs)
        {
            if (relativeUrl.Equals(tab.Href.TrimStart('/'), StringComparison.InvariantCultureIgnoreCase))
            {
                tab.IsActive = true;
            }
        }
    }

    private void TabClick(DeclarationsTab tab)
    {
        NavigationManager.NavigateTo(tab.Href);
    }
}

public class DeclarationsTab
{
    public string Name { get; set; }
    public string Href { get; set; }
    public bool IsActive { get; set; } = false;
}