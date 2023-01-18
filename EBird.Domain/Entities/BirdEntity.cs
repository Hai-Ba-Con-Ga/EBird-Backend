using EBird.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Domain.Entities
{
    public class BirdEntity : BaseEntity
    {
        [Column("BirdName", TypeName = "nvarchar")]
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [Column("BirdAge", TypeName = "int")]
        [Required]
        public int Age { get; set; }

        [Column("BirdWeight", TypeName = "float")]
        [Required]
        public float Weight { get; set; }

        [Column("BirdElo", TypeName = "int")]
        [Required]
        public int Elo { get; set; }

        [Column("BirdStatus", TypeName = "bit")]
        [Required]
        public bool Status { get; set; }

        [Column("BirdCreatedDatetime", TypeName = "datetime")]
        [Required]
        public DateTime CreatedDatetime { get; set; }

        [Column("BirdDescription", TypeName = "text")]
        public string Description { get; set; }

        [Column("BirdColor", TypeName = "nvarchar")]
        [MaxLength(50)]
        [Required]
        public string Color { get; set; }


        [Column("BirdTypeId")]
        public Guid BirdTypeId { get; set; }
        public BirdTypeEntity BirdType { get; set; }


        //[ForeignKey("OwnerId")]
        //public AccountEnity Owner { get; set; }



    }
}
