using EBird.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EBird.Domain.Entities
{
    [Table("Resource")]
    public class ResourceEntity : BaseEntity
    {
        [Column(TypeName = "varchar")]
        [MaxLength(100)]
        public string? Datalink { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(100)]
        public string? Description { get; set; }

        // Relationship
        public ICollection<MatchResourceEntity>?  MatchResources { get; set; }
        public ICollection<AccountResourceEntity>?  AccountResources { get; set; }
        public ICollection<BirdResourceEntity>?  Bird_Resource { get; set; }
    }
}
