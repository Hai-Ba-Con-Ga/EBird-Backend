using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Application.Exceptions;
using EBird.Application.Interfaces;
using EBird.Application.Model.Request;

namespace EBird.Application.Validation
{
    public class RequestValidation : BaseValidation
    {
        public static async Task ValidateCreateRequest(RequestCreateDTO request, IWapperRepository _repository)
        {
            await ValidateAccountId(request.CreatedById, _repository);
            await ValidateGroupId(request.GroupId, _repository);
            await ValidateBirdId(request.BirdId, _repository);
            await ValidatePlaceId(request.PlaceId, _repository);
            await ValidateRequestDatetime(request.RequestDatetime, _repository);
        }

        public static async Task ValidateRequestDatetime(DateTime requestDate, IWapperRepository _repository)
        {
            if (requestDate < DateTime.Now)
            {
                throw new BadRequestException("Request date cannot be in the past");
            }
        }
    }
}