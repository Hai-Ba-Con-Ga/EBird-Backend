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
            await ValidateAccountId(request.CreatedById ?? Guid.Empty);
            await ValidateGroupId(request.GroupId);
            await ValidateBirdId(request.BirdId);
            await ValidatePlaceId(request.PlaceId);
            ValidateRequestDatetime(request.RequestDatetime);
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