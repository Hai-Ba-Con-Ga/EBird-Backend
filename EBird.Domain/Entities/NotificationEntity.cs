using EBird.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Domain.Entities
{
    [Table("Notification")]
    public class NotificationEntity : BaseEntity
    {
        [Column("Content", TypeName = "text")]
        [MaxLength(50)]
        public string? Content { get; set; }

        [Required]
        [Column("CreateDateTime", TypeName = "datetime")]
        public DateTime CreateDateTime { get; set; } = DateTime.Now;

        //PK accountID
        [ForeignKey("AccountId")]
        public Guid AccountId { get; set; }
        public AccountEntity Account = null!;

        //PK NotificationType
        [ForeignKey("NotificationTypeId")]
        public Guid NotificatoinTypeId { get; set; }
        public NotificationTypeEntity NotificationType = null!;

    }
}
