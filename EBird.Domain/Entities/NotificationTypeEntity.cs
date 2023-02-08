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
    [Table("NotificationType")]
    public class NotificationTypeEntity : BaseEntity
    {
        [Required]
        [Column("TypeCode", TypeName = "varchar")]
        [MaxLength(100)]
        public string TypeCode { get; set; } = null!;

        [Required]
        [Column("TypeName", TypeName = "varchar")]
        [MaxLength(100)]
        public string TypeName { get; set; } = null!;


        // relationship
        public ICollection<NotificationEntity>? Notifications { get; set; }
    }
}
