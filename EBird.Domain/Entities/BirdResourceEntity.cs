using System.ComponentModel.DataAnnotations;
using EBird.Domain.Common;

namespace EBird.Domain.Entities
{
    public class BirdResourceEntity : BaseEntity
    {
        [Required]
        public Guid BirdId { get; set; }
        //1 - M relationship Bird
        public BirdEntity Bird { get; set; } = null!;
        [Required]
        public Guid ResourceId { get; set; }
        //1- M relationship Resource
        public ResourceEntity Resource { get; set; } = null!;

        public BirdResourceEntity(Guid birdId,  Guid resourceId)
        {
            BirdId = birdId;
            ResourceId = resourceId;
        }
    }
}