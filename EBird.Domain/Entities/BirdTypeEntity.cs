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
    [Table("BirdType")]
    public class BirdTypeEntity : BaseEntity
    {
        [Column("BirdTypeCode", TypeName = "nvarchar")]
        [MaxLength(50)]
        [Required]
        //to be indexed in fluentAPI
        public string TypeCode { get; set; }

        [Column("BirdTypeName", TypeName = "nvarchar")]
        [MaxLength(100)]
        [Required]
        public string TypeName { get; set; }

        [Column("BirdTypeCreatedDatetime", TypeName = "datetime")]
        [Required]
        public DateTime CreatedDatetime { get; set; }

        //Pk
        [ForeignKey("CreatedById")]
        public Guid CreatedById { get; set; }
        public AccountEntity CreatedBy { get; set; } = null!;

        //relationship
        public ICollection<BirdEntity>? Birds { get; set; }
    }
}
