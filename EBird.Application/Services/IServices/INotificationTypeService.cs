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
        public Task AddNotificationType(NotificationTypeRequestDTO ntDTO);
        public Task UpdateNotificationType(Guid id, NotificationTypeRequestDTO ntDTO);
        public Task DeleteNotificationType(Guid id);
        public Task<NotificationTypeDTO> GetNotificationType(Guid id);
        public Task<List<NotificationTypeDTO>> GetNotificationTypes();
    }
}
