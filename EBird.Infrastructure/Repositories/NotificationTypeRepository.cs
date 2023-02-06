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

        public async Task<NotificationTypeEntity> SoftDeleteNotificationTypeAsync(string notificationTypeCode)
        {
            NotificationTypeEntity _entity = await GetNotificationTypeByCodeAsync(notificationTypeCode);

            if (_entity == null)
            {
                return null;
            }
            _entity.IsDeleted = true;

            await this.UpdateAsync(_entity);

            return _entity;
        }

        public async Task<NotificationTypeEntity> GetNotificationTypeByCodeAsync(string notificationTypeCode)
        {
            return await this.FindWithCondition(x => x.TypeCode == notificationTypeCode);
        }

        public async Task<bool> IsExistNotificationTypeCode(string notificationTypeCode)
        {
            var result = await this.FindWithCondition(x => x.TypeCode == notificationTypeCode);
            if (result == null)
            {
                return false;
            }
            return true;
        }

        public async Task<List<NotificationTypeEntity>> GetAllNotificationTypesActiveAsync()
        {
            return await this.GetAllActiveAsync();
        }

        public async Task<NotificationTypeEntity> GetNotificationTypeActiveAsync(Guid id)
        {
            return await this.GetByIdAsync(id);
        }

        public async Task<int> UpdateNotificationTypeAsync(NotificationTypeEntity notificationType)
        {
            return await this.UpdateAsync(notificationType);
        }

        public async Task<NotificationTypeEntity> AddNotificationTypeAsync(NotificationTypeEntity notificationType)
        {
            var addEntity = await this.CreateAsync(notificationType);
            if (addEntity == 0)
            {
                return null;
            }
            return notificationType;
        }
    }
}
