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
        [Column(TypeName = "varchar")]
        public string? Datalink { get; set; }
        [Column(TypeName = "varchar")]
        public string? Description { get; set; }
    }
}