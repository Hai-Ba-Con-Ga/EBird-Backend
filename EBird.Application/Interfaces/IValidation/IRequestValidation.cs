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
        
    }
}