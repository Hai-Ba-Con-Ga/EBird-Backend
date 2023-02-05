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
    public class NotificationTypeRepository : GenericRepository<NotificationTypeEntity>, INotificationTypeRepository
    {
        public NotificationTypeRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Task<NotificationTypeEntity> AddNotificationType(NotificationTypeEntity notificationType)
        {
            throw new NotImplementedException();
        }

        public Task<NotificationTypeEntity> GetNotificationTypeActiveAsync(Guid notificationTypeId)
        {
            throw new NotImplementedException();
        }

        public Task<List<NotificationTypeEntity>> GetNotificationTypesActiveAsync()
        {
            throw new NotImplementedException();
        }

        public Task<NotificationTypeEntity> SoftDeleteNotificationTypeAsync(Guid notificationTypeId)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateNotificationTypeAsync(NotificationTypeEntity notificationType)
        {
            throw new NotImplementedException();
        }
    }
}
