using EBird.Application.Exceptions;
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

        public async Task<NotificationTypeEntity> SoftDeleteNotificationTypeAsync(Guid Id)
        {
            return await DeleteSoftAsync(Id);
        }

        public async Task<NotificationTypeEntity> GetNotificationTypeByCodeAsync(string notificationTypeCode)
        {
            return await this.FindWithCondition(x => x.TypeCode == notificationTypeCode);
        }

        public async Task<bool> IsExistNotificationTypeCode(string notificationTypeCode)
        {
            var result = await this.FindWithCondition(x => x.TypeCode == notificationTypeCode);
            return result != null;
        }

        public async Task<List<NotificationTypeEntity>> GetAllNotificationTypesActiveAsync()
        {
            return await this.GetAllActiveAsync();
        }

        public async Task<NotificationTypeEntity> GetNotificationTypeActiveAsync(Guid id)
        {
            return await this.GetByIdActiveAsync(id);
        }

        public async Task<int> UpdateNotificationTypeAsync(NotificationTypeEntity notificationType)
        {
            return await this.UpdateAsync(notificationType);
        }

        public async Task<Guid> AddNotificationTypeAsync(NotificationTypeEntity notificationType)
        {
            var addEntity = await this.CreateAsync(notificationType);
            if (addEntity == 0)
            {
                throw new BadRequestException("can not create notifiction type");
            }
            return notificationType.Id;
        }
    }
}
