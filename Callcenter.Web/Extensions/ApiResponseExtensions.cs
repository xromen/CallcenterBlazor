using System.Text.Json;
using Callcenter.Web.Models;
using Refit;
using ProblemDetails = Callcenter.Web.Models.ProblemDetails;

namespace Callcenter.Web.Extensions;

public static class ApiResponseExtensions
{
    public static async Task<ProblemDetails?> GetProblemDetailsAsync(this IApiResponse response)
    {
        if (response.IsSuccessStatusCode || response.Error == null)
            return null;

        try
        {
            return JsonSerializer.Deserialize<ProblemDetails>(response.Error.Content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch
        {
            return null; // если тело не ProblemDetails
        }
    }
}