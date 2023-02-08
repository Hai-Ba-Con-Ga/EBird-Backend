using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using EBird.Domain.Common;

namespace EBird.Domain.Entities
{
    [Table("Bird_Resource")]
    public class BirdResource : BaseEntity
    {
        //foreign key resource
        [Column("ResourceId")]
        [Required]
        public Guid ResourceId { get; set; }
        public ResourceEntity ResourceEntity { get; set; }

        //foreign key bird
        [Column("BirdId")]
        [Required]
        public Guid BirdId { get; set; }
        public BirdEntity BirdEntity { get; set; }

        public BirdResource(Guid resourceId, Guid birdId )
        {
            ResourceId = resourceId;
            BirdId = birdId;
        }
    }
}