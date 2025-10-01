using Callcenter.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Callcenter.Web.Components.Dialogs;

public partial class SupervisorCommentsDialog : ComponentBase
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; }

    [Parameter] public IEnumerable<SupervisorCommentDto> Comments { get; set; } = new List<SupervisorCommentDto>();

    private void Cancel() => MudDialog.Cancel();
}