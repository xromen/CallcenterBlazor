using System.Text.Json;
using Blazored.LocalStorage;
using CSharpFunctionalExtensions;
using Callcenter.Web.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace Callcenter.Web.Services.Authentication;

public class AuthenticationService(
    HttpClient client,
    AuthenticationStateProvider authStateProvider,
    ILocalStorageService localStorage)
{
    public const string AccessTokenKey = "authToken";
    public const string RefreshTokenKey = "refreshToken";

    public async Task<Result> LoginAsync(UserDto user, CancellationToken cancellationToken = default)
    {
        using var requestContent = new FormUrlEncodedContent([new("grant_type", "password"), new("username", user.Login), new("password", user.Password)]);

        var result = await AuthenticateAsync(requestContent, cancellationToken);

        return await result.Tap(async () => await ((AuthStateProvider)authStateProvider).NotifyUserAuthentication());
    }

    public async Task<Result> LogoutAsync(CancellationToken cancellationToken = default)
    {
        await localStorage.RemoveItemsAsync([AccessTokenKey, RefreshTokenKey], cancellationToken);
        await ((AuthStateProvider)authStateProvider).NotifyUserAuthentication();
        return Result.Success();
    }

    public async Task<Result<string>> RefreshTokenAsync(CancellationToken cancellationToken = default)
    {
        var token = await localStorage.GetItemAsync<string>(AccessTokenKey, cancellationToken);
        var refreshToken = await localStorage.GetItemAsync<string>(RefreshTokenKey, cancellationToken);

        if (token == null || refreshToken == null)
        {
            return Result.Failure<string>("Не удалось загрузить токен доступа или токен обновления.");
        }

        using var requestContent = new FormUrlEncodedContent([new("grant_type", "refresh_token"), new("refresh_token", refreshToken)]);

        client.DefaultRequestHeaders.Authorization = new("Bearer", token);
        var result = await AuthenticateAsync(requestContent, cancellationToken);
        return result.Map(data => data.AccessToken);
    }

    private async Task<Result<AuthData>> AuthenticateAsync(FormUrlEncodedContent requestContent, CancellationToken cancellationToken = default)
    {
        var response = await client.PostAsync(new Uri("connect/token", UriKind.Relative), requestContent, cancellationToken);
        client.DefaultRequestHeaders.Clear();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        
        if (response.IsSuccessStatusCode == false)
        {
            var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(content);
            var error = problemDetails?.Title ?? "Не удалось получить данные авторизации.";
            return Result.Failure<AuthData>(error);
        }

        var result = JsonSerializer.Deserialize<AuthData>(content);

        if (result == null)
        {
            return Result.Failure<AuthData>("Некорректный ответ от сервера.");
        }

        await localStorage.SetItemAsync(AccessTokenKey, result.AccessToken, cancellationToken);
        await localStorage.SetItemAsync(RefreshTokenKey, result.RefreshToken, cancellationToken);
        return result;
    }
}