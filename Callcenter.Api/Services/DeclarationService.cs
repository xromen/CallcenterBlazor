using Callcenter.Api.Data;
using Callcenter.Api.Data.Entities;
using Callcenter.Api.Extensions;
using Callcenter.Api.Models;
using Callcenter.Api.Models.Exceptions;
using Callcenter.Shared;
using Callcenter.Shared.Requests;
using Callcenter.Shared.Responses;
using Dapper;
using Mapster;
using Microsoft.EntityFrameworkCore;
using OpenIddict.EntityFrameworkCore.Models;

namespace Callcenter.Api.Services;

public class DeclarationService(
    ApplicationDbContext dbContext, 
    RequestEnvironment environment, 
    FileStorageService fileStorageService,
    ExcelService excelService)
{
    private IQueryable<Declaration> _baseDeclarationsQuery => dbContext.Declarations
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
        .Include(c => c.Code)
        .ThenInclude(c => c.OrganisationName);
    public async Task<GetDeclarationsResponseDto> GetDeclarations(int page, int pageSize, string? filter, CancellationToken cancellationToken = default)
    {
        if (environment.AuthUser == null)
        {
            throw new PermissionException("Не авторизованный доступ");
        }
        
        var listModels = await dbContext.Database
            .SqlQueryRaw<GetRkListModel>("select * from sp_get_rk_list({0}, {1}, {2}, {3})", environment.AuthUser!.Id, pageSize, page, filter)
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
        var declaration = await _baseDeclarationsQuery.SingleOrDefaultAsync(d => d.Id == id, cancellationToken);
        
        return declaration?.Adapt<DeclarationDto>();
    }

    public async Task<List<DeclarationActionDto>?> GetDeclarationHistory(int id, CancellationToken cancellationToken)
    {
        var histories = await dbContext.DeclarationHistories
            .Include(d => d.User)
            .ThenInclude(c => c.Group)
            .Where(c => c.DeclarationId == id)
            .ToListAsync(cancellationToken);
        
        return histories.Adapt<List<DeclarationActionDto>>();
    }

    public async Task<DeclarationDto> Update(int id, DeclarationDto dto, CancellationToken cancellationToken)
    {
        if (environment.AuthUser == null)
        {
            throw new PermissionException("Не авторизованный доступ");
        }
        
        var exists = await dbContext.Declarations.AnyAsync(d => d.Id == id, cancellationToken);

        if (!exists)
        {
            throw new EntityNotFoundException();
        }
        
        dto.Id = id;
        
        if (dto.AnswerStatusId == 1) //Если промежуточный ответ
        {
            dto.StatusId = 4; //отправлен промежуточный ответ
        }
        if (dto.AnswerStatusId == 2) //Если окончательный ответ
        {
            dto.StatusId = 5; //отправлен ответ
        }

        int answerSmo = 0;
        if (dto.AnswerStatusId == 2) //Если окончательный ответ
        {
            if (dto.CodeId > 4 && environment.AuthUser.OrgId < 5)
            {
                answerSmo = environment.AuthUser.OrgId;
            }

            dto.AnswerUserId ??= environment.AuthUser.Id;
        }
        else
        {
            dto.AnswerUserId = null;
        }
        
        var conn = dbContext.Database.GetDbConnection();

        var declarations = await conn.QueryAsync<Declaration>(@$"select * from sp_update_rk({environment.AuthUser.Id},
            @Id,
            @StatusId,
            @TypeId,
            @ContactFormId,
            @CitizenCategoryId,
            @DateRegistered,
            @DateRegisteredSmo,
            '', --fio
            @FirstName,
            @SecName,
            @FathName,
            @BirthDate,
            @ResidenceAddress,
            @Phone,
            @Email,
            @InsuredSmoId,
            @InsuredMoId,
            @Theme,
            @WorkDone,
            @AnswerDate,
            @ResultId,
            @ClosedDate,
            @AnswerStatusId,
            @SourceId,
            @Content,
            @InsuredEnp,
            @IdentityDocType,
            @IdentityDocSeries,
            @IdentityDocNumber,
            @SvedJalId,
            @SupervisorDate,
            @AnswerOrgId,
            @AnswerUserId,
            @SupervisorSmoDate,
            @MpTypeId,
            @RedirectReasonId,
            @MoPhoneNumber,
            @KemTypeId,
            @EjogNumber,
            @SvoStatusId,
            @AgentSecName)", dto);
        
        var entity = await dbContext.Declarations
            .Include(d => d.Files)
            .SingleAsync(d => d.Id == dto.Id, cancellationToken);
        
        entity.Files = dto.Files.Adapt<List<DeclarationFile>>();
        
        dbContext.Declarations.Update(entity);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return declarations.First().Adapt<DeclarationDto>();
    }

    public async Task<DeclarationDto> Add(DeclarationDto dto, CancellationToken cancellationToken)
    {
        if (environment.AuthUser == null)
        {
            throw new PermissionException("Не авторизованный доступ");
        }

        dto.AnswerUserId = 0;
        dto.StatusId = 2;

        if (dto.AnswerStatusId == 1) //Если промежуточный ответ
        {
            dto.StatusId = 4; //отправлен промежуточный ответ
        }
        if (dto.AnswerStatusId == 2) //Если окончательный ответ
        {
            dto.StatusId = 5; //отправлен ответ
            dto.AnswerUserId = environment.AuthUser.Id;
        }
        
        dto.CodeId = environment.AuthUser.Organisation.OrganisationName.Id;
        
        var conn = dbContext.Database.GetDbConnection();

        var declarations = await conn.QueryAsync<Declaration>(@$"select * from sp_add_rk({environment.AuthUser.Id},
            @StatusId,
            @CodeId,
            @TypeId,
            @ContactFormId,
            @CitizenCategoryId,
            @DateRegistered,
            @DateRegisteredSmo,
            null, --fio
            @FirstName,
            @SecName,
            @FathName,
            @BirthDate,
            @ResidenceAddress,
            @Phone,
            @Email,
            @InsuredSmoId,
            @InsuredMoId,
            @Theme,
            @WorkDone,
            @AnswerDate,
            @ResultId,
            @ClosedDate,
            @AnswerStatusId,
            @SourceId,
            @Content,
            @InsuredEnp,
            @IdentityDocType,
            @IdentityDocSeries,
            @IdentityDocNumber,
            @SvedJalId,
            @SupervisorDate,
            {environment.AuthUser.Id}, --creatorId
            @AnswerUserId,
            @SupervisorSmoDate,
            @MpTypeId,
            @RedirectReasonId,
            @MoPhoneNumber,
            @KemTypeId,
            @EjogNumber,
            @SvoStatusId,
            @AgentSecName)", dto);
        
        return declarations.First().Adapt<DeclarationDto>();
    }

    public async Task AddFile(int declarationId, IFormFile file, CancellationToken cancellationToken)
    {
        if (environment.AuthUser == null)
        {
            throw new PermissionException("Не авторизованный доступ");
        }
        
        var uniqFileName = Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName);
        
        using (var stream = new MemoryStream())
        {
            await file.CopyToAsync(stream, cancellationToken);
            
            stream.Position = 0;
            
            await fileStorageService.UploadFileAsync(uniqFileName, stream, cancellationToken);
        }

        await dbContext.Database.ExecuteSqlRawAsync("select * from sp_add_file({0}, {1}, {2}, {3})", 
            file.FileName, 
            environment.AuthUser.Id,
            declarationId,
            uniqFileName);
    }

    public async Task<FileModel> GetFile(int fileId, CancellationToken cancellationToken)
    {
        var fileEntity = await dbContext.Files.SingleOrDefaultAsync(c => c.Id == fileId, cancellationToken);

        if (fileEntity == null)
        {
            throw new EntityNotFoundException();
        }
        
        var fileStream = await fileStorageService.GetFileAsync(fileEntity.NameReal, cancellationToken);
        
        return new()
        {
            Name = fileEntity.Name,
            Stream = fileStream,
        };
    }

    public async Task<IEnumerable<int>> GetDeclarationIdsByFio(string firstName, string secName, DateOnly birthDate, CancellationToken cancellationToken)
    {
        var ids = await dbContext.Declarations
            .Where(c => 
                EF.Functions.ILike(c.FirstName, firstName) && 
                EF.Functions.ILike(c.SecName, secName) && 
                c.BirthDate == birthDate)
            .Select(c => c.Id)
            .ToListAsync(cancellationToken);
        
        return ids;
    }

    public async Task<DeclarationDto?> FindDeclarationByEjogNumber(string ejogNumber, CancellationToken cancellationToken)
    {
        var declaration = await dbContext.Declarations
            .FirstOrDefaultAsync(c => c.EjogNumber == ejogNumber, cancellationToken);
        
        return declaration.Adapt<DeclarationDto?>();
    }

    public async Task SupervisorClose(int declarationId, SupervisorCloseDto dto, CancellationToken cancellationToken)
    {
        if (environment.AuthUser == null)
        {
            throw new PermissionException("Не авторизованный доступ");
        }
        
        await dbContext.Database
            .ExecuteSqlRawAsync("select sp_close_ruk({0}, {1}, {2}, {3})", environment.AuthUser.Id, declarationId, dto.Notes, dto.IsBad ? 1 : 0);
    }

    public async Task<List<UserDto>> GetUsersToSend(CancellationToken cancellationToken)
    {
        var users = await dbContext.Users
            .Include(c => c.Group)
            .Include(c => c.Organisation)
            .Where(c => c.IsEnabled)
            .Where(c => c.Group.Name.Contains("1") || c.Group.Name.Contains("2"))
            .ToListAsync(cancellationToken);
        
        return users.Adapt<List<UserDto>>();
    }

    public async Task Send(int id, SendDeclarationRequestDto dto, CancellationToken cancellationToken)
    {
        if (environment.AuthUser == null)
        {
            throw new PermissionException("Не авторизованный доступ");
        }
        
        await dbContext.Database
            .ExecuteSqlRawAsync("select sp_send_card({0}, {1}, {2}, {3}, {4})", id, dto.OrganisationId, dto.OperatorLevel, environment.AuthUser.Id, dto.UserToSendId);
    }

    public async Task Remove(int id, CancellationToken cancellationToken)
    {
        if (environment.AuthUser == null)
        {
            throw new PermissionException("Не авторизованный доступ");
        }
        
        await dbContext.Database
            .ExecuteSqlRawAsync("select sp_add_card_for_delete({0}, {1}, {2})", id, environment.AuthUser.FullName, environment.ClientIp?.ToString());
    }

    public async Task<PaginatedResponseDto<DeclarationDto>> SearchAllByFilters(int page,
        int size,
        IEnumerable<FilterRequestDto>? filters,
        IEnumerable<OrderRequestDto>? orders,
        CancellationToken cancellationToken = default)
    {
        var baseQuery = BaseQueryForDeclarationsAll().ProjectToType<DeclarationDto>();
        
        var filtered = filters != null ? baseQuery.ApplyFilters(filters) : baseQuery;

        var totalFiltered = await filtered.CountAsync(cancellationToken);

        var ordered = orders != null && orders.Any() ?
            filtered.ApplyOrders(orders) :
            filtered.OrderByDescending(c => c.Id);

        var paginateQuery = ordered.Skip(page * size);
        if (size != 0)
        {
            paginateQuery = paginateQuery.Take(size);
        }

        var declarations = await paginateQuery.ToListAsync(cancellationToken);
        var dtos = declarations.Adapt<List<DeclarationDto>>();
        return new(page, totalFiltered, dtos);
    }
    
    public async Task<byte[]> AllExportExcel(
        IEnumerable<FilterRequestDto>? filters,
        IEnumerable<OrderRequestDto>? orders,
        CancellationToken cancellationToken = default)
    {
        var items = (await SearchAllByFilters(1, 0, filters, orders, cancellationToken)).Items;

        return await excelService.ExportAsync(items.Adapt<List<Models.Excel.Declaration>>(), false, cancellationToken);
    }
    
    public async Task<byte[]> AllExportExcelFull(CancellationToken cancellationToken = default)
    {
        if (environment.AuthUser == null)
        {
            throw new PermissionException("Не авторизованный доступ");
        }
        
        IQueryable<Declaration> query;

        if (environment.AuthUser.GroupId == 5)
        {
            query = _baseDeclarationsQuery;
        }
        else
        {
            query = _baseDeclarationsQuery.Where(c => (c.HaveOrgId ?? c.CodeId) == environment.AuthUser.OrgId);
        }
        
        var items = await query.ToListAsync(cancellationToken);

        return await excelService.ExportAsync(items.Adapt<List<Models.Excel.Declaration>>(), false, cancellationToken);
    }

    private IQueryable<Declaration> BaseQueryForDeclarationsAll()
    {
        if (environment.AuthUser == null)
        {
            throw new PermissionException("Не авторизованный доступ");
        }
        
        IQueryable<int> rkQuery;
        IQueryable<Declaration> baseQyery = _baseDeclarationsQuery.AsNoTracking();
        
        var user = environment.AuthUser;

        if (user.OrgId == 5)
        {
            // Маппинг групп -> какие группы брать для поиска rk
            var groupMap = new Dictionary<int[], int[]>
            {
                { new [] { 1, 2 }, new [] { 1, 2 } },
                { new [] { 10, 11 }, new [] { 10, 11 } },
                { new [] { 7, 8 }, new [] { 7, 8 } },
                { new [] { 12 }, new [] { 10, 11 } },
                { new [] { 6 }, new [] { 1, 2 } },
                { new [] { 9 }, new [] { 7, 8 } }
            };

            // если группа 4 → отдельный случай
            if (user.GroupId == 4)
            {
                return baseQyery
                    .OrderByDescending(r => r.Id);
            }

            // ищем соответствие по маппингу
            var targetGroups = groupMap
                .FirstOrDefault(x => x.Key.Contains(user.GroupId))
                .Value;

            if (targetGroups != null)
            {
                var userIds = dbContext.Users
                    .Where(u => targetGroups.Contains(u.GroupId) && u.OrgId == user.OrgId)
                    .Select(u => u.Id);

                rkQuery = dbContext.DeclarationPermissions
                    .Where(p => userIds.Contains(p.UserId))
                    .Select(p => p.DeclarationId);
            }
            else
            {
                rkQuery = Enumerable.Empty<int>().AsQueryable();
            }
        }
        else
        {
            var userIds = dbContext.Users
                .Where(u => u.OrgId == user.OrgId)
                .Select(u => u.Id);

            rkQuery = dbContext.DeclarationPermissions
                .Where(p => userIds.Contains(p.UserId))
                .Select(p => p.DeclarationId);
        }

        return baseQyery
            .Where(r => rkQuery.Contains(r.Id))
            .OrderByDescending(r => r.Id);
    }
}