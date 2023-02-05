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
        //[Column("NotiTypeCode", TypeName = "varchar")]
        //[MaxLength(50)]
        //public string NotiTypeCode { get; set; }


        [Column("Content", TypeName = "text")]
        [MaxLength(50)]
        public string Content { get; set; }

        [Required]
        [Column("CreateDateTime", TypeName = "datetime")]
        public DateTime CreateDateTime { get; set; }


        //PK accountID
        [ForeignKey("AccountId")]
        public Guid AccountId { get; set; }
        public AccountEntity Account;

        //PK NotificationType
        public Guid NotificatoinTypeId { get; set; }
        public NotificationTypeEntity NotificationType;

    }
}
