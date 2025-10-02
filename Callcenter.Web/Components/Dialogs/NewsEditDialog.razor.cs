using Callcenter.Shared;
using Callcenter.Web.Pages;
using Callcenter.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace Callcenter.Web.Components.Dialogs;

public partial class NewsEditDialog : ComponentBase, IDisposable
{
    [Parameter] public NewsDto NewsObj { get; set; } = new();
    
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; }
    
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    private DeclarationsService DeclarationsService { get; set; } = null!;

    [Inject]
    private NewsService NewsService { get; set; } = null!;
    
    [Inject]
    private ProblemDetailsHandler ProblemDetailsHandler { get; set; } = null!;
    
    private Dictionary<int, string> _orgs = new ();
    
    private CancellationTokenSource _tokenSource = new();

    protected override async Task OnInitializedAsync()
    {
        var result = await DeclarationsService.GetDictionaries(_tokenSource.Token);

        if (!result.Success)
        {
            ProblemDetailsHandler.Handle(result.Error!);
            return;
        }
        
        _orgs = result.Data!.Organisations;
    }
    
    private void Cancel() => MudDialog.Cancel();

    private async Task Delete()
    {
        if (NewsObj.Id == null)
        {
            return;
        }
        
        var result = await NewsService.DeleteNews(NewsObj.Id!.Value, _tokenSource.Token);

        if (!result.Success)
        {
            ProblemDetailsHandler.Handle(result.Error!);
            return;
        }
        
        Snackbar.Add("Новость удалена", Severity.Success);
        MudDialog.Close();
    }

    private async Task Save()
    {
        if (NewsObj.Id == null)
        {
            var result = await NewsService.CreateNews(NewsObj, _tokenSource.Token);

            if (!result.Success)
            {
                ProblemDetailsHandler.Handle(result.Error!);
                return;
            }
        }
        else
        {
            var result = await NewsService.UpdateNews(NewsObj.Id.Value, NewsObj, _tokenSource.Token);

            if (!result.Success)
            {
                ProblemDetailsHandler.Handle(result.Error!);
                return;
            }
        }
        
        Snackbar.Add("Новость сохранена", Severity.Success);
        MudDialog.Close();
    } 

    private void OrgCheckedChanged(bool value, int orgId)
    {
        if (value)
        {
            NewsObj.OrganisationIds.Add(orgId);
        }
        else
        {
            NewsObj.OrganisationIds.Remove(orgId);
        }
    }

    public void Dispose()
    {
        _tokenSource.Cancel();
        _tokenSource.Dispose();
        Snackbar.Dispose();
    }
}