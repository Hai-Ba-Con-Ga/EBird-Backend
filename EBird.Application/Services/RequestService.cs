using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EBird.Application.Exceptions;
using EBird.Application.Interfaces;
using EBird.Application.Interfaces.IValidation;
using EBird.Application.Model.PagingModel;
using EBird.Application.Model.Request;
using EBird.Application.Model.Resource;
using EBird.Application.Services.IServices;
using EBird.Application.Validation;
using EBird.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EBird.Application.Services
{
    public class RequestService : IRequestService
    {
        private IMapper _mapper;
        private IWapperRepository _repository;

        private IUnitOfValidation _unitOfValidation;

        public RequestService(IMapper mapper, IWapperRepository repository, IUnitOfValidation unitOfValidation)
        {
            _mapper = mapper;
            _repository = repository;
            _unitOfValidation = unitOfValidation;
        }

        public async Task<Guid> CreateRequest(RequestCreateDTO requestDto)
        {
            await _unitOfValidation.Request.ValidateCreateRequest(requestDto);

            var entity = _mapper.Map<RequestEntity>(requestDto);

            var rowEffect = await _repository.Request.CreateAsync(entity);

            if (rowEffect == 0)
                throw new BadRequestException("Request cannot be created");

            return entity.Id;
        }

        public async Task DeleteRequest(Guid id)
        {
            await _unitOfValidation.Request.ValidateRequestId(id);
            await _repository.Request.DeleteSoftAsync(id);
        }

        public async Task<RequestResponse> GetRequest(Guid id)
        {
            var result = await _repository.Request.GetRequest(id);
            var requestDto = _mapper.Map<RequestResponse>(result);
            return requestDto;
        }
        public async Task<ICollection<RequestResponse>> GetRequestsByGroupId(Guid groupId)
        {
            await _unitOfValidation.Request.ValidateGroupId(groupId);
            var result = await _repository.Request.GetRequestsByGroupId(groupId);
            return _mapper.Map<ICollection<RequestResponse>>(result);
        }

        public async Task<PagedList<RequestResponse>> GetRequests(RequestParameters parameters)
        {
            _unitOfValidation.Request.ValidateParameter(parameters);

            var resultEntityList = await _repository.Request.GetRequests(parameters);

            var requestDTOList = _mapper.Map<PagedList<RequestResponse>>(resultEntityList);
            requestDTOList.MapMetaData(resultEntityList);

            return requestDTOList;
        }

        public async Task<ICollection<RequestResponse>> GetRequests()
        {
            var result = await _repository.Request.GetRequests();
            return _mapper.Map<ICollection<RequestResponse>>(result);
        }

        public async Task JoinRequest(Guid requestId, Guid userId, JoinRequestDTO joinRequestDto)
        {
            await _unitOfValidation.Request.ValidateJoinRequest(requestId, userId, joinRequestDto);

            await _repository.Request.JoinRequest(requestId, userId, joinRequestDto);

        }

        public async Task<Guid> MergeRequest(Guid hostRequestId, Guid challengerRequestId)
        {
            await _unitOfValidation.Request.ValidateMergeRequest(hostRequestId, challengerRequestId);

            return await _repository.Request.MergeRequest(hostRequestId, challengerRequestId);
        }

        public async Task UpdateRequest(Guid id, RequestUpdateDTO request)
        {
            await _unitOfValidation.Request.ValidateRequestId(id);

            var requestEntity = await _repository.Request.GetRequest(id);

            _mapper.Map(request, requestEntity);

            try
            {
                await _repository.Request.UpdateAsync(requestEntity);
            }
            catch (DbUpdateException ex)
            {
                throw new BadRequestException("Request cannot be updated");
            }
        }

        public async Task ReadyRequest(Guid requestId, Guid userId)
        {
            await _unitOfValidation.Request.ValidateReadyRequest(requestId, userId);
            await _repository.Request.ReadyRequest(requestId);
        }
    }
}