using EBird.Application.Exceptions;
using EBird.Application.Interfaces;
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
            if(account == null)
            {
                throw new BadRequestException("Account ID is not valid");
            }
        }
    }
}
