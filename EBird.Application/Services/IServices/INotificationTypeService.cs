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
        public Task<NotificationTypeDTO> GetNotificationType(string NotificationTypeCode);
        public Task<List<NotificationTypeDTO>> GetNotificationTypes();
        public Task<NotificationTypeRequestDTO> AddNotificationType(NotificationTypeRequestDTO NotificationTypeDTO);
        public Task<NotificationTypeRequestDTO> UpdateNotificationType(string TypeCode, NotificationTypeRequestDTO NotificationTypeDTO);
        public Task<NotificationTypeDTO> DeleteNotificationType(string NotificationTypeCode);
    }
}
