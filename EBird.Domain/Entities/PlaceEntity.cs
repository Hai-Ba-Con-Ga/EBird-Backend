using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using EBird.Domain.Common;

namespace EBird.Domain.Entities
{
    public class PlaceEntity : BaseEntity
    {
        [Column("Address", TypeName ="nvarchar")]
        [StringLength(150)]
        [Required]
        public string Address { get; set; }

        [Column("Name", TypeName ="nvarchar")]
        [StringLength(100)]
        [Required]
        public string Name { get; set; }

        [Column("Longitude", TypeName ="nvarchar")]
        [StringLength(100)]
        [Required]
        public string Longitude { get; set; }

        [Column("Latitude", TypeName ="nvarchar")]
        [StringLength(100)]
        [Required]
        public string Latitude { get; set; }

        [Column("CreatedDate", TypeName ="datetime")]
        [Required]
        public DateTime CreatedDate { get; set; }

        public ICollection<RequestEntity> Requests { get; set; }
    }
}