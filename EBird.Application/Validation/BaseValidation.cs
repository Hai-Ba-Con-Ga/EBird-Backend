using EBird.Application.Exceptions;
using EBird.Application.Interfaces;
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
        public static async Task ValidateAccountId(Guid accountID, IWapperRepository _repository)
        {
            var account = await _repository.Account.GetByIdActiveAsync(accountID);
            if (account == null)
            {
                throw new BadRequestException("Account ID is not valid");
            }
        }

        public static async Task ValidateCreateResourceList(List<ResourceCreateDTO> rsrcList, IWapperRepository _repository)
        {
            try
            {
                foreach (var rsrc in rsrcList)
                {
                    Guid createById = rsrc.CreateById;
                    await ValidateAccountId(createById, _repository);
                }
            }
            catch (BadRequestException)
            {
                throw new BadRequestException("Resource's createById is not found");
            }
        }
    }
}
