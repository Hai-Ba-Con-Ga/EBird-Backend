using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Application.Model.Notification;

namespace EBird.Application.Services.IServices
{
    public interface INotificationService
    {
        public Task<NotificationDTO> GetNotification(Guid NotificationId);
        public Task<List<NotificationDTO>> GetNotifications();
        public Task<Guid> AddNotification(NotificationCreateDTO NotificationDTO);
        public Task<NotificationUpdateDTO> UpdateNotification(Guid id, NotificationUpdateDTO NotificationDTO);
        public Task<NotificationDTO> DeleteNotification(Guid NotificationId);

        public Task<List<NotificationDTO>> GetAllNotificationsByAccountId(Guid accountId);
        public Task<Guid> AddNotificationWithZapier(NotificationCreateDTO notificationCreateDTO);
    }
}