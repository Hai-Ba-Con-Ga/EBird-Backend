using EBird.Domain.Common;

namespace EBird.Domain.Entities
{
    public class BirdResourceEntity : BaseEntity
    {
        public Guid BirdId { get; set; }
        //1 - M relationship Bird
        public BirdEntity Bird { get; set; } = null!;
        public Guid ResourceId { get; set; }
        public ResourceEntity Resources { get; set; } = null!;
        //1- M relationship Resource
    }
}