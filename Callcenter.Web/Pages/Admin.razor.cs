using Bogus;
using Callcenter.Web.Components;
using Callcenter.Web.Components.Dialogs;
using Callcenter.Web.Models;
using Callcenter.Web.Services;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MudBlazor;
using UserDto = Callcenter.Shared.UserDto;

namespace Callcenter.Web.Pages;

public partial class Admin : ComponentBase
{
    [Inject] private IDialogService Dialog { get; set; } = null!;
    
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    
    [Inject] private AccountsService AccountsService { get; set; } = null!;
    
    [Inject] private ReportsService ReportsService { get; set; } = null!;
    
    [Inject] private ProblemDetailsHandler ProblemDetailsHandler { get; set; } = null!;
    
    [Inject] IJSRuntime Js { get; set; }
    
    private bool _isLoading = false;
    
    private List<UserDto> _users = new();
    private List<string> _userOrgs = new();

    private NewsTable _newsTable;

    private DateTime? _reportFrom;
    private DateTime? _reportTo;
    private int? _reportOmsMonth;

    private async Task TabIndexChanged(int arg)
    {
        _isLoading = true;
        
        switch (arg)
        {
            //Пользователи
            case 1:
                await UsersLoad();
                break;
        }
        
        _isLoading = false;
    }

    private async Task UsersLoad()
    {
        var result = await AccountsService.Get();

        if (!result.Success)
        {
            ProblemDetailsHandler.Handle(result.Error!);
            return;
        }

        _users = result.Data!;
        _userOrgs = _users.Select(u => u.OrgName).Distinct().ToList();
    }

    private async Task NewsRowClicked(NewsClickEventArgs arg)
    {
        DialogOptions options = new() { MaxWidth = MaxWidth.Medium, FullWidth = true, CloseButton = true };
        var parameters = new DialogParameters<NewsEditDialog> { { x => x.NewsObj, arg.Item } };
        
        var dialog = await Dialog.ShowAsync<NewsEditDialog>("Custom Options Dialog", parameters, options);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await _newsTable.LoadNews();
        }
    }

    private async Task AddNews(MouseEventArgs arg)
    {
        DialogOptions options = new() { MaxWidth = MaxWidth.Medium, FullWidth = true, CloseButton = true};
        
        var dialog = await Dialog.ShowAsync<NewsEditDialog>("Custom Options Dialog", options);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await _newsTable.LoadNews();
        }
    }

    private async Task UserCardClick(MouseEventArgs arg, UserDto user)
    {
        DialogOptions options = new() { MaxWidth = MaxWidth.Small, FullWidth = true, CloseButton = true};
        var parameters = new DialogParameters<UserEditDialog> { { x => x.UserId, user.Id} };
        
        var dialog = await Dialog.ShowAsync<UserEditDialog>("Редактирование пользователя", parameters, options);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await UsersLoad();
        }
    }

    private async Task AddUser(MouseEventArgs arg)
    {
        DialogOptions options = new() { MaxWidth = MaxWidth.Small, FullWidth = true, CloseButton = true };
        
        var dialog = await Dialog.ShowAsync<UserEditDialog>("Создание пользователя", options);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await UsersLoad();
        }
    }

    private Task MakeReportPgGeneral(MouseEventArgs arg)
    {
        return MakeReportFromTo(ReportsService.GetPgFormGeneral, "Форма ПГ общая.xlsx");
    }

    private Task MakeReportPgGroupByMo(MouseEventArgs arg)
    {
        return MakeReportFromTo(ReportsService.GetPgFormGroupByMo, "Форма ПГ в разрезе МО.xlsx");
    }

    private async Task MakeOmsPerformanceCriteria(MouseEventArgs arg)
    {
        if (_reportOmsMonth == null)
        {
            Snackbar.Add("Вначале выберите месяц", Severity.Warning);
            return;
        }

        _isLoading = true;
        try
        {
            var result = await ReportsService.GetOmsPerformanceCriteriaForm(_reportOmsMonth.Value);

            if (!result.Success)
            {
                ProblemDetailsHandler.Handle(result.Error!);
                return;
            }

            await Js.InvokeVoidAsync("saveAsFile", "Критерии эффективности ОМС.xlsx",
                Convert.ToBase64String(result.Data!));
        }
        finally
        {
            _isLoading = false;
        }
    }

    private Task MakeJustifiedComplaints(MouseEventArgs arg)
    {
        return MakeReportFromTo(ReportsService.GetJustifiedComplaintsForm, "Обоснованные жалобы по МО.xlsx");
    }

    private async Task MakeReportFromTo(Func<DateOnly, DateOnly, CancellationToken, Task<ApiResult<byte[]>>> report, string fileName)
    {
        if (_reportFrom == null || _reportTo == null)
        {
            Snackbar.Add("Вначале выберите даты", Severity.Warning);
            return;
        }
        
        _isLoading = true;
        try
        {
            var result = await report(DateOnly.FromDateTime(_reportFrom.Value),
                DateOnly.FromDateTime(_reportTo.Value), CancellationToken.None);

            if (!result.Success)
            {
                ProblemDetailsHandler.Handle(result.Error!);
                return;
            }

            await Js.InvokeVoidAsync("saveAsFile", fileName,
                Convert.ToBase64String(result.Data!));
        }
        finally
        {
            _isLoading = false;
        }
    }

    private Task MakeAllComplaints(MouseEventArgs arg)
    {
        return MakeReportFromTo(ReportsService.GetAllComplaintsForm, "Все жалобы по МО.xlsx");
    }
}