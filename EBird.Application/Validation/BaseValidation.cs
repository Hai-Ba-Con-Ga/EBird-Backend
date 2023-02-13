using EBird.Application.Exceptions;
using EBird.Application.Interfaces;
using EBird.Application.Model.PagingModel;
using EBird.Application.Model.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Validation
{
    public class BaseValidation
    {
        protected IWapperRepository _repository;

        public BaseValidation(IWapperRepository repository)
        {
            _repository = repository;
        }

        public async Task ValidateAccountId(Guid accountID)
        {
            var account = await _repository.Account.GetByIdActiveAsync(accountID);
            if (account == null)
            {
                throw new BadRequestException("Account ID is not valid");
            }
        }

        public async Task ValidateGroupId(Guid? groupId)
        {
            if (groupId != null)
            {
                var group = await _repository.Group.GetGroupActiveAsync(groupId.Value);
                if (group == null)
                {
                    throw new BadRequestException("Group does not exist");
                }
            }
        }

        public async Task ValidateBirdId(Guid birdId)
        {
            var bird = await _repository.Bird.GetBirdActiveAsync(birdId);
            if (bird == null)
            {
                throw new BadRequestException("Bird does not exist");
            }
        }

        public async Task ValidatePlaceId(Guid placeId)
        {
            var place = await _repository.Place.GetPlace(placeId);
            if (place == null)
            {
                throw new BadRequestException("Place does not exist");
            }
        }

        public async Task ValidateCreateResourceList(List<ResourceCreateDTO> rsrcList)
        {
            try
            {
                foreach (var rsrc in rsrcList)
                {
                    Guid createById = rsrc.CreateById;
                    await ValidateAccountId(createById);
                }
            }
            catch (BadRequestException)
            {
                throw new BadRequestException("Resource's createById is not found");
            }
        }

        public async Task ValidateRequestId(Guid id)
        {
            var request = await _repository.Request.GetRequest(id);
            if (request == null)
            {
                throw new BadRequestException("Request does not exist");
            }
        }

        public void ValidateParameter(QueryStringParameters parameters)
        {
            if (parameters == null)
            {
                throw new BadRequestException("Paging parameters is invalid");
            }
            if (parameters.PageNumber < 0)
            {
                throw new BadRequestException("Page number is invalid");
            }
            if (parameters.PageSize < 0)
            {
                throw new BadRequestException("Page size is invalid");
            }
        }
    }
}
