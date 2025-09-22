using Callcenter.Api.Data;
using Callcenter.Api.Models;
using Callcenter.Api.Models.Exceptions;
using Callcenter.Shared;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Callcenter.Api.Services;

public class QuestionsService(ApplicationDbContext dbContext, RequestEnvironment environment)
{
    public async Task<List<QuestionGroupDto>> GetGroups(CancellationToken cancellationToken)
    {
        if (environment.AuthUser == null)
        {
            throw new PermissionException("Не авторизованный доступ");
        }
        
        var groups = await dbContext.QuestionGroups
            .Include(c => c.Questions)
            .Where(c => c.OrganisationId == Math.Max(environment.AuthUser.OrgId, 5))
            .ToListAsync(cancellationToken);
        
        return groups.Adapt<List<QuestionGroupDto>>();
    }
}