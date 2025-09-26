using Callcenter.Api.Data;
using Callcenter.Api.Models;
using Callcenter.Shared;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Callcenter.Api.Services;

public class DictionariesService(ApplicationDbContext dbContext)
{
    private static readonly DateOnly NullDate = new DateOnly(1900, 1, 1);
    public async Task<DictionariesDto> GetDictionaries(CancellationToken cancellationToken)
    {
        var contactForms = await dbContext.DeclarationContactForms.OrderBy(c => c.Id).ToListAsync(cancellationToken);
        var declarationThemes = await dbContext.DeclarationThemes.OrderBy(c => c.Id).ToListAsync(cancellationToken);
       

        return new()
        {
            AnswerStatuses = await dbContext.AnswerStatuses.OrderBy(c => c.Id).ToDictionaryAsync(c => c.Id, c => c.Name, cancellationToken),
            DeclarationStatuses = await dbContext.DeclarationStatuses.OrderBy(c => c.Id).ToDictionaryAsync(c => c.Id, c => c.Name, cancellationToken),
            OrganisationNames = await dbContext.OrganisationNames.OrderBy(c => c.Id).ToDictionaryAsync(c => c.OrganisationId, c => c.Name, cancellationToken),
            DeclarationTypes = await dbContext.DeclarationTypes.OrderBy(c => c.Id).ToDictionaryAsync(c => c.Id, c => c.Name, cancellationToken),
            DeclarationSources = await dbContext.DeclarationSources.OrderBy(c => c.Id).ToDictionaryAsync(c => c.Id, c => c.Name, cancellationToken),
            MoPhoneNumbers = await dbContext.MoPhoneNumbers.OrderBy(c => c.Id).ToDictionaryAsync(c => c.PhoneNumber, c => c.Name, cancellationToken),
            VerbalContactForms = contactForms.Where(c => c.Type == 1).ToDictionary(c => c.Id, c => c.Name),
            WritingContactForms = contactForms.Where(c => c.Type == 2).ToDictionary(c => c.Id, c => c.Name),
            CitizenCategories = await dbContext.CitizenCategories.OrderBy(c => c.Id).ToDictionaryAsync(c => c.Id, c => c.Name, cancellationToken),
            IdentityDocumentTypes = await dbContext.IdentityDocumentTypes.OrderBy(c => c.Id).ToDictionaryAsync(c => c.Id, c => c.Name, cancellationToken),
            SmoOrganisations = await dbContext.Organisations.OrderBy(c => c.Id)
                .Where(c => c.TypeOrgsId == 3)
                .ToDictionaryAsync(c => c.Id, c => c.Name, cancellationToken),
            DeclarationThemes = declarationThemes.GroupBy(c => c.TypeId)
                .ToDictionary(c => c.Key, c => c.Select(c => c.Name).ToList()),
            MoOrganisations = await GetMoOrganisations(cancellationToken),
            KemTypes = await dbContext.KemTypes.OrderBy(c => c.Id).ToDictionaryAsync(c => c.Id, c => c.Name, cancellationToken),
            DeclarationResults = await dbContext.DeclarationResults.OrderBy(c => c.Id).ToDictionaryAsync(c => c.Id, c => c.Name, cancellationToken),
            SvedJals = await dbContext.SvedJal.OrderBy(c => c.Id).ToDictionaryAsync(c => c.Id, c => c.Name, cancellationToken),
            RedirectReasons = await dbContext.RedirectReasons.OrderBy(c => c.Id).ToDictionaryAsync(c => c.Id, c => c.Name, cancellationToken),
        };
    }
    private class tempClass
    {
        public int mcod { get; set; }
        public string nam_mok { get; set; }
    }
    private async Task<Dictionary<string, string>> GetMoOrganisations(CancellationToken cancellationToken)
    {
        //var editsDates = (await dbContext.F003MoDocuments
        //    .Include(c => c.Mo)
        //    .Select(c => new { c.DateEdit, c.Mo.McodId })
        //    .ToListAsync())
        //    .OrderByDescending(c => c.DateEdit)
        //    .DistinctBy(c => c.McodId)
        //    .ToDictionary(c => c.McodId, c => c.DateEdit);

        //var moOrganisationsL = await dbContext.F003Mos
        //    .Include(c => c.MoDocuments)
        //    .ThenInclude(c => c.Document)
        //    .Include(c => c.Mcod)
        //    .Where(c => c.MoDocuments.Any(d => d.DateEdit == editsDates[c.McodId]))
        //    .Where(c => c.MoDocuments.Any(d => d.Document.DateEnd == null || d.Document.DateEnd != NullDate))
        //    .Where(c => c.Mcod.OmsPriz)
        //    .OrderBy(c => c.Mcod.Mcod)
        //    .ToListAsync();

        //var moOrganisationsL = await dbContext.F003MoDocuments
        //    .Include(c => c.Mo)
        //    .ThenInclude(c => c.Mcod)
        //    .Include(c => c.Document)
        //    .Where(doc =>
        //            // берём только записи с максимальной датой по каждой МО
        //            doc.DateEdit == dbContext.F003MoDocuments
        //                .Where(x => x.MoId == doc.MoId)
        //                .Max(x => (DateTime?)x.DateEdit)
        //        )
        //    .Where(d => d.Document.DateEnd == null || d.Document.DateEnd != NullDate)
        //    .Where(c => c.Mo.Mcod.OmsPriz)
        //    .OrderBy(c => c.Mo.Mcod.Mcod)
        //    .ToListAsync();

        var moOrganisations = await dbContext.Database
            .SqlQueryRaw<tempClass>("select * from site_mo_select_top()")
            .ToDictionaryAsync(c => c.mcod.ToString(), c => c.nam_mok);

        //var moOrganisations = moOrganisationsL.ToDictionary(c => c.mcod.ToString(), c => c.nam_mok);
        //var moOrganisations = moOrganisationsL.ToDictionary(c => c.Mcod.Mcod.ToString(), c => c.Name);
        
        var moDeparts = await dbContext.MoDepartments.ToDictionaryAsync(c => c.MoCode, c => c.Name, cancellationToken);

        foreach (var depart in moDeparts)
        {
            moOrganisations.TryAdd(depart.Key, depart.Value);
        }
        
        moOrganisations = moOrganisations.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
        
        return moOrganisations;
    }
}