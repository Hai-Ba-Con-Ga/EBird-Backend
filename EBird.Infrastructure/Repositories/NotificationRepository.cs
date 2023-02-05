using EBird.Application.Interfaces.IRepository;
using EBird.Domain.Entities;
using EBird.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Infrastructure.Repositories
{
    public class NotificationRepository : GenericRepository<NotificationEntity>, INotificationRepository
    {
        public NotificationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<NotificationEntity> AddNotification(NotificationEntity notification)
        {
            notification.CreateDateTime = DateTime.Now;
            var result = await this.CreateAsync(notification);
            if (result == 0) return null;
            return notification;
        }

        public Task<List<NotificationEntity>> GetAllNotificationByAccountIdSync(Guid accountId)
        {
            throw new NotImplementedException();
        }

        public Task<NotificationEntity> GetNotificationActiveAsync(Guid notificationId)
        {
            throw new NotImplementedException();
        }

        public Task<List<NotificationEntity>> GetNotificationsActiveAsync()
        {
            throw new NotImplementedException();
        }

        public Task<NotificationEntity> SoftDeleteNotificationAsync(Guid notificationId)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateNotificationAsync(NotificationEntity notification)
        {
            throw new NotImplementedException();
        }
    }
}
