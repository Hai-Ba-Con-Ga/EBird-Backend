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

		public async Task<NotificationEntity> AddNotificationAsync(NotificationEntity Notification)
        {
            Notification.CreateDateTime = DateTime.Now;
            var res = await this.CreateAsync(Notification);
            if (res > 0)
            {
                return Notification;
            }
            return null;
        }

		public async Task<List<NotificationEntity>> GetAllNotificationActiveByAccountIdAsync(Guid accountId)
		{
			return await this.FindAllWithCondition(x => x.AccountId == accountId && x.IsDeleted == false);
		}

		public async Task<NotificationEntity> GetNotificationActiveAsync(Guid NotificationId)
        {
            return await this.GetByIdActiveAsync(NotificationId);
        }

        public async Task<List<NotificationEntity>> GetNotificationsActiveAsync()
        {
            return await this.GetAllActiveAsync();
        }

        public async Task<NotificationEntity> SoftDeleteNotificationAsync(Guid NotificationId)
        {
            return await this.DeleteSoftAsync(NotificationId);
        }

        public async Task<int> UpdateNotificationAsync(NotificationEntity Notification)
        {
            return await this.UpdateAsync(Notification);
        }
    }
}
