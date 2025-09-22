using System.ComponentModel.DataAnnotations.Schema;

namespace Callcenter.Api.Data.Entities;

[Table("dc_form_obr")]
public class DeclarationContactForm
{
    [Column("id")]
    public int Id { get; set; }
    
    /// <summary>
    /// 1 - Устная форма обращения <br/>
    /// 2 - Письменная форма обращения
    /// </summary>
    [Column("type")]
    public int Type { get; set; }
    
    [Column("name")]
    public string Name { get; set; }
}