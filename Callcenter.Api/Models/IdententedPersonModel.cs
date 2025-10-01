using System.ComponentModel.DataAnnotations;

namespace Callcenter.Api.Models
{
    public class IdententedPersonModel
    {
        [Key]
        public long PersonId { get; set; }
        public int? InsuredSmoId { get; set; }

        public string? ResidenceAddress { get; set; }

        public string? InsuredEnp { get; set; }

        public string? IdentityDocType { get; set; }

        public string? IdentityDocSeries { get; set; }

        public string? IdentityDocNumber { get; set; }
    }
}
