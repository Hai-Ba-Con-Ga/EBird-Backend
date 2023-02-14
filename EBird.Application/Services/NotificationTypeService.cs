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

        public async Task<Guid> AddNotificationType(NotificationTypeRequestDTO ntDTO)
        {
            // check validation
            await NotificationTypeValidation.ValidationNotificationType(ntDTO, _repository);
            // convert DTO to Entity
            var entity = _mapper.Map<NotificationTypeEntity>(ntDTO);
            // update to DB
            return await _repository.NotificationType.AddNotificationTypeAsync(entity);
        }

        public async Task UpdateNotificationType(Guid Id, NotificationTypeRequestDTO ntDTO)
        {
            // check validation
            await NotificationTypeValidation.ValidationNotificationType(ntDTO, _repository);
            
            var entity = await _repository.NotificationType.GetNotificationTypeActiveAsync(Id);
            if (entity == null)
                throw new NotFoundException("Can not found notification type for updating");

            _mapper.Map(ntDTO, entity);
            await _repository.NotificationType.UpdateNotificationTypeAsync(entity);
        }

        public async Task DeleteNotificationType(Guid id)
        {
            await _repository.NotificationType.DeleteSoftAsync(id);
        }
        public async Task<List<NotificationTypeDTO>> GetNotificationTypes()
        {
            var result = await _repository.NotificationType.GetAllNotificationTypesActiveAsync();
            if (result == null)
                throw new NotFoundException("Notification Type not found");
            return _mapper.Map<List<NotificationTypeDTO>>(result);
        }

        public async Task<NotificationTypeDTO> GetNotificationType(Guid Id)
        {
            var result = await _repository.NotificationType.GetNotificationTypeActiveAsync(Id);
            if (result == null)
                throw new NotFoundException("Notification Type not found");
            return _mapper.Map<NotificationTypeDTO>(result);
        }


    }
}
