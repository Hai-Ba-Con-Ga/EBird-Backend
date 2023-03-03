using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Application.Exceptions;
using EBird.Application.Interfaces;
using EBird.Application.Interfaces.IValidation;
using EBird.Application.Model.Request;
using EBird.Domain.Enums;

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

            if (request.Status != RequestStatus.Waiting)
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

        public async Task ValidateLeaveRequest(Guid requestId, Guid userId)
        {
            await ValidateAccountId(userId);
            await ValidateRequestId(requestId);

            var request = await _repository.Request.GetRequest(requestId);

            bool isChallenger = request.ChallengerId == userId;

            if (isChallenger == false)
            {
                throw new BadRequestException("User is not challenger");
            }
        }

        public async Task ValidateMergeRequest(params Guid[] requestIds)
        {
            foreach (var id in requestIds)
            {
                if (id == Guid.Empty)
                {
                    throw new BadRequestException("Request Id cannot be empty");
                }

                var request = await _repository.Request.GetByIdActiveAsync(id);

                if (request == null)
                {
                    throw new BadRequestException("Request does not exist");
                }

                // if (request.Status != RequestStatus.Waiting)
                // {
                //     throw new BadRequestException("Request is not waiting for M");
                // }

                if (request.ExpDatetime < DateTime.Now)
                {
                    throw new BadRequestException("Request is expired");
                }

                if (request.ChallengerBirdId != null || request.ChallengerId != null)
                {
                    throw new BadRequestException("Request had a challenger");
                }
            }

           var isValid =  await ValidateTowRequestIsSameUser(requestIds[0], requestIds[1]);

           if (isValid == false)
            throw new BadRequestException("Request is same user or same bird");
        }

        public async Task ValidateReadyRequest(Guid requestId, Guid userId)
        {
            if (requestId == Guid.Empty)
            {
                throw new BadRequestException("Request Id cannot be empty");
            }

            var request = await _repository.Request.GetByIdActiveAsync(requestId);

            if (request == null)
            {
                throw new BadRequestException("Request does not exist");
            }

            if (request.Status != RequestStatus.Matched)
            {
                throw new BadRequestException("Request is not matched for ready");
            }

            await ValidateAccountId(userId);

            if (request.ChallengerId != userId)
            {
                throw new BadRequestException("User is not a challenger for ready this request");
            }

        }

        public void ValidateRequestDatetime(DateTime requestDate)
        {
            if (requestDate < DateTime.Now)
            {
                throw new BadRequestException("Request date cannot be in the past");
            }
        }

        public async Task<bool> ValidateTowRequestIsSameUser(Guid hostRequestID, Guid challengerRequestID)
        {
            if (hostRequestID == challengerRequestID)
            {
                return false;
            }

            var hostRequest = await _repository.Request.GetByIdActiveAsync(hostRequestID);

            if (hostRequest == null)
            {
                return false;
            }
            
            var challengerRequest = await _repository.Request.GetByIdActiveAsync(challengerRequestID);

            if (challengerRequest == null)
            {
                return false;
            }

            if (hostRequest.HostId == challengerRequest.HostId)
            {
                return false;
            }

            if (hostRequest.HostBirdId == challengerRequest.HostBirdId)
            {
                return false;
            }

            return true;

        }

        
    }
}