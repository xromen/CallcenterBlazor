namespace Callcenter.Shared;

public class DictionariesDto
{
    public Dictionary<int, string> AnswerStatuses { get; set; }
    
    public Dictionary<int, string> DeclarationStatuses { get; set; }
    
    public Dictionary<int, string> OrganisationNames { get; set; }
    
    public Dictionary<int, string> DeclarationTypes { get; set; }
    
    public Dictionary<int, string> DeclarationSources { get; set; }
    
    /// <summary>
    /// Key - номер телефона <br/>
    /// Value - наименование МО
    /// </summary>
    public Dictionary<string, string> MoPhoneNumbers { get; set; }
    
    public Dictionary<int, string> VerbalContactForms { get; set; }
    
    public Dictionary<int, string> WritingContactForms { get; set; }
    
    public Dictionary<int, string> CitizenCategories { get; set; }
    
    public Dictionary<int, string> IdentityDocumentTypes { get; set; }
    
    public Dictionary<int, string> Organisations { get; set; }
    
    public Dictionary<int, string> SmoOrganisations { get; set; }
    
    /// <summary>
    /// Key - Код МО <br/>
    /// Value - Наименование МО
    /// </summary>
    public Dictionary<string, string> MoOrganisations { get; set; }
    
    /// <summary>
    /// Key - Тип обращения <br/>
    /// Value - Список тем обращения
    /// </summary>
    public Dictionary<int, List<string>> DeclarationThemes { get; set; }
    
    public Dictionary<int, string> KemTypes { get; set; }
    
    public Dictionary<int, string> DeclarationResults { get; set; }
    
    public Dictionary<int, string> SvedJals { get; set; }
    
    public Dictionary<int, string> RedirectReasons { get; set; }
    
    public Dictionary<int, string> MpTypes { get; set; }
}