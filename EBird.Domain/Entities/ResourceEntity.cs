using EBird.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EBird.Domain.Entities
{
    [Table("Resource")]
    public class ResourceEntity : BaseEntity
    {
        public Guid CreateById { get; set; }
        public AccountEntity Account { get; set; } = null!;

        [Column(TypeName = "text")]
        public string? Datalink { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(255)]
        public string? Description { get; set; }

        [Column("CreateDate", TypeName ="datetime")]
        [Required]
        public DateTime CreateDate { get; set; }

        public ICollection<AccountResourceEntity> AccountResources { get; set; }

        public ICollection<BirdResourceEntity> BirdResources { get; set; }
    }
}
