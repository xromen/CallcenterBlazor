using Callcenter.Web.Extensions;
using Mapster;
using Refit;

namespace Callcenter.Web.Models;

public class ApiResult<T>
{
    public bool Success => Error == null;
    public T? Data { get; }
    public ProblemDetails? Error { get; }

    private ApiResult(T? data, ProblemDetails? error)
    {
        Data = data;
        Error = error;
    }

    public static async Task<ApiResult<T>> FromResponseAsync<TIn>(ApiResponse<TIn> response)
    {
        if (response.IsSuccessStatusCode)
            return new ApiResult<T>(response.Content.Adapt<T>(), null);

        return new ApiResult<T>(default, await response.GetProblemDetailsAsync());
    }

    public static async Task<ApiResult<T>> FromResponseAsync(IApiResponse response, Func<Task<T>> data)
    {
        if (response.IsSuccessStatusCode)
            return new ApiResult<T>(await data(), null);

        return new ApiResult<T>(default, await response.GetProblemDetailsAsync());
    }
}