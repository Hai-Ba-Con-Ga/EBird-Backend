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
    [Table("Bird")]
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

        [Column("BirdStatus", TypeName = "nvarchar")]
        [MaxLength(50)]
        public string Status { get; set; }

        [Column("BirdCreatedDatetime", TypeName = "datetime")]
        [Required]
        public DateTime CreatedDatetime { get; set; }

        [Column("BirdDescription", TypeName = "text")]
        public string Description { get; set; }

        [Column("BirdColor", TypeName = "nvarchar")]
        [MaxLength(50)]
        [Required]
        public string Color { get; set; }

        //forgein key with account table
        [Column("BirdTypeId")]
        public Guid BirdTypeId { get; set; }
        public BirdTypeEntity BirdType { get; set; }

        //forgein key with account table
        [Column("OwnerId")]
        public Guid OwnerId { get; set; }
        public AccountEntity Owner { get; set; }

        //collection
        public ICollection<BirdResource> BirdResources { get; set; }
    }
}
