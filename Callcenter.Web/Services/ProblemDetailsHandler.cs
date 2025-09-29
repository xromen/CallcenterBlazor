using System.Text;
using Callcenter.Web.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Callcenter.Web.Services;

public class ProblemDetailsHandler(ISnackbar snackbar, NavigationManager navigation)
{
    public void Handle(ProblemDetails details)
    {
        if(details.Status == 401)
        {
            navigation.NavigateTo("/Account/Login");
        }
        else
        {
            var messageSb = new StringBuilder($"""
                                                 {details.Title} <br/>
                                                 requestId: {details.RequestId} <br/>
                                                 """);

            if (details.Errors != null && details.Errors.Any())
            {
                foreach (var error in details.Errors)
                {
                    messageSb.Append(error.Key + ": <br/>");
                    foreach (var errorText in error.Value)
                    {
                        messageSb.Append($"    {errorText} <br/>");
                    }
                }
            }
            
            var message = new MarkupString(messageSb.ToString());
            
            snackbar.Add(message, Severity.Warning, configure => { configure.VisibleStateDuration = 10000; });
        }
    }
}