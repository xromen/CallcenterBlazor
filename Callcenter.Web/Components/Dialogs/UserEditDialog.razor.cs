using Callcenter.Shared;
using Callcenter.Web.Models;
using Callcenter.Web.Pages;
using Callcenter.Web.Services;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace Callcenter.Web.Components.Dialogs;

public partial class UserEditDialog : ComponentBase
{
    [Parameter] 
    public int? UserId { get; set; }
    
    private UserCreateDto _user { get; set; } = new();
    
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; }
    
    [Inject] private ISnackbar Snackbar { get; set; }
    
    [Inject] private DeclarationsService DeclarationsService { get; set; }
    
    [Inject] private AccountsService AccountsService { get; set; }
    
    [Inject] private ProblemDetailsHandler ProblemDetailsHandler { get; set; }

    private Dictionary<int, string> _orgs = new();
    private List<UserGroupDto> _userGroups = new();

    protected override async Task OnInitializedAsync()
    {
        var dictResult = await DeclarationsService.GetDictionaries();

        if (!dictResult.Success)
        {
            ProblemDetailsHandler.Handle(dictResult.Error!);
            return;
        }
        
        _orgs = dictResult.Data!.Organisations;
        
        var userGroupsResult = await AccountsService.GetUserGroups();
        
        if (!userGroupsResult.Success)
        {
            ProblemDetailsHandler.Handle(userGroupsResult.Error!);
            return;
        }
        
        _userGroups = userGroupsResult.Data!;

        if (UserId.HasValue)
        {
            var userResult = await AccountsService.GetById(UserId.Value);
            
            if (!userResult.Success)
            {
                ProblemDetailsHandler.Handle(dictResult.Error!);
                return;
            }
            
            _user = userResult.Data!.Adapt<UserCreateDto>();
        }
        
        StateHasChanged();
    }
    
    private void Cancel() => MudDialog.Cancel();

    private async Task Delete()
    {
        if(UserId == null)
            return;
        
        var result = await AccountsService.Delete(UserId.Value);

        if (!result.Success)
        {
            ProblemDetailsHandler.Handle(result.Error!);
            return;
        }
        
        Snackbar.Add("Пользователь удален", Severity.Success);
        MudDialog.Close();
    }

    private async Task Save()
    {
        ApiResult result;
        
        if (UserId.HasValue)
        {
            result = await AccountsService.Update(UserId.Value, _user);
        }
        else
        {
            result = await AccountsService.Create(_user);
        }
        
        if (!result.Success)
        {
            ProblemDetailsHandler.Handle(result.Error!);
            return;
        }
        
        Snackbar.Add("Пользователь сохранен", Severity.Success);
        MudDialog.Close();
    }

    private void UserOrgIdChanged(int? id)
    {
        _user.OrgId = id;
        _user.GroupId = null;
    }
}