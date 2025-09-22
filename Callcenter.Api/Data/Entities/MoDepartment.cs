using System.ComponentModel.DataAnnotations.Schema;

namespace Callcenter.Api.Data.Entities;

[Table("tbl_mo_departs")]
public class MoDepartment
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("main_mcod")]
    public string MainMoCode { get; set; }
    
    [Column("mcod")]
    public string MoCode { get; set; }
    
    [Column("name")]
    public string Name { get; set; }
}