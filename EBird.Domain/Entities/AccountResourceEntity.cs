using EBird.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EBird.Domain.Entities
{
    [Table("Account_Resource")]
    public class AccountResourceEntity : BaseEntity
    {
        //PK
        [ForeignKey("AccountId")]
        public Guid AccountId { get; set; }
        public AccountEntity Account { get; set; } = null!;

        //PK
        [ForeignKey("ResourceId")]
        public Guid ResourceId { get; set; }
        public ResourceEntity Resource { get; set; } = null!;
    }
}
