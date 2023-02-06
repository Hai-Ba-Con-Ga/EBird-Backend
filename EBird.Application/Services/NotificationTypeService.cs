using AutoMapper;
using EBird.Application.Exceptions;
using EBird.Application.Interfaces;
using EBird.Application.Model.NotificationType;
using EBird.Application.Services.IServices;
using EBird.Application.Validation;
using EBird.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Services
{
    public class NotificationTypeService : INotificationTypeService
    {
        private IWapperRepository _repository;
        private IMapper _mapper;

        public NotificationTypeService(IWapperRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<NotificationTypeRequestDTO> AddNotificationType(NotificationTypeRequestDTO notificationTypeRequestDTO)
        {
            await notificationTypeValidation.ValidationNotificationType(notificationTypeRequestDTO, _repository);
            var notificationTypeEntity = _mapper.Map<NotificationTypeEntity>(notificationTypeRequestDTO);

            var updatedEntity = await _repository.NotificationType.AddNotificationTypeAsync(notificationTypeEntity);

            if (updatedEntity == null)
                throw new Exception("Can not add Notification Type to database");
            return notificationTypeRequestDTO;
        }

        public async Task<NotificationTypeDTO> DeleteNotificationType(Guid id)
        {
            var result = await _repository.NotificationType.DeleteSoftAsync(id);
            if (result == null)
                throw new NotFoundException("Notification type not found");

            var birdTypeDeletedDTO = _mapper.Map<NotificationTypeDTO>(result);

            return birdTypeDeletedDTO;
        }

        public async Task<NotificationTypeDTO> GetNotificationType(Guid Id)
        {
            var result = await _repository.NotificationType.GetNotificationTypeActiveAsync(Id);

            if (result == null)
                throw new NotFoundException("Notification Type not found");
            return _mapper.Map<NotificationTypeDTO>(result);
        }

        public async Task<List<NotificationTypeDTO>> GetNotificationTypes()
        {
            return _mapper.Map<List<NotificationTypeDTO>>(await _repository.NotificationType.GetAllNotificationTypesActiveAsync());
        }

        public async Task<NotificationTypeRequestDTO> UpdateNotificationType(Guid Id, NotificationTypeRequestDTO notificationTypeDTO)
        {
            await notificationTypeValidation.ValidationNotificationType(notificationTypeDTO, _repository);

            var notificationType = await _repository.NotificationType.GetByIdAsync(Id);
            if (notificationType == null)
                throw new NotFoundException("Can not found notification type for updating");

            _mapper.Map(notificationTypeDTO, notificationType);

            int rowEffect = await _repository.NotificationType.UpdateAsync(notificationType);
            if (rowEffect == 0)
                throw new Exception("Can update notification type to database");

            return notificationTypeDTO;
        }
    }
}
