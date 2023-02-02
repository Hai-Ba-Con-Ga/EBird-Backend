using EBird.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Domain.Entities
{
    [Table("VerifcationStore")]
    public class VerifcationStoreEntity : BaseEntity
    {
        public string Code { get; set; }
        public Guid AccountId { get; set; }
    }
}
