using EBird.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EBird.Domain.Entities
{
    [Table("Report")]
    public class ReportEntity : BaseEntity
    {
        [Column(TypeName = "text")]
        public string Content { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar")]
        public string Title { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(50)]
        [Required]
        public string status { get; set; }

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime CreateDatetime { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime HandleDatetime { get; set; }

        //PK
        [ForeignKey("CreatedBy")]
        public Guid CreatedById { get; set; }
        public AccountEntity CreatedBy { get; set; } = null!;

        [ForeignKey("HandledBy")]
        public Guid HandledById { get; set; }
        public AccountEntity HandledBy { get; set; } = null!;

    }
}
