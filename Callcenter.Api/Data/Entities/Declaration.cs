using System.ComponentModel.DataAnnotations.Schema;

namespace Callcenter.Api.Data.Entities;

[Table("tbl_rk")]
public class Declaration
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("status_id")]
    public int? StatusId { get; set; }
    
    [ForeignKey(nameof(StatusId))]
    public DeclarationStatus? Status { get; set; }
    
    [Column("code_id")]
    public int? CodeId { get; set; }
    
    [Column("type_id")]
    public int? TypeId { get; set; }
    
    [ForeignKey(nameof(TypeId))]
    public DeclarationType? Type { get; set; }
    
    [Column("form_id")]
    public int? FormId { get; set; }
    
    [ForeignKey(nameof(FormId))]
    public DeclarationContactForm? ContactForm { get; set; }
    
    [Column("citizen_id")]
    public int? CitizenCategoryId { get; set; }
    
    [ForeignKey(nameof(CitizenCategoryId))]
    public CitizenCategory? CitizenCategory { get; set; }
    
    [Column("date_reg")]
    public DateTime? DateRegistered { get; set; }
    
    [Column("date_reg_smo")]
    public DateTime? DateRegisteredSmo { get; set; }
    
    [Column("first_name")]
    public string? FirstName { get; set; }

    [Column("sec_name")]
    public string? SecName { get; set; }
    
    [Column("fath_name")]
    public string? FathName { get; set; }
    
    [Column("age")]
    public DateOnly? BirthDate { get; set; }
    
    [Column("address_fact")]
    public string? ResidenceAddress { get; set; }
    
    [Column("contact_phone")]
    public string? Phone { get; set; }
    
    [Column("contact_email")]
    public string? Email { get; set; }
    
    [Column("smo_reg_id")]
    public int? InsuredSmoId { get; set; }
    
    [ForeignKey(nameof(InsuredSmoId))]
    public Organisation? InsuredSmo { get; set; }
    
    [Column("mo_reg_id")]
    public int? InsuredMoId { get; set; }
    
    [Column("obr_theme")]
    public string? Theme { get; set; }
    
    [Column("work_done")]
    public string? WorkDone { get; set; }
    
    [Column("date_answer")]
    public DateOnly? AnswerDate { get; set; }
    
    [Column("result_id")]
    public int? ResultId { get; set; }
    
    [ForeignKey(nameof(ResultId))]
    public DeclarationResult? Result { get; set; }
    
    [Column("date_closed")]
    public DateOnly? ClosedDate { get; set; }
    
    [Column("rk_number")]
    public string Number { get; set; }
    
    [Column("an_status")]
    public int? AnswerStatusId { get; set; }
    
    [ForeignKey(nameof(AnswerStatusId))]
    public AnswerStatus? AnswerStatus { get; set; }
    
    [Column("source_id")]
    public int? SourceId { get; set; }
    
    [ForeignKey(nameof(SourceId))]
    public DeclarationSource? Source { get; set; }
    
    [Column("content")]
    public string? Content { get; set; }
    
    [Column("enp")]
    public string? InsuredEnp { get; set; }
    
    [Column("doc_type")]
    public string? IdentityDocType { get; set; }
    
    [Column("doc_ser")]
    public string? IdentityDocSeries { get; set; }
    
    [Column("doc_nom")]
    public string? IdentityDocNumber { get; set; }
    
    [Column("sved_jal_id")]
    public int? SvedJalId { get; set; }
    
    [ForeignKey(nameof(SvedJalId))]
    public SvedJal? SvedJal { get; set; }
    
    [Column("date_rukovod")]
    public DateOnly? SupervisorDate { get; set; }
    
    [Column("creator_id")]
    public int? CreatorId { get; set; }
    
    [ForeignKey(nameof(CreatorId))]
    public User? Creator { get; set; }
    
    [Column("answer_org_id")]
    public int? AnswerOrgId { get; set; }
    
    [ForeignKey(nameof(AnswerOrgId))]
    public Organisation? AnswerOrg { get; set; }
    
    [Column("answer_user_id")]
    public int? AnswerUserId { get; set; }
    
    [ForeignKey(nameof(AnswerUserId))]
    public User? AnswerUser { get; set; }
    
    [Column("have_org_id")]
    public int? HaveOrgId { get; set; }
    
    [ForeignKey(nameof(HaveOrgId))]
    public Organisation? HaveOrg { get; set; }
    
    [Column("date_rukovod_smo")]
    public DateOnly? SupervisorSmoDate { get; set; }
    
    [Column("vid_mp")]
    public int? MpTypeId { get; set; }
    
    [ForeignKey(nameof(MpTypeId))]
    public MpType? MpType { get; set; }
    
    [Column("redirect_reason_id")]
    public int? RedirectReasonId { get; set; }
    
    [ForeignKey(nameof(RedirectReasonId))]
    public RedirectReason? RedirectReason { get; set; }
    
    [Column("mo_phone")]
    public string? MoPhoneNumber { get; set; }
    
    public MoPhoneNumber? MoPhone { get; set; }
    
    [Column("vid_kem_id")]
    public int? KemTypeId { get; set; }
    
    [ForeignKey(nameof(KemTypeId))]
    public KemType? KemType { get; set; }
    
    [Column("ezhog_number")]
    public string? EjogNumber { get; set; }
    
    [Column("svo_status_id")]
    public int? SvoStatusId { get; set; }
    
    [ForeignKey(nameof(SvoStatusId))]
    public SvoStatus? SvoStatus { get; set; }
    
    [Column("agent_sec_name")]
    public string? AgentSecName { get; set; }
    
    [InverseProperty(nameof(DeclarationAction.Declaration))]
    public List<DeclarationAction> History { get; set; }
    
    [InverseProperty(nameof(DeclarationFile.Declaration))]
    public List<DeclarationFile> Files { get; set; }
}