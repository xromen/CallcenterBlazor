using Callcenter.Api.Data;
using Callcenter.Api.Models;
using Callcenter.Shared;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Callcenter.Api.Services;

public class DictionariesService(ApplicationDbContext dbContext)
{
    public async Task<DictionariesDto> GetDictionaries(CancellationToken cancellationToken)
    {
        var contactForms = await dbContext.DeclarationContactForms.AsNoTracking().OrderBy(c => c.Id).ToListAsync(cancellationToken);
        var declarationThemes = await dbContext.DeclarationThemes.AsNoTracking().OrderBy(c => c.Id).ToListAsync(cancellationToken);
       
        return new()
        {
            AnswerStatuses = await dbContext.AnswerStatuses.AsNoTracking().OrderBy(c => c.Id).ToDictionaryAsync(c => c.Id, c => c.Name, cancellationToken),
            DeclarationStatuses = await dbContext.DeclarationStatuses.AsNoTracking().OrderBy(c => c.Id).ToDictionaryAsync(c => c.Id, c => c.Name, cancellationToken),
            OrganisationNames = await dbContext.OrganisationNames.AsNoTracking().OrderBy(c => c.Id).ToDictionaryAsync(c => c.OrganisationId, c => c.Name, cancellationToken),
            DeclarationTypes = await dbContext.DeclarationTypes.AsNoTracking().OrderBy(c => c.Id).ToDictionaryAsync(c => c.Id, c => c.Name, cancellationToken),
            DeclarationSources = await dbContext.DeclarationSources.AsNoTracking().OrderBy(c => c.Id).ToDictionaryAsync(c => c.Id, c => c.Name, cancellationToken),
            MoPhoneNumbers = await dbContext.MoPhoneNumbers.AsNoTracking().OrderBy(c => c.Id).ToDictionaryAsync(c => c.PhoneNumber, c => c.Name, cancellationToken),
            VerbalContactForms = contactForms.Where(c => c.Type == 1).ToDictionary(c => c.Id, c => c.Name),
            WritingContactForms = contactForms.Where(c => c.Type == 2).ToDictionary(c => c.Id, c => c.Name),
            CitizenCategories = await dbContext.CitizenCategories.AsNoTracking().OrderBy(c => c.Id).ToDictionaryAsync(c => c.Id, c => c.Name, cancellationToken),
            IdentityDocumentTypes = await dbContext.IdentityDocumentTypes.AsNoTracking().OrderBy(c => c.Id).ToDictionaryAsync(c => c.Id, c => c.Name, cancellationToken),
            Organisations = await dbContext.Organisations.AsNoTracking().OrderBy(c => c.Id)
                .ToDictionaryAsync(c => c.Id, c => c.Name, cancellationToken), 
            SmoOrganisations = await dbContext.Organisations.AsNoTracking().OrderBy(c => c.Id)
                .Where(c => c.TypeOrgsId == 3)
                .ToDictionaryAsync(c => c.Id, c => c.Name, cancellationToken),
            DeclarationThemes = declarationThemes.GroupBy(c => c.TypeId)
                .ToDictionary(c => c.Key, c => c.Select(c => c.Name).ToList()),
            MoOrganisations = await GetMoOrganisations(cancellationToken),
            KemTypes = await dbContext.KemTypes.AsNoTracking().OrderBy(c => c.Id).ToDictionaryAsync(c => c.Id, c => c.Name, cancellationToken),
            DeclarationResults = await dbContext.DeclarationResults.AsNoTracking().OrderBy(c => c.Id).ToDictionaryAsync(c => c.Id, c => c.Name, cancellationToken),
            SvedJals = await dbContext.SvedJal.AsNoTracking().OrderBy(c => c.Id).ToDictionaryAsync(c => c.Id, c => c.Name, cancellationToken),
            RedirectReasons = await dbContext.RedirectReasons.AsNoTracking().OrderBy(c => c.Id).ToDictionaryAsync(c => c.Id, c => c.Name, cancellationToken),
            MpTypes = await dbContext.MpTypes.AsNoTracking().OrderBy(c => c.Id).ToDictionaryAsync(c => c.Id, c => c.Name, cancellationToken),
        };
    }
    private class tempClass
    {
        public int mcod { get; set; }
        public string nam_mok { get; set; }
    }
    private async Task<Dictionary<string, string>> GetMoOrganisations(CancellationToken cancellationToken)
    {
        var moOrganisations = await dbContext.Database
            .SqlQueryRaw<tempClass>("select * from site_mo_select_top()")
            .ToDictionaryAsync(c => c.mcod.ToString(), c => c.nam_mok, cancellationToken);
        
        var moDeparts = await dbContext.MoDepartments.ToDictionaryAsync(c => c.MoCode, c => c.Name, cancellationToken);

        foreach (var depart in moDeparts)
        {
            moOrganisations.TryAdd(depart.Key, depart.Value);
        }
        
        moOrganisations = moOrganisations.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
        
        return moOrganisations;
    }
}