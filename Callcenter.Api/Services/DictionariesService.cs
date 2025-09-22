using Callcenter.Api.Data;
using Callcenter.Shared;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Callcenter.Api.Services;

public class DictionariesService(ApplicationDbContext dbContext)
{
    private static readonly DateOnly NullDate = new DateOnly(1900, 1, 1);
    public async Task<DictionariesDto> GetDictionaries(CancellationToken cancellationToken)
    {
        var contactForms = await dbContext.DeclarationContactForms.ToListAsync(cancellationToken);
        var declarationThemes = await dbContext.DeclarationThemes.ToListAsync(cancellationToken);
       

        return new()
        {
            AnswerStatuses = await dbContext.AnswerStatuses.ToDictionaryAsync(c => c.Id, c => c.Name, cancellationToken),
            DeclarationStatuses = await dbContext.DeclarationStatuses.ToDictionaryAsync(c => c.Id, c => c.Name, cancellationToken),
            OrganisationNames = await dbContext.OrganisationNames.ToDictionaryAsync(c => c.OrganisationId, c => c.Name, cancellationToken),
            DeclarationTypes = await dbContext.DeclarationTypes.ToDictionaryAsync(c => c.Id, c => c.Name, cancellationToken),
            DeclarationSources = await dbContext.DeclarationSources.ToDictionaryAsync(c => c.Id, c => c.Name, cancellationToken),
            MoPhoneNumbers = await dbContext.MoPhoneNumbers.ToDictionaryAsync(c => c.PhoneNumber, c => c.Name, cancellationToken),
            VerbalContactForms = contactForms.Where(c => c.Type == 1).ToDictionary(c => c.Id, c => c.Name),
            WritingContactForms = contactForms.Where(c => c.Type == 2).ToDictionary(c => c.Id, c => c.Name),
            CitizenCategories = await dbContext.CitizenCategories.ToDictionaryAsync(c => c.Id, c => c.Name, cancellationToken),
            IdentityDocumentTypes = await dbContext.IdentityDocumentTypes.ToDictionaryAsync(c => c.Id, c => c.Name, cancellationToken),
            SmoOrganisations = await dbContext.Organisations
                .Where(c => c.TypeOrgsId == 3)
                .ToDictionaryAsync(c => c.Id, c => c.Name, cancellationToken),
            DeclarationThemes = declarationThemes.GroupBy(c => c.TypeId)
                .ToDictionary(c => c.Key, c => c.Select(c => c.Name).ToList()),
            MoOrganisations = await GetMoOrganisations(cancellationToken),
            KemTypes = await dbContext.KemTypes.ToDictionaryAsync(c => c.Id, c => c.Name, cancellationToken),
            DeclarationResults = await dbContext.DeclarationResults.ToDictionaryAsync(c => c.Id, c => c.Name, cancellationToken),
            SvedJals = await dbContext.SvedJal.ToDictionaryAsync(c => c.Id, c => c.Name, cancellationToken),
            RedirectReasons = await dbContext.RedirectReasons.ToDictionaryAsync(c => c.Id, c => c.Name, cancellationToken),
        };
    }

    private async Task<Dictionary<string, string>> GetMoOrganisations(CancellationToken cancellationToken)
    {
        var moOrganisations = await dbContext.F003Mos
            .Include(c => c.MoDocuments)
            .ThenInclude(c => c.Document)
            .Include(c => c.Mcod)
            .Where(c => c.MoDocuments.Any(d => d.Document.DateEnd == null || d.Document.DateEnd != NullDate))
            .Where(c => c.Mcod.OmsPriz)
            .OrderBy(c => c.Mcod.Mcod)
            .ToDictionaryAsync(c => c.Mcod.Mcod.ToString(), c => c.Name, cancellationToken);
        
        var moDeparts = await dbContext.MoDepartments.ToDictionaryAsync(c => c.MoCode, c => c.Name, cancellationToken);

        foreach (var depart in moDeparts)
        {
            moOrganisations.TryAdd(depart.Key, depart.Value);
        }
        
        moOrganisations = moOrganisations.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
        
        return moOrganisations;
    }
}