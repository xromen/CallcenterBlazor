using Callcenter.Api.Data.Entities;
using Callcenter.Api.Models;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;

namespace Callcenter.Api.Middlewares;

public class AuthMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(
        HttpContext context,
        UserManager<User> userManager,
        RequestEnvironment environment)
    {
        environment.ClientIp = context.Connection.RemoteIpAddress;
        
        var userId = context.User.GetClaim(OpenIddictConstants.Claims.Subject);

        if (userId != null && int.TryParse(userId, out var id))
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                environment.AuthUser = user;
            }
        }

        await next(context);
    }
}
