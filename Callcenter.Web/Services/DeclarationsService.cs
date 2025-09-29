using Callcenter.Shared;
using Callcenter.Shared.Requests;
using Callcenter.Shared.Responses;
using Callcenter.Web.Models;
using Callcenter.Web.Services.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Components.Forms;
using Refit;
using UserDto = Callcenter.Shared.UserDto;

namespace Callcenter.Web.Services;

public class DeclarationsService(IApiClient apiClient)
{
    public async Task<ApiResult<DeclarationDto>> Add(DeclarationModel model, CancellationToken cancellationToken = default)
    {
        var dto = model.Adapt<DeclarationDto>();
        var response = await apiClient.Add(dto, cancellationToken);
        
        return await ApiResult<DeclarationDto>.FromResponseAsync(response);
    }
    
    public async Task<ApiResult<GetDeclarationsResponseDto>> GetList(int pageSize, int page, string? filter = null, CancellationToken cancellationToken = default)
    {
        var request = new PaginatedRequestDto()
        {
            PageSize = pageSize,
            Page = page,
            Filter = filter
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
    public async Task<ApiResult<DeclarationActionDto>> UpdateDeclaration(int id, DeclarationDto dto, CancellationToken cancellationToken = default)
    {
        var response = await apiClient.UpdateDeclaration(id, dto, cancellationToken);
        
        return await ApiResult<DeclarationActionDto>.FromResponseAsync(response);
    }
    public async Task UploadFile(int id, IBrowserFile file, CancellationToken cancellationToken = default)
    {
        var stream = file.OpenReadStream(maxAllowedSize: 10_000_000_000);
        var streamPart = new StreamPart(stream, file.Name, file.ContentType);
        
        var response = await apiClient.UploadFile(id, streamPart, cancellationToken);
    }
    public async Task<ApiResult<byte[]>> DownloadFile(int fileId, CancellationToken cancellationToken = default)
    {
        var response = await apiClient.DownloadFile(fileId, cancellationToken);
        
        return await ApiResult<byte[]>
            .FromResponseAsync(response, () => response.Content!.ReadAsByteArrayAsync(cancellationToken));
    }
    public async Task<ApiResult<List<int>>> FindDeclarationsByFio(string firstName, string secName, DateTime birthDate, CancellationToken cancellationToken = default)
    {
        var response = await apiClient.GetDeclarationIds(firstName, secName, birthDate.ToString("o"), cancellationToken);
        
        return await ApiResult<List<int>>.FromResponseAsync(response);
    }
    public async Task<ApiResult<DeclarationDto>> FindDeclarationsByEjogNumber(string ejogNumber, CancellationToken cancellationToken = default)
    {
        var response = await apiClient.FindByEjogNumber(ejogNumber, cancellationToken);
        
        return await ApiResult<DeclarationDto>.FromResponseAsync(response);
    }
    public async Task<ApiResult<object>> SupervisorClose(int declarationId, string? notes, bool isBad, CancellationToken cancellationToken = default)
    {
        var response = await apiClient.SupervisorClose(declarationId, new()
        {
            Notes = notes,
            IsBad = isBad
        }, cancellationToken);
        
        return await ApiResult<object>.FromResponseAsync(response);
    }
    public async Task<ApiResult<List<UserDto>>> GetUsersToSend(CancellationToken cancellationToken = default)
    {
        var response = await apiClient.GetUsersToSend(cancellationToken);
        
        return await ApiResult<List<UserDto>>.FromResponseAsync(response);
    }
    public async Task<ApiResult<object>> SendDeclaration(
        int declarationId, 
        int organisationId, 
        int operatorLevel, 
        int userToSendId, 
        CancellationToken cancellationToken = default)
    {
        var request = new SendDeclarationRequestDto()
        {
            DeclarationId = declarationId,
            OrganisationId = organisationId,
            OperatorLevel = operatorLevel,
            UserToSendId = userToSendId
        };
        
        var response = await apiClient.SendDeclaration(declarationId, request, cancellationToken);
        
        return await ApiResult<object>.FromResponseAsync(response);
    }
    public async Task<ApiResult<byte[]>> AllExcelExport(
        FilterRequestDto[]? filters,
        OrderRequestDto[]? orders,
        CancellationToken cancellationToken = default)
    {
        var request = new ExcelExportRequest()
        {
            Filters = filters,
            Orders = orders,
        };
        var response = await apiClient.AllExcelExport(request, cancellationToken);
        
        return await ApiResult<byte[]>.FromResponseAsync(response, () => response.Content!.ReadAsByteArrayAsync(cancellationToken));
    }
    public async Task<ApiResult<byte[]>> AllExcelExportFull(CancellationToken cancellationToken = default)
    {
        var response = await apiClient.AllExcelExportFull(cancellationToken);
        
        return await ApiResult<byte[]>.FromResponseAsync(response, () => response.Content!.ReadAsByteArrayAsync(cancellationToken));
    }

    public async Task<ApiResult<object>> Remove(int id, CancellationToken cancellationToken = default)
    {
        var response = await apiClient.RemoveDeclaration(id, cancellationToken);
        
        return await ApiResult<object>.FromResponseAsync(response);
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

    public async Task<ApiResult<QuestionGroupDto>> CreateQuestionGroup(string name, CancellationToken cancellationToken = default)
    {
        var response = await apiClient.CreateQuestionGroup(new(){ Name = name }, cancellationToken);
        
        return await ApiResult<QuestionGroupDto>.FromResponseAsync(response);
    }

    public async Task<ApiResult<QuestionGroupDto>> RenameQuestionGroup(int id, string name, CancellationToken cancellationToken = default)
    {
        var response = await apiClient.RenameQuestionGroup(id, new() { Name = name }, cancellationToken);
        
        return await ApiResult<QuestionGroupDto>.FromResponseAsync(response);
    }

    public async Task<ApiResult<QuestionDto>> ChangeQuestionAnswer(int id, string answer, CancellationToken cancellationToken = default)
    {
        var response = await apiClient.ChangeQuestionAnswer(id, new(){Answer = answer}, cancellationToken);
        
        return await ApiResult<QuestionDto>.FromResponseAsync(response);
    }

    public async Task<ApiResult<QuestionDto>> CreateQuestion(int groupId, string questionName, string questionAnswer, CancellationToken cancellationToken = default)
    {
        var request = new QuestionCreateRequest()
        {
            GroupId = groupId,
            Answer = questionAnswer,
            Name = questionName
        };
        
        var response = await apiClient.CreateQuestion(request, cancellationToken);
        
        return await ApiResult<QuestionDto>.FromResponseAsync(response);
    }

    public async Task<ApiResult<QuestionDto>> DeleteQuestion(int id, CancellationToken cancellationToken = default)
    {
        var response = await apiClient.DeleteQuestion(id, cancellationToken);
        
        return await ApiResult<QuestionDto>.FromResponseAsync(response);
    }

    public async Task<ApiResult<PaginatedResponseDto<DeclarationDto>>> SearchAll(
        PaginatedRequestDto? paginate,
        FilterRequestDto[]? filters,
        OrderRequestDto[]? orders,
        CancellationToken cancellationToken = default)
    {
        var request = new SearchRequestDto()
        {
            PaginatedRequest = paginate,
            Filters = filters,
            Orders = orders,
        };
        var response = await apiClient.SearchAll(request, cancellationToken);
        
        return await ApiResult<PaginatedResponseDto<DeclarationDto>>.FromResponseAsync(response);
    }
}