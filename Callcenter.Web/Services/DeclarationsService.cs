using Callcenter.Shared;
using Callcenter.Shared.Requests;
using Callcenter.Shared.Responses;
using Callcenter.Web.Models;
using Callcenter.Web.Services.Interfaces;

namespace Callcenter.Web.Services;

public class DeclarationsService(IDeclarationClient apiClient)
{
    public async Task<ApiResult<GetDeclarationsResponseDto>> GetList(int pageSize, int page, CancellationToken cancellationToken = default)
    {
        var request = new PaginatedRequestDto()
        {
            PageSize = pageSize,
            Page = page
        };
        
        var response = await apiClient.GetAll(request, cancellationToken);
        
        return await ApiResult<GetDeclarationsResponseDto>.FromResponseAsync(response);
    }
    public async Task<ApiResult<DeclarationModel>> GetById(int id, CancellationToken cancellationToken = default)
    {
        var response = await apiClient.GetById(id, cancellationToken);
        
        return await ApiResult<DeclarationModel>.FromResponseAsync(response);
    }
    public async Task<ApiResult<List<DeclarationActionDto>>> GetHistory(int id, CancellationToken cancellationToken = default)
    {
        var response = await apiClient.GetHistory(id, cancellationToken);
        
        return await ApiResult<List<DeclarationActionDto>>.FromResponseAsync(response);
    }

    public async Task<ApiResult<DictionariesDto>> GetDictionaries(CancellationToken cancellationToken = default)
    {
        var response = await apiClient.GetDictionaries(cancellationToken);
        
        return await ApiResult<DictionariesDto>.FromResponseAsync(response);
    }

    public async Task<ApiResult<List<QuestionGroupDto>>> GetQuestionGroups(CancellationToken cancellationToken = default)
    {
        var response = await apiClient.GetQuestionGroups(cancellationToken);
        
        return await ApiResult<List<QuestionGroupDto>>.FromResponseAsync(response);
    }
}