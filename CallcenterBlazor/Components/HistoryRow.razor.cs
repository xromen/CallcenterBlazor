using CallcenterBlazor.Pages;
using Microsoft.AspNetCore.Components;

namespace CallcenterBlazor.Components;

public partial class HistoryRow : ComponentBase
{
    [Parameter]
    public RkAction Action { get; set; }
}