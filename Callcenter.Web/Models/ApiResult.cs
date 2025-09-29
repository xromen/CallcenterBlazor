using Callcenter.Web.Extensions;
using Mapster;
using Refit;

namespace Callcenter.Web.Models;

public class ApiResult
{
    public bool Success => Error == null;
    public ProblemDetails? Error { get; }
    
    protected ApiResult(ProblemDetails? error)
    {
        Error = error;
    }
    
    public static async Task<ApiResult> FromResponseAsync(IApiResponse response)
    {
        if (response.IsSuccessStatusCode)
            return new ApiResult(null);

        return new ApiResult(await response.GetProblemDetailsAsync());
    }
}

public class ApiResult<T> : ApiResult
{
    public T? Data { get; }

    private ApiResult(T? data, ProblemDetails? error) : base(error)
    {
        Data = data;
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