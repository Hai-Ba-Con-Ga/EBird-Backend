using EBird.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EBird.Domain.Entities
{
    [Table("Post")]
    public class PostEntity : BaseEntity
    {
        [Column(TypeName = "text")]
        public string Content { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar")]
        public string Title { get; set; }

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime CreateDatetime { get; set; }

        //PK
        [ForeignKey("CreatedBy")]
        public Guid CreatedById { get; set; }
        public AccountEntity CreatedBy { get; set; } = null!;


    }
}
