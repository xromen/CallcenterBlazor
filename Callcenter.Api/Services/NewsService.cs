using Callcenter.Api.Data;
using Callcenter.Api.Data.Entities;
using Callcenter.Api.Models;
using Callcenter.Api.Models.Exceptions;
using Callcenter.Shared;
using Callcenter.Shared.Responses;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Callcenter.Api.Services;

public class NewsService(ApplicationDbContext dbContext, RequestEnvironment environment)
{
    public async Task<PaginatedResponseDto<NewsDto>> GetNews(int page, int pageSize, CancellationToken cancellationToken)
    {
        if (environment.AuthUser == null)
        {
            throw new PermissionException("Не авторизованный доступ");
        }
        
        var query = dbContext.News
            .Include(c => c.CreatedBy)
            .Include(c => c.Organisations)
            .Where(c => c.Organisations.Select(o => o.Id).Contains(environment.AuthUser.OrgId))
            .OrderByDescending(c => c.Date);
        
        var totalCount = await query.CountAsync(cancellationToken);

        var news = await query
            .Skip(pageSize * page)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new(page, totalCount, news.Adapt<List<NewsDto>>());
    }

    public async Task<NewsDto> Create(NewsDto dto, CancellationToken cancellationToken)
    {
        if (environment.AuthUser == null)
        {
            throw new PermissionException("Не авторизованный доступ");
        }
        
        var entity = dto.Adapt<News>();

        entity.CreatorUserId = environment.AuthUser.Id;
        entity.Organisations = await dbContext.Organisations
            .Where(o => dto.OrganisationIds.Contains(o.Id))
            .ToListAsync(cancellationToken);
        
        dbContext.News.Add(entity);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return entity.Adapt<NewsDto>();
    }

    public async Task<NewsDto> Update(int id, NewsDto dto, CancellationToken cancellationToken)
    {
        var dbEntity = await dbContext.News.SingleOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (dbEntity == null)
        {
            throw new EntityNotFoundException();
        }
        
        dbEntity.Text = dto.Text;

        dbEntity.Organisations = await dbContext.Organisations
            .Where(o => dto.OrganisationIds.Contains(o.Id))
            .ToListAsync(cancellationToken);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return dbEntity.Adapt<NewsDto>();
    }

    public async Task<NewsDto> Delete(int id, CancellationToken cancellationToken)
    {
        var dbEntity = await dbContext.News
            .Include(c => c.Organisations)
            .SingleOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (dbEntity == null)
        {
            throw new EntityNotFoundException();
        }

        dbContext.News.Remove(dbEntity);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return dbEntity.Adapt<NewsDto>();
    }
}