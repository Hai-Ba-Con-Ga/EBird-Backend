using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EBird.Application.Exceptions;
using EBird.Application.Interfaces;
using EBird.Application.Model.PagingModel;
using EBird.Application.Model.Request;
using EBird.Application.Services.IServices;
using EBird.Application.Validation;
using EBird.Domain.Entities;

namespace EBird.Application.Services
{
    public class RequestService : IRequestService
    {
        private IMapper _mapper;
        private IWapperRepository _repository;

        public RequestService(IMapper mapper, IWapperRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<Guid> CreateRequest(RequestCreateDTO requestDto)
        {
            if (requestDto == null)
            {
                throw new BadRequestException("Request cannot be null");
            }

            await RequestValidation.ValidateCreateRequest(requestDto, _repository);

            var requestEntity = _mapper.Map<RequestEntity>(requestDto);

            return await _repository.Request.CreateRequest(requestEntity);
        }

        public async Task DeleteRequest(Guid id)
        {
            await _repository.Request.DeleteRequest(id);
        }

        public async Task<RequestResponse> GetRequest(Guid id)
        {
            var result = await _repository.Request.GetRequest(id);
            return _mapper.Map<RequestResponse>(result);
        }

        public async Task<PagedList<RequestResponse>> GetRequests(RequestParameters parameters)
        {
            RequestValidation.ValidateParameter(parameters);

            var resultEntityList  = await _repository.Request.GetRequests(parameters);

            var requestDTOList = _mapper.Map<PagedList<RequestResponse>>(resultEntityList);
            requestDTOList.MapMetaData(resultEntityList);

            return requestDTOList;
        }

        public async Task<ICollection<RequestResponse>> GetRequests()
        {
            var result = await _repository.Request.GetRequests();
            return _mapper.Map<ICollection<RequestResponse>>(result);
        }

        public async Task UpdateRequest(Guid id, RequestUpdateDTO request)
        {
            await RequestValidation.ValidateRequestId(id, _repository);

            var requestEntity = await _repository.Request.GetRequest(id);
            
            await _repository.Request.UpdateRequest(requestEntity);
        }
    }
}