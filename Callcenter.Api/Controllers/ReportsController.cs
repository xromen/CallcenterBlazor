using Callcenter.Api.Services;
using Callcenter.Shared.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace Callcenter.Api.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Policy = "admin")]
[Route("[controller]")]
public class ReportsController(ReportsService service) : ControllerBase
{
    /// <summary>
    /// Выгрузка ПГ Общая в формате xlsx
    /// </summary>
    /// <param name="request">Запрос для выгрузки</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Файл Excel</returns>
    [HttpGet("PgFormGeneral")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPgFormGeneral([FromQuery] FormRequestDto request, CancellationToken cancellationToken)
    {
        var report = await service.GetPgFormGeneral(request.From, request.To, cancellationToken);
        
        return File(report, 
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "ПГ общая.xlsx");
    }
    
    /// <summary>
    /// Выгрузка ПГ в разрезе МО
    /// </summary>
    /// <param name="request">Запрос для выгрузки</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Файл Excel</returns>
    [HttpGet("PgFormGroupByMo")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPgFormGroupByMo([FromQuery] FormRequestDto request, CancellationToken cancellationToken)
    {
        var report = await service.GetPgFormGroupByMo(request.From, request.To, cancellationToken);
        
        return File(report, 
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "ПГ в разрезе МО.xlsx");
    }
    
    /// <summary>
    /// Выгрузка критериев эффективности ОМС
    /// </summary>
    /// <param name="month">Месяц выгрузки</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Файл Excel</returns>
    [HttpGet("OmsPerformanceCriteriaForm")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetOmsPerformanceCriteriaForm([FromQuery] int month, CancellationToken cancellationToken)
    {
        var report = await service.GetOmsPerformanceCriteriaForm(month, cancellationToken);
        
        return File(report, 
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "ПГ в разрезе МО.xlsx");
    }
    
    /// <summary>
    /// Выгрузка обоснованных жалоб
    /// </summary>
    /// <param name="request">Запрос для выгрузки</param>
    /// <param name="cancellationToken">Токена отмены</param>
    /// <returns>Файл Excel</returns>
    [HttpGet("JustifiedComplaintsForm")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetJustifiedComplaintsForm([FromQuery] FormRequestDto request, CancellationToken cancellationToken)
    {
        var report = await service.GetJustifiedComplaintsForm(request.From, request.To, cancellationToken);
        
        return File(report, 
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "ПГ в разрезе МО.xlsx");
    }
    
    /// <summary>
    /// Выгрзка всех жалоб
    /// </summary>
    /// <param name="request">Запрос для выгрузки</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Файл Excel</returns>
    [HttpGet("AllComplaintsForm")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllComplaintsForm([FromQuery] FormRequestDto request, CancellationToken cancellationToken)
    {
        var report = await service.GetAllComplaintsForm(request.From, request.To, cancellationToken);
        
        return File(report, 
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "ПГ в разрезе МО.xlsx");
    }
}