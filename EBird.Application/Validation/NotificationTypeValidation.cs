using EBird.Application.Exceptions;
using EBird.Application.Interfaces;
using EBird.Application.Model.NotificationType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Validation
{
    public class notificationTypeValidation : BaseValidation
    {
        public static async Task ValidationNotificationType(NotificationTypeRequestDTO notificationType, IWapperRepository _repository)
        {
            // check notificationType null
            if (notificationType == null)
                throw new Exception("NotificationType is null");

            // check Exist notificationType
            if (await _repository.NotificationType.IsExistNotificationTypeCode(notificationType.TypeCode) == true)
                throw new BadRequestException("NotificationType Code is exist");
        }
    }
}
