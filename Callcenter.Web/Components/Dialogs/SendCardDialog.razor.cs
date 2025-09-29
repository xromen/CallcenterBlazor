using Callcenter.Shared;
using Callcenter.Web.Models;
using Callcenter.Web.Pages;
using Callcenter.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using UserDto = Callcenter.Shared.UserDto;

namespace Callcenter.Web.Components.Dialogs;

public partial class SendCardDialog : ComponentBase
{
    [Parameter] public int DeclarationId { get; set; }
    
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; }
    
    [Inject] private ISnackbar Snackbar { get; set; }
    
    [Inject] private DeclarationsService Service { get; set; }
    
    [Inject] private ProblemDetailsHandler ProblemDetailsHandler { get; set; }
    
    private Dictionary<int, string> _organisations = new Dictionary<int, string>();
    private List<UserDto> _users = new List<UserDto>();

    private int? _selectedOrgId;
    private int? _selectedOperatorLevel;
    private int? _selectedUserId;
    
    private bool IsSendDisabled => _selectedOrgId.HasValue || _selectedOperatorLevel.HasValue ||  _selectedUserId.HasValue;

    protected override async Task OnInitializedAsync()
    {
        var result = await Service.GetUsersToSend();

        if (!result.Success)
        {
            ProblemDetailsHandler.Handle(result.Error!);
            return;
        }
        
        _users = result.Data!;
        _organisations = result.Data!.GroupBy(c => c.OrgId).ToDictionary(g => g.Key, g => g.First().OrgName);
        
        await base.OnInitializedAsync();
    }

    private void Cancel() => MudDialog.Cancel();

    private async Task Send()
    {
        var result = await Service.SendDeclaration(DeclarationId, _selectedOrgId!.Value, _selectedOperatorLevel!.Value, _selectedUserId!.Value);

        if (!result.Success)
        {
            ProblemDetailsHandler.Handle(result.Error!);
            MudDialog.Cancel();
        }
        
        Snackbar.Add("Карточка отправлена", Severity.Success);
        MudDialog.Close();
    }

    private void ExpandedOrgChanged(bool value, int orgId)
    {
        if (value)
        {
            _selectedOrgId = orgId;
        }
        
        _selectedOperatorLevel = null;
        _selectedUserId = null;
    }

    private void OperatorLevelChanged(int? obj)
    {
        _selectedOperatorLevel = obj;
        _selectedUserId = null;
    }
}