using EBird.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EBird.Domain.Entities
{
    [Table("Place")]
    public class PlaceEntity : BaseEntity
    {
        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string Address { get; set; }

        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime CreateDatetime { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string? Longitude { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string? Latitude { get; set; }

        // relationship
        public ICollection<RequestEntity> Requests { get; set; }
        public ICollection<MatchEntity> Matchs { get; set; }


    }
}
