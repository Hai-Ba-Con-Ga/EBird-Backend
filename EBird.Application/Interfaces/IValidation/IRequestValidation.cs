using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Application.Model.Request;

namespace EBird.Application.Interfaces.IValidation
{
    public interface IRequestValidation : IBaseValidation
    {
        public Task ValidateCreateRequest(RequestCreateDTO request);
        Task ValidateJoinRequest(Guid requestId, Guid userId, JoinRequestDTO joinRequestDto);
        public void ValidateRequestDatetime(DateTime requestDate);
        public Task ValidateMergeRequest(params Guid[] requestIds);
        public Task ValidateReadyRequest(Guid requestId, Guid userId);
        public Task<bool> ValidateTowRequestIsSameUser(Guid hostRequestID, Guid challengerRequestID);
    }
}