using EBird.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Domain.Entities
{
    public class VerifcationStoreEntity : BaseEntity
    {
        public string Code { get; set; }
        public Guid AccountId { get; set; }
    }
}
