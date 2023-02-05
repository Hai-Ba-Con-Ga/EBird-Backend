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
        Task<List<NotificationTypeEntity>> GetNotificationTypesActiveAsync();
        Task<NotificationTypeEntity> GetNotificationTypeActiveAsync(Guid notificationTypeId);
        Task<NotificationTypeEntity> AddNotificationType(NotificationTypeEntity notificationType);
        Task<int> UpdateNotificationTypeAsync(NotificationTypeEntity notificationType);
        Task<NotificationTypeEntity> SoftDeleteNotificationTypeAsync(Guid notificationTypeId);

    }
}