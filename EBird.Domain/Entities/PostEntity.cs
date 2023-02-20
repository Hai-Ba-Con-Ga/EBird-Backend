using EBird.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Domain.Entities
{
    [Table("Post")]
    public class PostEntity : BaseEntity
    {
        [Required]
        [Column(TypeName = "text")]
        public string Content { get; set; } = null!;

        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string Title { get; set; } = null!;

        [Required]
        public DateTime CreateDateTime { get; set; } = DateTime.Now;

        [ForeignKey("Account")]
        public Guid CreateById { get; set; }
        public AccountEntity CreateBy { get; set; } = null!;

        [ForeignKey("ThumbnailId")]
        public Guid? ThumbnailId { get; set; }
        public ResourceEntity? Thumbnail { get; set; }
    }
}
