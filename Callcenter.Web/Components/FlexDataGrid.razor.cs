using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Callcenter.Web.Components;

[CascadingTypeParameter("T")]
public partial class FlexDataGrid<T> : ComponentBase
{
    [Parameter]
    public EventCallback<DataGridRowClickEventArgs<T>> RowClick { get; set; }
    
    [Parameter]
    public Func<GridState<T>, Task<GridData<T>>> ServerData { get; set; }
    
    [Parameter]
    public RenderFragment Columns { get; set; }
    
    [Parameter]
    public RenderFragment PagerContent { get; set; }
    
    private MudDataGrid<T> _mudDataGrid;
    private string SelectedRowClassFunc(T element, int rowNumber)
    {
        return _mudDataGrid.SelectedItems.Contains(element) ? "selected" : "";
    }
}