using Callcenter.Api.Services;
using Callcenter.Shared;
using Callcenter.Shared.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace Callcenter.Api.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
[Route("[controller]")]
public class QuestionsController(QuestionsService service) : ControllerBase
{
    /// <summary>
    /// Получение групп вопросов и самих вопросов в группах
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список групп с вопросами</returns>
    [HttpGet("groups")]
    [ProducesResponseType(typeof(List<QuestionGroupDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetGroups(CancellationToken cancellationToken)
    {
        var result = await service.GetGroups(cancellationToken);
        
        return Ok(result);
    }
    
    /// <summary>
    /// Создание новой группы вопросов
    /// </summary>
    /// <param name="request">Запрос на создание группы</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Созданная группа</returns>
    [HttpPost("groups/create")]
    [ProducesResponseType(typeof(QuestionGroupDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateGroup([FromBody] CreateQuestionGroupRequest request, CancellationToken cancellationToken)
    {
        var result = await service.CreateGroup(request.Name, cancellationToken);
        
        return Ok(result);
    }
    
    /// <summary>
    /// Смена имени группы вопросов
    /// </summary>
    /// <param name="id">Идентификатор группы</param>
    /// <param name="request">Запрос для смены имени</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Измененная группа вопросов</returns>
    [HttpPatch("groups/{id:int}/rename")]
    [ProducesResponseType(typeof(QuestionGroupDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RenameGroup(int id, [FromBody] RenameQuestionGroupRequest request, CancellationToken cancellationToken)
    {
        var result = await service.RenameGroup(id, request.Name, cancellationToken);
        
        return Ok(result);
    }
    
    /// <summary>
    /// Изменить ответ на вопрос
    /// </summary>
    /// <param name="id">Идентификатор вопроса</param>
    /// <param name="request">Запроса на смену ответа</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Измененный вопрос</returns>
    [HttpPatch("{id:int}/answer")]
    [ProducesResponseType(typeof(QuestionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangeQuestionAnswer(int id, [FromBody] ChangeQuestionAnswerRequest request, CancellationToken cancellationToken)
    {
        var result = await service.ChangeQuestionAnswer(id, request.Answer, cancellationToken);
        
        return Ok(result);
    }
    
    /// <summary>
    /// Создание нового вопроса
    /// </summary>
    /// <param name="request">Запрос для создания вопроса</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Созданный вопрос</returns>
    [HttpPost("create")]
    [ProducesResponseType(typeof(QuestionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateQuestion([FromBody] QuestionCreateRequest request, CancellationToken cancellationToken)
    {
        var result = await service.CreateQuestion(request, cancellationToken);
        
        return Ok(result);
    }
    
    /// <summary>
    /// Удаление вопроса
    /// </summary>
    /// <param name="id">Идентификатор вопроса</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Удаленный вопрос</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(QuestionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteQuestion(int id, CancellationToken cancellationToken)
    {
        var result = await service.DeleteQuestion(id, cancellationToken);
        
        return Ok(result);
    }
}