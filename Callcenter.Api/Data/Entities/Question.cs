using System.ComponentModel.DataAnnotations.Schema;

namespace Callcenter.Api.Data.Entities;

[Table("tbl_questions")]
public class Question
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("name")]
    public string Name { get; set; }
    
    [Column("question")]
    public string Answer { get; set; }
    
    [Column("q_group_id")]
    public int GroupId { get; set; }
    
    [ForeignKey(nameof(GroupId))]
    public QuestionGroup Group { get; set; }
}