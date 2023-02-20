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
    [Table("VerifcationStore")]
    public class VerifcationStoreEntity : BaseEntity
    {
        [Column(TypeName = "varchar")]
        [MaxLength(50)]
        public string Code { get; set; }
        public Guid AccountId { get; set; }
    }
}
