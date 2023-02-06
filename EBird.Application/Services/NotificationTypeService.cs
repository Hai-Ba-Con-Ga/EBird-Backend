using AutoMapper;
using EBird.Application.Exceptions;
using EBird.Application.Interfaces;
using EBird.Application.Model.NotificationType;
using EBird.Application.Services.IServices;
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
            //await BirdTypeValidation.ValidateBirdTypeDTO(birdTypeDTO, _repository);

            var notificationTypeEntity = _mapper.Map<NotificationTypeEntity>(notificationTypeRequestDTO);

            var updatedEntity = await _repository.NotificationType.AddNotificationTypeAsync(notificationTypeEntity);

            if (updatedEntity == null)
            {
                throw new Exception("Can not insert data to database");
            }

            return notificationTypeRequestDTO;
        }

        public async Task<NotificationTypeDTO> DeleteNotificationType(string NotificationTypeCode)
        {
            throw new NotImplementedException();
        }

        public async Task<NotificationTypeDTO> GetNotificationType(string NotificationTypeCode)
        {
            throw new NotImplementedException();
        }

        public async Task<List<NotificationTypeDTO>> GetNotificationTypes()
        {
            var listNotifiType = await _repository.NotificationType.GetAllNotificationTypesActiveAsync();
            if(listNotifiType == null || listNotifiType.Count == 0)
                throw new NotFoundException("Notification type not found");
            return _mapper.Map<List<NotificationTypeDTO>>(listNotifiType);
        }

        public async Task<NotificationTypeRequestDTO> UpdateNotificationType(string TypeCode, NotificationTypeRequestDTO NotificationTypeDTO)
        {
            throw new NotImplementedException();
        }
    }
}
