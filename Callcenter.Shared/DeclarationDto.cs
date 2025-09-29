namespace Callcenter.Shared;

public class DeclarationDto
{
    public int? Id { get; set; }
    
    public int? StatusId { get; set; }
    
    public string? StatusName { get; set; }
    
    public int? CodeId { get; set; }
    
    public string? CodeName { get; set; }
    
    public int? TypeId { get; set; }
    
    public string? TypeName { get; set; }
    
    public int? ContactFormId { get; set; }
    
    public string? ContactFormName { get; set; }
    
    public int? CitizenCategoryId { get; set; }
    
    public string? CitizenCategoryName { get; set; }
    
    public DateTime? DateRegistered { get; set; }
    
    public DateTime? DateRegisteredSmo { get; set; }
    
    public string? FirstName { get; set; }

    public string? SecName { get; set; }
    
    public string? FathName { get; set; }
    
    public DateOnly? BirthDate { get; set; }
    
    public string? ResidenceAddress { get; set; }
    
    public string? Phone { get; set; }
    
    public string? Email { get; set; }
    
    public int? InsuredSmoId { get; set; }
    
    public string? InsuredSmoName { get; set; }
    
    public int? InsuredMoId { get; set; }
    
    public string? Theme { get; set; }
    
    public string? WorkDone { get; set; }
    
    public DateOnly? AnswerDate { get; set; }
    
    public int? ResultId { get; set; }
    
    public string? ResultName { get; set; }
    
    public DateOnly? ClosedDate { get; set; }
    
    public string? Number { get; set; }
    
    public int? AnswerStatusId { get; set; }
    
    public string? AnswerStatusName { get; set; }
    
    public int? SourceId { get; set; }
    
    public string? Content { get; set; }
    
    public string? InsuredEnp { get; set; }
    
    public string? IdentityDocType { get; set; }
    
    public string? IdentityDocSeries { get; set; }
    
    public string? IdentityDocNumber { get; set; }
    
    public int? SvedJalId { get; set; }
    
    public string? SvedJalName { get; set; }
    
    public DateOnly? SupervisorDate { get; set; }
    
    public UserDto? Creator { get; set; }
    
    public int? AnswerOrgId { get; set; }
    
    public string? AnswerOrgName { get; set; }
    
    public int? AnswerUserId { get; set; }
    
    public UserDto? AnswerUser { get; set; }
    
    public int? HaveOrgId { get; set; }
    
    public string? HaveOrgName { get; set; }
    
    public DateOnly? SupervisorSmoDate { get; set; }
    
    public int? MpTypeId { get; set; }
    
    public string? MpTypeName { get; set; }
    
    public int? RedirectReasonId { get; set; }
    
    public string? RedirectReasonName { get; set; }
    
    public string? MoPhoneNumber { get; set; }
    
    public MoPhoneNumberDto? MoPhone { get; set; }
    
    public int? KemTypeId { get; set; }
    
    public string? KemTypeName { get; set; }
    
    public string? EjogNumber { get; set; }
    
    public int? SvoStatusId { get; set; }
    
    public string? SvoStatusName { get; set; }
    
    public string? AgentSecName { get; set; }

    public List<FileDto>? Files { get; set; } = new();
}