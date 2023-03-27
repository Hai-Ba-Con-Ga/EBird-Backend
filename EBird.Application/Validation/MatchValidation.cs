using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Application.Exceptions;
using EBird.Application.Interfaces;
using EBird.Application.Interfaces.IValidation;
using EBird.Application.Model.Match;
using EBird.Domain.Enums;

namespace EBird.Application.Validation
{
    public class MatchValidation : BaseValidation, IMatchValidation
    {
        public MatchValidation(IWapperRepository repository) : base(repository)
        {
        }

        public async Task ValidateCreateMatch(MatchCreateDTO matchCreateDTO)
        {
            var request = await _repository.Request.GetByIdActiveAsync(matchCreateDTO.RequestId);

            await this.ValidateAccountId(matchCreateDTO.UserId ?? Guid.Empty);

            if (request == null)
                throw new BadRequestException("Request not found");

            // if (request.IsReady == false)
            //     throw new BadRequestException("Request is not ready");

            if (request.HostId != matchCreateDTO.UserId)
                throw new BadRequestException("You are not the host of this request");

            if (request.Status != RequestStatus.Matched)
                throw new BadRequestException("This request was not matched");

            if (request.ChallengerId == null || request.ChallengerId == Guid.Empty || 
                request.ChallengerBirdId == null || request.ChallengerBirdId == Guid.Empty)
            {
                throw new BadRequestException("This request haven't challenger");
            }
        }
    }
}