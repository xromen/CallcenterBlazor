using Callcenter.Shared;

namespace Callcenter.Web.Models;

public class DeclarationModel
{
    public int Id { get; set; }
    
    public int? StatusId { get; set; }
    
    public int? CodeId { get; set; }
    
    public int? TypeId { get; set; }
    
    public int? ContactFormId { get; set; }
    
    public int? CitizenCategoryId { get; set; }
    
    public DateTime? DateRegistered { get; set; }
    
    public DateTime? DateRegisteredSmo { get; set; }
    
    public string? FirstName { get; set; }

    public string? SecName { get; set; }
    
    public string? FathName { get; set; }
    
    public DateTime? BirthDate { get; set; }
    
    public string? ResidenceAddress { get; set; }
    
    public string? Phone { get; set; }
    
    public string? Email { get; set; }
    
    public int? InsuredSmoId { get; set; }
    
    public int? InsuredMoId { get; set; }

    // public MoOrganisationModel? InsuredMo
    // {
    //     get => _insuredMo;
    //     set
    //     {
    //         _insuredMo = value;
    //         InsuredMoId = value == null || string.IsNullOrWhiteSpace(value.MoCode) ? null : Convert.ToInt32(value.MoCode);
    //     }
    // }
    // private MoOrganisationModel? _insuredMo;

    public string? Theme { get; set; }
    
    public string? WorkDone { get; set; }
    
    public DateTime? AnswerDate { get; set; }
    
    public int? ResultId { get; set; }
    
    public DateTime? ClosedDate { get; set; }
    
    public string Number { get; set; }
    
    public int? AnswerStatusId { get; set; }
    
    public int? SourceId { get; set; }
    
    public string? Content { get; set; }
    
    public string? InsuredEnp { get; set; }
    
    public string? IdentityDocType { get; set; }
    
    public string? IdentityDocSeries { get; set; }
    
    public string? IdentityDocNumber { get; set; }
    
    public int? SvedJalId { get; set; }
    
    public DateTime? SupervisorDate { get; set; }
    
    public UserDto? Creator { get; set; }
    
    public int? AnswerOrgId { get; set; }
    
    public UserDto? AnswerUser { get; set; }
    
    public int? HaveOrgId { get; set; }
    
    public DateTime? SupervisorSmoDate { get; set; }
    
    public int? MpTypeId { get; set; }
    
    public int? RedirectReasonId { get; set; }
    
    public string? MoPhoneNumber { get; set; }

    public MoPhoneModel? MoPhone
    {
        get => _moPhone;
        set
        {
            _moPhone = value;
            MoPhoneNumber = value?.PhoneNumber;
        }
    }
    private MoPhoneModel? _moPhone;

    public int? KemTypeId { get; set; }
    
    public string? EjogNumber { get; set; }
    
    public int? SvoStatusId { get; set; }
    
    public string? AgentSecName { get; set; }
    
    public List<FileDto> Files { get; set; }
    
    public List<SupervisorCommentDto> Comments { get; set; }
}