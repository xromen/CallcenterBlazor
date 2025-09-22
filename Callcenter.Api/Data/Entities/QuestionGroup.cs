using System.ComponentModel.DataAnnotations.Schema;

namespace Callcenter.Api.Data.Entities;

[Table("tbl_question_groups")]
public class QuestionGroup
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("name")]
    public string Name { get; set; }
    
    [Column("org_id")]
    public int OrganisationId { get; set; }
    
    [ForeignKey(nameof(OrganisationId))]
    public Organisation Organisation { get; set; }
    
    [InverseProperty(nameof(Question.Group))]
    public List<Question> Questions { get; set; } = new();
}