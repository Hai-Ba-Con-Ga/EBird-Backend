using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using EBird.Domain.Common;

namespace EBird.Domain.Entities
{
    [Table("Account_Resource")]
    public class Account_Resource : BaseEntity
    {
        //foreign key account
        [Column("AccountId")]
        [Required]
        public Guid AccountId { get; set; }
        public AccountEntity AccountEntity { get; set; }

        //foreign key resource
        [Column("ResourceId")]
        [Required]
        public Guid ResourceId { get; set; }
        public ResourceEntity ResourceEntity { get; set; }
    }
}