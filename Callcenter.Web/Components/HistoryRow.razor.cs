using Callcenter.Web.Pages;
using Microsoft.AspNetCore.Components;

namespace Callcenter.Web.Components;

public partial class HistoryRow : ComponentBase
{
    [Parameter]
    public RkAction Action { get; set; }
}