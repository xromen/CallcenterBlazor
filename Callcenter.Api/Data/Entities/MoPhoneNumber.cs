using System.ComponentModel.DataAnnotations.Schema;

namespace Callcenter.Api.Data.Entities;

[Table("tbl_number_phones")]
public class MoPhoneNumber
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("main_mcod")]
    public string MainMcod { get; set; }
    
    [Column("mcod")]
    public string Mcod { get; set; }
    
    [Column("name")]
    public string Name { get; set; }
    
    [Column("phone_number")]
    public string PhoneNumber { get; set; }
}