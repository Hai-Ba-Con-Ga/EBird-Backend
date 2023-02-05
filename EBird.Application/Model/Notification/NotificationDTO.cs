using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EBird.Application.Model.Notification
{
    public class NotificationDTO
    {
        public Guid Id { get; set; }

        public string Content { get; set; }

        public DateTime CreateDateTime { get; set; }

        //PK accountID
        public Guid AccountId { get; set; }

        //PK NotificationType
        public Guid NotificatoinTypeId { get; set; }
    }
}