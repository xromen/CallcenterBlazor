using Callcenter.Api.Data;
using Callcenter.Api.Data.Entities;
using Callcenter.Api.Models;
using Callcenter.Api.Models.Exceptions;
using Callcenter.Shared;
using Callcenter.Shared.Requests;
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

    public async Task<QuestionGroupDto> RenameGroup(int id, string name, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new IncorrectDataException("Имя не может быть пустым");
        }
        
        var entity = await dbContext.QuestionGroups
            .Include(c => c.Questions)
            .SingleOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (entity == null)
        {
            throw new EntityNotFoundException();
        }
        
        entity.Name = name;
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return entity.Adapt<QuestionGroupDto>();
    }

    public async Task<QuestionDto> ChangeQuestionAnswer(int id, string answer, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(answer))
        {
            throw new IncorrectDataException("Ответ не может быть пустым");
        }
        
        var entity = await dbContext.Questions.SingleOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (entity == null)
        {
            throw new EntityNotFoundException();
        }
        
        entity.Answer = answer;
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return entity.Adapt<QuestionDto>();
    }

    public async Task<QuestionDto> DeleteQuestion(int id, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Questions.SingleOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (entity == null)
        {
            throw new EntityNotFoundException();
        }

        dbContext.Remove(entity);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return entity.Adapt<QuestionDto>();
    }

    public async Task<QuestionGroupDto> CreateGroup(string name, CancellationToken cancellationToken)
    {
        if (environment.AuthUser == null)
        {
            throw new PermissionException("Не авторизованный доступ");
        }

        var entity = new QuestionGroup()
        {
            Name = name,
            OrganisationId = environment.AuthUser.OrgId,
        };
        
        await dbContext.QuestionGroups.AddAsync(entity, cancellationToken);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return entity.Adapt<QuestionGroupDto>();
    }

    public async Task<QuestionDto> CreateQuestion(QuestionCreateRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Answer))
        {
            throw new IncorrectDataException("Имя вопроса и ответ не должны быть пустыми");
        }
        
        var group = await dbContext.QuestionGroups.SingleOrDefaultAsync(c => c.Id == request.GroupId, cancellationToken);

        if (group == null)
        {
            throw new EntityNotFoundException();
        }

        var question = new Question()
        {
            GroupId = group.Id,
            Name = request.Name,
            Answer = request.Answer,
        };
        
        await dbContext.Questions.AddAsync(question, cancellationToken);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return question.Adapt<QuestionDto>();
    }
}