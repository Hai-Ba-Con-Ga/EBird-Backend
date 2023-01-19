using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Domain.Common
{
    public abstract class BaseEntity
    {
        [Key]
        public Guid Id { get; init; }
        
        
        public bool IsDeleted { get; set; } = false;

        protected BaseEntity(Guid id)
        {
            Id = id;
        }
    }
}
