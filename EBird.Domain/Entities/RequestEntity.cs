using EBird.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EBird.Domain.Entities
{
    [Table("Request")]
    public class RequestEntity : BaseEntity
    {
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime CreateDatetime { get; set; }

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime DateTime { get; set; }

        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(50)]
        public string Status { get; set; } = null!;

        // Pk
        [ForeignKey("CreatedById")]
        public Guid CreatedById { get; set; }
        public AccountEntity CreatedBy { get; set; } = null!;

        [ForeignKey("PlaceId")]
        public Guid PlaceId { get; set; }
        public PlaceEntity Place { get; set; } = null!;
        
        [ForeignKey("RoomId")]
        public Guid RoomId { get; set; }
        public RoomEntity Room { get; set; } = null!;

        [ForeignKey("BirdId")]
        public Guid BirdId { get; set; }
        public BirdEntity Bird { get; set; } = null!;

        [ForeignKey("GroupId")]
        public Guid GroupId { get; set; }
        public GroupEntity? Group { get; set; }
    }
}
