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
    public class Bird_Resource : BaseEntity
    {
        //foreign resource
        [Column("ResourceId")]
        [Required]
        public Guid ResourceId { get; set; }
        public ResourceEntity ResourceEntity { get; set; }

        //foreign resource bird
        [Column("BirdId")]
        [Required]
        public Guid BirdId { get; set; }
        public BirdEntity BirdEntity { get; set; }
    }
}