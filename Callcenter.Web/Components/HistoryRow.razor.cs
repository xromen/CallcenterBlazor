using Callcenter.Shared;
using Callcenter.Web.Pages;
using Microsoft.AspNetCore.Components;

namespace Callcenter.Web.Components;

public partial class HistoryRow : ComponentBase
{
    [Parameter]
    public DeclarationActionDto Action { get; set; }
}