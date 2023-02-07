using EBird.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EBird.Domain.Entities
{
    [Table("Match")]
    public class MatchEntity : BaseEntity
    {

        [MaxLength(20)]
        [Column(TypeName = "varchar")]
        public string? Result { get; set; }

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime CreateDatetime { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? DateTime { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(20)]
        public string? Status { get; set; }

        //PK
        [ForeignKey("CreatedBy")]
        public Guid CreatedById { get; set; }
        public AccountEntity CreatedBy { get; set; } = null!;

        [ForeignKey("PlaceId")]
        public Guid PlaceId { get; set; }
        public PlaceEntity Place { get; set; } = null!;

        // Relationship
        public ICollection<MatchResourceEntity>? MatchResources { get; set; }
        public ICollection<MatchBirdEntity>? MatchBirds { get; set; }
        public ICollection<MatchMessageEntity>? MatchMessages { get; set; }


    }
}
