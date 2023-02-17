using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Application.Model.PagingModel;
using EBird.Application.Model.Resource;

namespace EBird.Application.Interfaces.IValidation
{
    public interface IBaseValidation
    {
        public Task ValidateAccountId(Guid accountID);
        public Task ValidateGroupId(Guid? groupId);
        public Task ValidateBirdId(Guid birdId);
        public Task ValidatePlaceId(Guid placeId);
        public Task ValidateCreateResourceList(List<ResourceCreateDTO> rsrcList);
        // public Task ValidateRequestId(Guid id);
        public void ValidateParameter(QueryStringParameters parameters);    
        public Task ValidateMatchId(Guid matchId);

    }
}