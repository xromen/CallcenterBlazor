using Callcenter.Shared;
using Callcenter.Shared.Requests;
using Callcenter.Shared.Responses;
using Refit;

namespace Callcenter.Web.Services.Interfaces;

public interface IDeclarationClient
{
    [Get("/declarations")]
    Task<ApiResponse<GetDeclarationsResponseDto>> GetAll(
        PaginatedRequestDto? paginate = null,
        CancellationToken cancellationToken = default
    );
    
    [Get("/declarations/{id}")]
    Task<ApiResponse<DeclarationDto>> GetById(
        int id,
        CancellationToken cancellationToken = default
    );
    
    [Get("/declarations/{id}/history")]
    Task<ApiResponse<List<DeclarationActionDto>>> GetHistory(
        int id,
        CancellationToken cancellationToken = default
    );
    
    [Get("/dictionaries")]
    Task<ApiResponse<DictionariesDto>> GetDictionaries(
        CancellationToken cancellationToken = default
    );
    
    [Get("/questions/groups")]
    Task<ApiResponse<List<QuestionGroupDto>>> GetQuestionGroups(
        CancellationToken cancellationToken = default
    );
}