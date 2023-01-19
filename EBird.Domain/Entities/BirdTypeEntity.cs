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
    public class BirdTypeEntity : BaseEntity
    {
        [Column("BirdTypeCode", TypeName = "nvarchar")]
        [MaxLength(50)]
        [Required]
        //index in fluentAPI
        public string TypeCode { get; set; }

        [Column("BirdTypeName", TypeName = "nvarchar")]
        [MaxLength(100)]
        [Required]
        public string TypeName { get; set; }

        [Column("BirdTypeCreatedDatetime", TypeName = "datetime")]
        [Required]
        public DateTime CreatedDatetime { get; set; }

        //relationship
        public ICollection<BirdEntity> Birds { get; set; }

        public BirdTypeEntity(string typeCode, string name, DateTime createdDatetime) : base(Guid.NewGuid())
        {
            TypeCode = typeCode;
            TypeName = name;
            CreatedDatetime = createdDatetime;
        }

        
    }
}
