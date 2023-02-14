using EBird.Application.Exceptions;
using EBird.Application.Interfaces;
using EBird.Application.Model.Notification;
using EBird.Application.Model.NotificationType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Validation
{
    public class NotificationValidation
    {
        public static async Task ValidationNotificationType(NotificationCreateDTO notification, IWapperRepository _repository)
        {
            // check Exist notificationType ID
            if (await _repository.NotificationType.GetNotificationTypeActiveAsync(notification.NotificatoinTypeId) == null)
                throw new BadRequestException("NotificationType ID is not exist");
        }
    }
}
