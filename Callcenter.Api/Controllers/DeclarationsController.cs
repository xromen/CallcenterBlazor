using System.Text.Json;
using Callcenter.Api.Services;
using Callcenter.Shared;
using Callcenter.Shared.Requests;
using Callcenter.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using OpenIddict.Validation.AspNetCore;

namespace Callcenter.Api.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
[Route("[controller]")]
public class DeclarationsController(DeclarationService service) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(DeclarationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add([FromBody] DeclarationDto dto, CancellationToken cancellationToken)
    {
        var result = await service.Add(dto, cancellationToken);
        
        return Ok(result);
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(GetDeclarationsResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetDeclarations([FromQuery] PaginatedRequestDto request, CancellationToken cancellationToken)
    {
        var result = await service.GetDeclarations(request.Page, request.PageSize, request.Filter, cancellationToken);
        
        return Ok(result);
    }
    
    [HttpGet("{id:int}/history")]
    [ProducesResponseType(typeof(List<DeclarationActionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetDeclarationHistory(int id, CancellationToken cancellationToken)
    {
        var result = await service.GetDeclarationHistory(id, cancellationToken);
        
        if(result == null)
            return NotFound();
        
        return Ok(result);
    }
    
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(GetDeclarationsResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetDeclaration(int id, CancellationToken cancellationToken)
    {
        var result = await service.GetDeclarationById(id, cancellationToken);
        
        if(result == null)
            return NotFound();
        
        return Ok(result);
    }
    
    [HttpGet("findByFio")]
    [ProducesResponseType(typeof(List<int>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetDeclarationIds([FromQuery] string firstName, [FromQuery] string secName, [FromQuery] DateOnly birthDate, CancellationToken cancellationToken)
    {
        var result = await service.GetDeclarationIdsByFio(firstName, secName, birthDate, cancellationToken);
        
        if(result == null)
            return NotFound();
        
        return Ok(result);
    }
    
    [HttpGet("findByEjogNumber")]
    [ProducesResponseType(typeof(DeclarationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> FindDeclarationByEjog([FromQuery] string ejogNumber, CancellationToken cancellationToken)
    {
        var result = await service.FindDeclarationByEjogNumber(ejogNumber, cancellationToken);
        
        return Ok(result);
    }
    
    [HttpPost("all/findByFilters")]
    [ProducesResponseType(typeof(PaginatedResponseDto<DeclarationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchByFilters([FromBody] SearchRequestDto request, CancellationToken cancellationToken = default)
    {
        var result = await service.SearchAllByFilters(request.PaginatedRequest.Page, request.PaginatedRequest.PageSize,
            request.Filters, request.Orders, cancellationToken);
        return Ok(result);
    }
    
    [HttpPost("all/excelExport")]
    [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> AllExcelExport(
        [FromBody] ExcelExportRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var bytes = await service.AllExportExcel(request.Filters, request.Orders, cancellationToken);
        return File(bytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "Выгрузка.xlsx");
    }

    [HttpPost("all/excelExportFull")] [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> AllExcelExportFull(
        CancellationToken cancellationToken = default
    )
    {
        var bytes = await service.AllExportExcelFull(cancellationToken);
        return File(bytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "Выгрузка.xlsx");
    }
    
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(GetDeclarationsResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateDeclaration(int id, DeclarationDto dto, CancellationToken cancellationToken)
    {
        var result = await service.Update(id, dto, cancellationToken);
        
        if(result == null)
            return NotFound();
        
        return Ok(result);
    }
    
    [HttpPost("{id:int}/send")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SendDeclaration(int id, SendDeclarationRequestDto dto, CancellationToken cancellationToken)
    {
        await service.Send(id, dto, cancellationToken);
        
        return Ok();
    }
    
    [HttpPost("{id:int}/file")]
    [ProducesResponseType(typeof(GetDeclarationsResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddDeclarationFile(int id, IFormFile file, CancellationToken cancellationToken)
    {
        await service.AddFile(id, file, cancellationToken);
        
        return Ok();
    }
    
    [HttpGet("file/{fileId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DownloadDeclarationFile(int fileId, CancellationToken cancellationToken)
    {
        var file = await service.GetFile(fileId, cancellationToken);
        
        var provider = new FileExtensionContentTypeProvider();

        if (!provider.TryGetContentType(file.Name, out var contentType))
        {
            contentType = "application/octet-stream";
        }
        
        return File(file.Stream, contentType, file.Name);
    }
    
    [HttpPost("{id:int}/supervisorClose")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> SupervisorClose(int id, [FromBody] SupervisorCloseDto dto, CancellationToken cancellationToken)
    {
        await service.SupervisorClose(id, dto, cancellationToken);

        return Ok();
    }
    
    [HttpGet("usersToSend")]
    [ProducesResponseType(typeof(List<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUsersToSend(CancellationToken cancellationToken)
    {
        var users = await service.GetUsersToSend( cancellationToken);

        return Ok(users);
    }
    
    [HttpDelete("{id:int}")]
    [ProducesResponseType( StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> Remove(int id, CancellationToken cancellationToken)
    {
        await service.Remove(id, cancellationToken);

        return Ok();
    }
}