using EBird.Application.Model;
using EBird.Domain.Entities;
using Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Services.IServices
{
    public interface IAccountServices
    {
        Task<Response<AccountResponse>> GetAccountById(Guid id);
        Task<Response<List<AccountResponse>>> GetAllAccount();
        Task<Response<string>> UpdateAccount(AccountEntity updateAccount);
        Task<Response<string>> DeleteAccount(Guid id);
    }
}
