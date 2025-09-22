using Callcenter.Api.Data;
using Callcenter.Api.Models;
using Callcenter.Api.Models.Exceptions;
using Callcenter.Shared;
using Callcenter.Shared.Responses;
using Mapster;
using Microsoft.EntityFrameworkCore;
using OpenIddict.EntityFrameworkCore.Models;

namespace Callcenter.Api.Services;

public class DeclarationService(ApplicationDbContext dbContext, RequestEnvironment environment)
{
    public async Task<GetDeclarationsResponseDto> GetDeclarations(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        if (environment.AuthUser == null)
        {
            throw new PermissionException("Не авторизованный доступ");
        }
        
        var listModels = await dbContext.Database
            .SqlQueryRaw<GetRkListModel>("select * from sp_get_rk_list({0}, {1}, {2})", environment.AuthUser!.Id, pageSize, page)
            .ToListAsync(cancellationToken);

        var listDto = listModels.Adapt<List<GetRkListDto>>();
        
        var firstListModel = listModels.FirstOrDefault();
        
        return new()
        {
            Declarations = listDto,
            TotalDeclarationsItems = firstListModel?.TotalItems ?? 0,
            Statistics = new()
            {
                NeedReworkCount = firstListModel?.NeedReworkCount ?? 0,
                SendAnswerCount = firstListModel?.SendAnswerCount ?? 0,
                SmoRedirectCount = firstListModel?.SmoRedirectCount ?? 0,
            }
        };
    }

    public async Task<DeclarationDto?> GetDeclarationById(int id, CancellationToken cancellationToken = default)
    {
        var declaration = await dbContext.Declarations
            .Include(d => d.Status)
            .Include(d => d.Type)
            .Include(d => d.ContactForm)
            .Include(d => d.CitizenCategory)
            .Include(d => d.InsuredSmo)
            .Include(d => d.Result)
            .Include(d => d.AnswerStatus)
            .Include(d => d.Source)
            .Include(d => d.SvedJal)
            .Include(d => d.Creator)
            .Include(d => d.AnswerOrg)
            .Include(d => d.AnswerUser)
            .Include(d => d.HaveOrg)
            .Include(d => d.MpType)
            .Include(d => d.RedirectReason)
            .Include(d => d.KemType)
            .Include(d => d.SvoStatus)
            .Include(d => d.MoPhone)
            .Include(c => c.Files)
            .SingleOrDefaultAsync(d => d.Id == id, cancellationToken);
        
        return declaration?.Adapt<DeclarationDto>();
    }

    public async Task<List<DeclarationActionDto>?> GetDeclarationHistory(int id, CancellationToken cancellationToken)
    {
        var declaration = await dbContext.Declarations
            .Include(declaration => declaration.History)
            .ThenInclude(c => c.User)
            .SingleOrDefaultAsync(c => c.Id == id, cancellationToken);
        
        return declaration?.History.Adapt<List<DeclarationActionDto>>();
    }
}