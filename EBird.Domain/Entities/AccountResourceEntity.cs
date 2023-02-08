using EBird.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EBird.Domain.Entities
{
    public class AccountResourceEntity : BaseEntity
    {
        [Required]
        public Guid AccountId { get; set; }
        //1-M relationship AccountEnity
        public AccountEntity Account { get; set; } = null!;
        [Required]
        public Guid ResourceId { get; set; }
        //1-M relationship ResourceEntity
        public ResourceEntity Resource { get; set; } = null!;

    }
}