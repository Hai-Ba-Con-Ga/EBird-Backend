using EBird.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EBird.Domain.Entities
{
    [Table("Match_Bird")]
    public class MatchBirdEntity : BaseEntity
    {
        [Column(TypeName = "bit")]
        public bool? Result { get; set; }

        [Column(TypeName = "int")]
        public int? AfterElo { get; set; }

        [Column(TypeName = "int")]
        public int? BeforeElo { get; set; }

        //PK
        [ForeignKey("MatchId")]
        public Guid MatchId { get; set; }
        public MatchEntity Match { get; set; } = null!;

        //PK
        [ForeignKey("BirdId")]
        public Guid BirdId { get; set; }
        public BirdEntity Bird { get; set; } = null!;
    }
}
