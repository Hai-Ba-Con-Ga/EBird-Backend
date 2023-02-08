using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using EBird.Domain.Common;

namespace EBird.Domain.Entities
{
    [Table("Resource")]
    public class ResourceEntity : BaseEntity
    {
        [Column("ResourceDataLink", TypeName ="varchar")]
        [MaxLength(255)]
        [Required]
        public string DataLink { get; set; }

        [Column("ResourceDescription", TypeName ="nvarchar")]
        [MaxLength(100)]
        public string Description { get; set; }

        //CreateBy foreign key
        [Column("ResourceCreateById")]
        [Required]
        public Guid CreateById { get; set; }
        public AccountEntity CreateBy { get; set; }

        //collection 
        public ICollection<Bird_Resource> Bird_Resource { get; set; } = null!;
        public ICollection<Account_Resource> Account_Resource { get; set; } = null!;

    }
}