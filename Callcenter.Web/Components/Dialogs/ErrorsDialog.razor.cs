using Microsoft.AspNetCore.Components;

namespace Callcenter.Web.Components.Dialogs;

public partial class ErrorsDialog : ComponentBase
{
    [Parameter]
    public IEnumerable<string> Errors { get; set; } = new List<string>();
}