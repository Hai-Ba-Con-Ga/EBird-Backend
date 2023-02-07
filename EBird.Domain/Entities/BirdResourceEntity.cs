using EBird.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EBird.Domain.Entities
{
    [Table("Bird_Resource")]
    public class BirdResourceEntity : BaseEntity
    {
        //PK
        [ForeignKey("BirdId")]
        public Guid BirdId { get; set; }
        public BirdEntity Bird { get; set; } = null!;

        //PK
        [ForeignKey("ResourceId")]
        public Guid ResourceId { get; set; }
        public ResourceEntity Resource { get; set; } = null!;
    }
}
