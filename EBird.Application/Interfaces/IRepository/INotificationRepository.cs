using EBird.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Interfaces.IRepository
{
    public interface INotificationRepository : IGenericRepository<NotificationEntity>
    {
        Task<List<NotificationEntity>> GetNotificationsActiveAsync();
        Task<NotificationEntity> GetNotificationActiveAsync(Guid notificationId);
        Task<NotificationEntity> AddNotification(NotificationEntity notification);
        Task<int> UpdateNotificationAsync(NotificationEntity notification);
        Task<NotificationEntity> SoftDeleteNotificationAsync(Guid notificationId);


        Task<List<NotificationEntity>> GetAllNotificationByAccountIdSync(Guid accountId);
    }
}
