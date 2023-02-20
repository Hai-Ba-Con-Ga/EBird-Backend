using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Application.Exceptions;
using EBird.Application.Interfaces;
using EBird.Application.Interfaces.IValidation;
using EBird.Application.Model.Request;

namespace EBird.Application.Validation
{
    public class RequestValidation : BaseValidation, IRequestValidation
    {
        public RequestValidation(IWapperRepository repository) : base(repository)
        {
        }

        public async Task ValidateCreateRequest(RequestCreateDTO request)
        {
            if (request == null)
            {
                throw new BadRequestException("Request cannot be null");
            }

            await ValidateAccountId(request.HostId ?? Guid.Empty);
            await ValidateGroupId(request.GroupId);
            await ValidateBirdId(request.HostBirdId);
            ValidateRequestDatetime(request.RequestDatetime);
        }

        public async Task ValidateJoinRequest(Guid requestId, Guid userId, JoinRequestDTO joinRequestDto)
        {
            var request = await _repository.Request.GetByIdActiveAsync(requestId);
            if (request == null)
            {
                throw new BadRequestException("Request does not exist");
            }

            if (request.Status != Domain.Enums.RequestStatus.Waiting)
            {
                throw new BadRequestException("Request is not waiting for join");
            }

            await ValidateAccountId(userId);
            if (request.HostId == userId)
            {
                throw new BadRequestException("User cannot join his own request");
            }

            await ValidateBirdId(joinRequestDto.ChallengerBirdId);
            if (request.HostBirdId == joinRequestDto.ChallengerBirdId)
            {
                throw new BadRequestException("Bird have existed in this request");
            }
          
        }

        public void ValidateRequestDatetime(DateTime requestDate)
        {
            if (requestDate < DateTime.Now)
            {
                throw new BadRequestException("Request date cannot be in the past");
            }
        }
    }
}