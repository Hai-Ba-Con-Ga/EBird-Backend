using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EBird.Application.Exceptions;
using EBird.Application.Interfaces;
using EBird.Application.Model.Notification;
using EBird.Application.Services.IServices;
using EBird.Domain.Entities;

namespace EBird.Application.Services
{
    public class NotificationService : INotificationService
    {
        
        private IWapperRepository _repository;
        private IMapper _mapper;

        public NotificationService(IWapperRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<NotificationCreateDTO> AddNotification(NotificationCreateDTO notificationCreateDTO)
        {
            // await NotificationValidation.ValidateNotification(NotificationDTO, _repository);

            NotificationEntity notificationEntity = _mapper.Map<NotificationEntity>(notificationCreateDTO);

            notificationEntity = await _repository.Notification.AddNotificationAsync(notificationEntity);

            if(notificationEntity == null)
            {
                throw new BadRequestException("Notification is not added");
            }
            
            return notificationCreateDTO;
        }

        public async Task<NotificationDTO> DeleteNotification(Guid NotificationID)
        {
            var NotificationEntity = await _repository.Notification.SoftDeleteNotificationAsync(NotificationID);
            
            if(NotificationEntity == null)
            {
                throw new NotFoundException("Not found Notification for delete");
            }
            
            return _mapper.Map<NotificationDTO>(NotificationEntity);
        }

        public async Task<NotificationDTO> GetNotification(Guid NotificationID)
        {
            var NotificationEntity = await _repository.Notification.GetNotificationActiveAsync(NotificationID);
            
            if(NotificationEntity == null)
            {
                throw new NotFoundException("Can not found Notification");
            }
            
            var NotificationDTO = _mapper.Map<NotificationDTO>(NotificationEntity);
            
            return NotificationDTO;
        }

        public async Task<List<NotificationDTO>> GetNotifications()
        {
            var listNotificationEntity = await _repository.Notification.GetNotificationsActiveAsync();
            return _mapper.Map<List<NotificationDTO>>(listNotificationEntity);
        }

        public async Task<NotificationUpdateDTO> UpdateNotification(Guid NotificationID, NotificationUpdateDTO NotificationDTO)
        {
            // await NotificationValidation.ValidateNotification(NotificationDTO, _repository);
            var NotificationEntity = await _repository.Notification.GetNotificationActiveAsync(NotificationID);

            if(NotificationEntity == null)
            {
                throw new NotFoundException("Can not found Notification");
            }

            _mapper.Map(NotificationDTO, NotificationEntity);

            var result = await _repository.Notification.UpdateNotificationAsync(NotificationEntity);

            if(result == null)
            {
                throw new BadRequestException("Update is fail");
            }

            return NotificationDTO;
        }

        public async Task<List<NotificationDTO>> GetAllNotificationsByAccountId(Guid accountId)
        {
            var listNotificationEntity = await _repository.Notification.GetAllNotificationActiveByAccountIdAsync(accountId);
            if(listNotificationEntity.Count == 0)
            {
                throw new NotFoundException("Can not found Notification");
            }
            return _mapper.Map<List<NotificationDTO>>(listNotificationEntity);
        }

	}
}