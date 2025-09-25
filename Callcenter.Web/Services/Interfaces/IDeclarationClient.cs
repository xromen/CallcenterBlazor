using Callcenter.Shared;
using Callcenter.Shared.Requests;
using Callcenter.Shared.Responses;
using Refit;

namespace Callcenter.Web.Services.Interfaces;

public interface IDeclarationClient
{
    [Post("/declarations")]
    Task<ApiResponse<DeclarationDto>> Add(
        [Body] DeclarationDto dto,
        CancellationToken cancellationToken = default
    );
    
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

    [Put("/declarations/{id}")]
    Task<ApiResponse<DeclarationActionDto>> UpdateDeclaration(
        int id,
        [Body(BodySerializationMethod.Serialized)]
        DeclarationDto declaration,
        CancellationToken cancellationToken = default
    );

    [Multipart]
    [Post("/declarations/{id}/file")]
    Task<ApiResponse<object>> UploadFile(
        int id,
        [AliasAs("file")] StreamPart file,
        CancellationToken cancellation = default
    );
    
    [Get("/declarations/file/{fileId}")]
    Task<ApiResponse<HttpContent>> DownloadFile(int fileId, CancellationToken cancellationToken = default);
    
    [Get("/declarations/findByEjogNumber")]
    Task<ApiResponse<DeclarationDto>> FindByEjogNumber(
        [AliasAs("ejogNumber")] string ejogNumber,
        CancellationToken cancellationToken = default);
    
    [Get("/declarations/findByFio")]
    Task<ApiResponse<List<int>>> GetDeclarationIds(
        [AliasAs("firstName")] string firstName,
        [AliasAs("secName")] string secName,
        [AliasAs("birthDate")] string birthDate,
        CancellationToken cancellationToken = default);
    
    [Post("/declarations/{id}/supervisorClose")]
    Task<ApiResponse<object>> SupervisorClose(
        int id,
        [Body(BodySerializationMethod.Serialized)] SupervisorCloseDto dto,
        CancellationToken cancellationToken = default);
    
    [Get("/declarations/usersToSend")]
    Task<ApiResponse<List<UserToSendDto>>> GetUsersToSend(
        CancellationToken cancellationToken = default);
    
    [Post("/declarations/{id}/send")]
    Task<ApiResponse<object>> SendDeclaration(
        int id,
        SendDeclarationRequestDto dto,
        CancellationToken cancellationToken = default);
    
    [Delete("/declarations/{id}")]
    Task<ApiResponse<object>> RemoveDeclaration(
        int id,
        CancellationToken cancellationToken = default);

    [Get("/dictionaries")]
    Task<ApiResponse<DictionariesDto>> GetDictionaries(
        CancellationToken cancellationToken = default
    );

    [Get("/questions/groups")]
    Task<ApiResponse<List<QuestionGroupDto>>> GetQuestionGroups(
        CancellationToken cancellationToken = default
    );

    [Post("/questions/groups/create")]
    Task<ApiResponse<QuestionGroupDto>> CreateQuestionGroup(
        [Body(BodySerializationMethod.Serialized)]
        CreateQuestionGroupRequest request,
        CancellationToken cancellationToken = default
    );

    [Patch("/questions/groups/{id}/rename")]
    Task<ApiResponse<QuestionGroupDto>> RenameQuestionGroup(
        int id,
        [Body(BodySerializationMethod.Serialized)]
        RenameQuestionGroupRequest name,
        CancellationToken cancellationToken = default
    );

    [Patch("/questions/{id}/answer")]
    Task<ApiResponse<QuestionDto>> ChangeQuestionAnswer(
        int id,
        [Body(BodySerializationMethod.Serialized)]
        ChangeQuestionAnswerRequest answer,
        CancellationToken cancellationToken = default
    );

    [Post("/questions/create")]
    Task<ApiResponse<QuestionDto>> CreateQuestion(
        [Body(BodySerializationMethod.Serialized)]
        QuestionCreateRequest request,
        CancellationToken cancellationToken = default
    );

    [Delete("/questions/{id}")]
    Task<ApiResponse<QuestionDto>> DeleteQuestion(
        int id,
        CancellationToken cancellationToken = default
    );
}