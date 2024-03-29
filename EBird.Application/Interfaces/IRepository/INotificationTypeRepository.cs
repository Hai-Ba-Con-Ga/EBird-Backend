﻿using EBird.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Interfaces.IRepository
{
    public interface INotificationTypeRepository : IGenericRepository<NotificationTypeEntity>
    {
        Task<List<NotificationTypeEntity>> GetAllNotificationTypesActiveAsync();
        Task<NotificationTypeEntity> GetNotificationTypeActiveAsync(Guid Id);
        Task<Guid> AddNotificationTypeAsync(NotificationTypeEntity notificationType);
        Task<int> UpdateNotificationTypeAsync(NotificationTypeEntity notificationType);
        Task<NotificationTypeEntity> SoftDeleteNotificationTypeAsync(Guid Id);
        public Task<bool> IsExistNotificationTypeCode(string notificationTypeCode);


    }
}
