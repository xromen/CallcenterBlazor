using Callcenter.Shared;
using Callcenter.Shared.Requests;
using Callcenter.Shared.Responses;
using Refit;

namespace Callcenter.Web.Services.Interfaces;

public interface IApiClient
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
    
    [Post("/declarations/all/findByFilters")]
    Task<ApiResponse<PaginatedResponseDto<DeclarationDto>>> SearchAll(
        [Body(BodySerializationMethod.Serialized)] SearchRequestDto request,
        CancellationToken cancellationToken = default
    );
    
    [Post("/declarations/all/excelExport")]
    Task<ApiResponse<HttpContent>> AllExcelExport(
        [Body(BodySerializationMethod.Serialized)] ExcelExportRequest request,
        CancellationToken cancellationToken = default
    );
    
    [Post("/declarations/all/excelExportFull")]
    Task<ApiResponse<HttpContent>> AllExcelExportFull(
        CancellationToken cancellationToken = default
    );
    
    [Post("/declarations/{id}/supervisorClose")]
    Task<ApiResponse<object>> SupervisorClose(
        int id,
        [Body(BodySerializationMethod.Serialized)] SupervisorCloseDto dto,
        CancellationToken cancellationToken = default);
    
    [Get("/declarations/usersToSend")]
    Task<ApiResponse<List<UserDto>>> GetUsersToSend(
        CancellationToken cancellationToken = default);
    
    [Post("/declarations/{id}/send")]
    Task<ApiResponse<object>> SendDeclaration(
        int id,
        SendDeclarationRequestDto dto,
        CancellationToken cancellationToken = default);
    
    [Post("/declarations/identPerson")]
    Task<ApiResponse<List<IdentedPersonDto>>> IdentPerson(
        [Body(BodySerializationMethod.Serialized)] IdentPersonRequestDto request,
        CancellationToken cancellationToken = default);
    
    [Delete("/declarations/{id}")]
    Task<ApiResponse<object>> RemoveDeclaration(
        int id,
        CancellationToken cancellationToken = default);

    [Get("/dictionaries")]
    Task<ApiResponse<DictionariesDto>> GetDictionaries(
        CancellationToken cancellationToken = default
    );
    
    //Вопросы ответы

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
    
    //Новости
    
    [Post("/news")]
    Task<ApiResponse<NewsDto>> CreateNews(
        [Body(BodySerializationMethod.Serialized)] NewsDto dto,
        CancellationToken cancellationToken = default);
    
    [Get("/news")]
    Task<ApiResponse<PaginatedResponseDto<NewsDto>>> GetNews(
        PaginatedRequestDto request,
        CancellationToken cancellationToken = default);
    
    [Put("/news/{id}")]
    Task<ApiResponse<NewsDto>> UpdateNews(
        int id,
        [Body(BodySerializationMethod.Serialized)] NewsDto dto,
        CancellationToken cancellationToken = default);
    
    [Delete("/news/{id}")]
    Task<ApiResponse<NewsDto>> DeleteNews(
        int id,
        CancellationToken cancellationToken = default);
    
    //Аккаунт
    
    [Post("/accounts/create")]
    Task<ApiResponse<object>> CreateUser(
        [Body(BodySerializationMethod.Serialized)] UserCreateDto dto,
        CancellationToken cancellationToken = default);
    
    [Get("/accounts")]
    Task<ApiResponse<List<UserDto>>> GetUsers(
        CancellationToken cancellationToken = default);
    
    [Get("/accounts/{id}")]
    Task<ApiResponse<UserDto>> GetUserById(
        int id,
        CancellationToken cancellationToken = default);
    
    [Get("/accounts/groups")]
    Task<ApiResponse<List<UserGroupDto>>> GetUserGroups(
        CancellationToken cancellationToken = default);
    
    [Put("/accounts/{id}")]
    Task<ApiResponse<object>> UpdateUser(
        int id,
        [Body(BodySerializationMethod.Serialized)] UserCreateDto dto,
        CancellationToken cancellationToken = default);
    
    [Delete("/accounts/{id}")]
    Task<ApiResponse<UserDto>> DeleteUser(
        int id,
        CancellationToken cancellationToken = default);
    
    //Отчеты
    
    [Get("/reports/PgFormGeneral")]
    Task<ApiResponse<HttpContent>> GetPgFormGeneral(
        DateOnly from,
        DateOnly to,
        CancellationToken cancellationToken = default);
    
    [Get("/reports/PgFormGroupByMo")]
    Task<ApiResponse<HttpContent>> GetPgFormGroupByMo(
        DateOnly from,
        DateOnly to,
        CancellationToken cancellationToken = default);
    
    [Get("/reports/OmsPerformanceCriteriaForm")]
    Task<ApiResponse<HttpContent>> GetOmsPerformanceCriteriaForm(
        int month,
        CancellationToken cancellationToken = default);
    
    [Get("/reports/JustifiedComplaintsForm")]
    Task<ApiResponse<HttpContent>> GetJustifiedComplaintsForm(
        DateOnly from,
        DateOnly to,
        CancellationToken cancellationToken = default);
    
    [Get("/reports/AllComplaintsForm")]
    Task<ApiResponse<HttpContent>> GetAllComplaintsForm(
        DateOnly from,
        DateOnly to,
        CancellationToken cancellationToken = default);
    
}