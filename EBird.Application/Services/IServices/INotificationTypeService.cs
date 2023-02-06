using EBird.Application.Model.NotificationType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Services.IServices
{
    public interface INotificationTypeService
    {
        public Task<NotificationTypeDTO> GetNotificationType(Guid id);
        public Task<List<NotificationTypeDTO>> GetNotificationTypes();
        public Task<NotificationTypeRequestDTO> AddNotificationType(NotificationTypeRequestDTO notificationTypeDTO);
        public Task<NotificationTypeRequestDTO> UpdateNotificationType(Guid id, NotificationTypeRequestDTO notificationTypeDTO);
        public Task<NotificationTypeDTO> DeleteNotificationType(Guid id);
    }
}
