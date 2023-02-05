using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EBird.Application.Model.Notification
{
    public class NotificationCreateDTO
    {
        public string Content { get; set; }

        //PK accountID
        public Guid AccountId { get; set; }

        //PK NotificationType
        public Guid NotificatoinTypeId { get; set; }
    }
}