using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Callcenter.Web.Components;

[CascadingTypeParameter("T")]
public partial class FlexDataGrid<T> : ComponentBase
{
    [CascadingParameter(Name = "IsZoomed")]
    public bool IsZoomed { get; set; }
    
    [Parameter]
    public EventCallback<DataGridRowClickEventArgs<T>> RowClick { get; set; }
    
    [Parameter]
    public Func<GridState<T>, Task<GridData<T>>> ServerData { get; set; }
    
    [Parameter]
    public RenderFragment Columns { get; set; }
    
    [Parameter]
    public RenderFragment PagerContent { get; set; } 
    
    [Parameter]
    public Func<T, int, string> RowClassFunc { get; set; }

    [Parameter] public List<IFilterDefinition<T>> FilterDefinitions { get; set; } = new();

    public HashSet<T> SelectedItems => _mudDataGrid.SelectedItems;
    
    private MudDataGrid<T> _mudDataGrid;
}