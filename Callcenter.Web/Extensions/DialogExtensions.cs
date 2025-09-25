using Callcenter.Web.Components;
using Callcenter.Web.Components.Dialogs;
using MudBlazor;

namespace Callcenter.Web.Extensions;

public static class DialogExtensions
{
    public static async Task<bool> ShowConfirmDialog(this IDialogService dialogService, string content, string buttonText, Color buttonColor)
    {
        var parameters = new DialogParameters<ConfirmDialog>
        {
            { x => x.ContentText, content },
            { x => x.ButtonText, buttonText },
            { x => x.Color, buttonColor }
        };

        var dialog = await dialogService.ShowAsync<ConfirmDialog>("Подтверждение", parameters);
        var result = await dialog.Result;

        return !result!.Canceled && (bool)result.Data!;
    }

    public static Task ShowErrorsDialog(this IDialogService dialogService, IEnumerable<string> errors)
    {
        DialogOptions options = new() { CloseButton = true, FullWidth = true, Position = DialogPosition.TopCenter};
        var parameters = new DialogParameters<ErrorsDialog> { { x => x.Errors, errors } };
                
        return dialogService.ShowAsync<ErrorsDialog>("Обнаружены ошибки", parameters, options);
    }

    public static async Task<bool> ShowWarningsDialog(this IDialogService dialogService, IEnumerable<string> warnings)
    {
        DialogOptions options = new() { CloseButton = true, FullWidth = true, Position = DialogPosition.TopCenter};
        var parameters = new DialogParameters<WarningsDialog> { { x => x.Warnings, warnings } };
                
        var dialog = await dialogService.ShowAsync<WarningsDialog>("Предупреждения", parameters, options);
        var result = await dialog.Result;
        
        return result!.Canceled == false && (bool)result.Data! == true;
    }
}