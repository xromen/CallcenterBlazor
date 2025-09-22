using Callcenter.Web.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Callcenter.Web.Services;

public class ProblemDetailsHandler(ISnackbar snackbar, NavigationManager navigation)
{
    public void Handle(ProblemDetails details)
    {
        var message = new MarkupString($"""
                                        {details.Title} <br/>
                                        requestId: {details.RequestId}
                                        """);

        snackbar.Add(message, Severity.Warning, configure => { configure.VisibleStateDuration = 10000; });

        if(details.Status == 401)
        {
            navigation.NavigateTo("/Account/Login");
        }
    }
}