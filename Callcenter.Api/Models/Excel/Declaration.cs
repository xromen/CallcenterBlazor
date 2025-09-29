namespace Callcenter.Api.Models.Excel;

public class Declaration
{
    [ExcelColumn(ColumnName = "Id")]
    public int? Id { get; set; }
    
    [ExcelColumn(ColumnName = "Статус")]
    public string? StatusName { get; set; }
    
    [ExcelColumn(ColumnName = "Код обращения")]
    public string? CodeName { get; set; }
    
    [ExcelColumn(ColumnName = "Вид обращения")]
    public string? TypeName { get; set; }
    
    [ExcelColumn(ColumnName = "Форма обращения")]
    public string? ContactFormName { get; set; }
    
    [ExcelColumn(ColumnName = "Категория граждан")]
    public string? CitizenCategoryName { get; set; }//
    
    [ExcelColumn(ColumnName = "Дата поступления обращения в ФОМС", Format = "dd.MM.yyyy")]
    public DateTime? DateRegistered { get; set; }
    
    [ExcelColumn(ColumnName = "Дата поступления обращения в СМО", Format = "dd.MM.yyyy")]
    public DateTime? DateRegisteredSmo { get; set; }
    
    [ExcelColumn(ColumnName = "Имя")]
    public string? FirstName { get; set; }

    [ExcelColumn(ColumnName = "Фамилия")]
    public string? SecName { get; set; }
    
    [ExcelColumn(ColumnName = "Отчество")]
    public string? FathName { get; set; }
    
    [ExcelColumn(ColumnName = "Дата рождения", Format = "dd.MM.yyyy")]
    public DateOnly? BirthDate { get; set; }
    
    [ExcelColumn(ColumnName = "Адрес проживания")]
    public string? ResidenceAddress { get; set; }
    
    [ExcelColumn(ColumnName = "Номер телефона")]
    public string? Phone { get; set; }
    
    [ExcelColumn(ColumnName = "Email")]
    public string? Email { get; set; }
    
    [ExcelColumn(ColumnName = "СМО застрахованного")]
    public string? InsuredSmoName { get; set; }//
    
    [ExcelColumn(ColumnName = "МО застрахованного")]
    public int? InsuredMoId { get; set; }
    
    [ExcelColumn(ColumnName = "Тема обращения")]
    public string? Theme { get; set; }
    
    [ExcelColumn(ColumnName = "Проделанная работа")]
    public string? WorkDone { get; set; }
    
    [ExcelColumn(ColumnName = "Дата окончательного ответа", Format = "dd.MM.yyyy")]
    public DateOnly? AnswerDate { get; set; }
    
    [ExcelColumn(ColumnName = "Результат обращения")]
    public string? ResultName { get; set; }//
    
    [ExcelColumn(ColumnName = "Дата закрытия администратором", Format = "dd.MM.yyyy")]
    public DateOnly? ClosedDate { get; set; }
    
    [ExcelColumn(ColumnName = "Состояние исполнения")]
    public string? AnswerStatusName { get; set; }
    
    [ExcelColumn(ColumnName = "Источник поступления")]
    public int? SourceId { get; set; }
    
    [ExcelColumn(ColumnName = "Содержание обращения")]
    public string? Content { get; set; }
    
    [ExcelColumn(ColumnName = "ЕНП")]
    public string? InsuredEnp { get; set; }
    
    [ExcelColumn(ColumnName = "Тип документа")]
    public string? IdentityDocType { get; set; }
    
    [ExcelColumn(ColumnName = "Серия документа")]
    public string? IdentityDocSeries { get; set; }
    
    [ExcelColumn(ColumnName = "Номер документа")]
    public string? IdentityDocNumber { get; set; }
    
    [ExcelColumn(ColumnName = "Сведения о жалобе")]
    public string? SvedJalName { get; set; }//
    
    [ExcelColumn(ColumnName = "Дата закрытия руководителем", Format = "dd.MM.yyyy")]
    public DateOnly? SupervisorDate { get; set; }
    
    [ExcelColumn(ColumnName = "Создатель карточки")]
    public string? CreatorUserName { get; set; }//
    
    [ExcelColumn(ColumnName = "Организация ответа")]
    public string? AnswerOrgName { get; set; }
    
    [ExcelColumn(ColumnName = "Пользователь ответа")]
    public string? AnswerUserName { get; set; }//
    
    [ExcelColumn(ColumnName = "Переслано в")]
    public string? HaveOrgName { get; set; }//
    
    [ExcelColumn(ColumnName = "Дата закрытия руководителем СМО", Format = "dd.MM.yyyy")]
    public DateOnly? SupervisorSmoDate { get; set; }
    
    [ExcelColumn(ColumnName = "Вид оказанной МП")]
    public string? MpTypeName { get; set; }//
    
    [ExcelColumn(ColumnName = "Направлено")]
    public string? RedirectReasonName { get; set; }//
    
    [ExcelColumn(ColumnName = "Контактный номер поступления")]
    public string? MoPhoneNumber { get; set; }
    
    [ExcelColumn(ColumnName = "Вид проведенных КЭМ")]
    public string? KemTypeName { get; set; }
    
    [ExcelColumn(ColumnName = "Номер ЭЖОГ")]
    public string? EjogNumber { get; set; }
    
    [ExcelColumn(ColumnName = "СВО статус")]
    public string? SvoStatusName { get; set; }
    
    [ExcelColumn(ColumnName = "Ф.И.О. представителя ЗЛ")]
    public string? AgentSecName { get; set; }
}