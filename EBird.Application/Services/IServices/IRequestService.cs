﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBird.Application.Model.PagingModel;
using EBird.Application.Model.Request;

namespace EBird.Application.Services.IServices
{
    public interface IRequestService
    {
        public Task<Guid> CreateRequest(RequestCreateDTO request);
        public Task UpdateRequest(Guid id, RequestUpdateDTO request);
        public Task DeleteRequest(Guid id);
        public Task<RequestResponseDTO> GetRequest(Guid id);
        public Task<PagedList<RequestResponseDTO>> GetRequests(RequestParameters parameters);
        public Task<ICollection<RequestResponseDTO>> GetRequests();
        public Task<PagedList<RequestResponseDTO>> GetRequestsByGroupId(Guid groupId, RequestParameters parameters = null);
        public Task JoinRequest(Guid requestId, Guid userId, JoinRequestDTO joinRequestDto);
        public Task<Guid> MergeRequest(Guid hostRequestId, Guid challengerRequestId);
        Task ReadyRequest(Guid requestId, Guid userId);
        Task<ICollection<RequestResponseDTO>> GetRequestByUserId(Guid userId);
        Task<bool> CheckRequest(Guid hostRequestID, Guid challengerRequestID);
        Task LeaveRequest(Guid requestId, Guid userId);
        Task KickFromRequest(Guid requestId, Guid userId, Guid kickedUserId);
    }
}
