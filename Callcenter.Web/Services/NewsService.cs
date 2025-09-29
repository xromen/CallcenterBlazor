using Callcenter.Shared;
using Callcenter.Shared.Requests;
using Callcenter.Shared.Responses;
using Callcenter.Web.Models;
using Callcenter.Web.Pages;
using Callcenter.Web.Services.Interfaces;
using Refit;

namespace Callcenter.Web.Services;

public class NewsService(IApiClient apiClient)
{
    public async Task<ApiResult<NewsDto>> CreateNews(NewsDto newsDto, CancellationToken cancellationToken = default)
    {
        var response = await apiClient.CreateNews(newsDto, cancellationToken);
        
        return await ApiResult<NewsDto>.FromResponseAsync(response);
    }
    
    public async Task<ApiResult<PaginatedResponseDto<NewsDto>>> GetNews(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var request = new PaginatedRequestDto()
        {
            Page = page - 1,
            PageSize = pageSize
        };
        
        var response = await apiClient.GetNews(request, cancellationToken);
        
        return await ApiResult<PaginatedResponseDto<NewsDto>>.FromResponseAsync(response);
    }
    
    public async Task<ApiResult<NewsDto>> UpdateNews(int id, NewsDto newsDto, CancellationToken cancellationToken = default)
    {
        var response = await apiClient.UpdateNews(id, newsDto, cancellationToken);
        
        return await ApiResult<NewsDto>.FromResponseAsync(response);
    }
    
    public async Task<ApiResult<NewsDto>> DeleteNews(int id, CancellationToken cancellationToken = default)
    {
        var response = await apiClient.DeleteNews(id, cancellationToken);
        
        return await ApiResult<NewsDto>.FromResponseAsync(response);
    }
}