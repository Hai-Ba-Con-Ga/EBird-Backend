using EBird.Application.Interfaces;
using EBird.Application.Services.IServices;
using EBird.Domain.Entities;
using Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Services
{
    public class AccountServices : IAccountServices
    {
        private readonly IGenericRepository<AccountEntity> _accountRepository;

        public AccountServices(IGenericRepository<AccountEntity> accountRepository)
        {
            _accountRepository = accountRepository;
        }
        public async Task<Response<AccountEntity>> GetAccountById(Guid id)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            if (account == null)
            {
                return Response<AccountEntity>.Builder().SetStatusCode(400).SetMessage("Account is not exist");
            }
            return Response<AccountEntity>.Builder().SetStatusCode(200).SetSuccess(true).SetData(account);
        }
        public async Task<Response<List<AccountEntity>>> GetAllAccount()
        {
            var accounts = await _accountRepository.GetAllAsync();
            if (accounts == null)
            {
                return Response<List<AccountEntity>>.Builder().SetStatusCode(400).SetMessage("Account is not exist");
            }
            return Response<List<AccountEntity>>.Builder().SetStatusCode(200).SetSuccess(true).SetData(accounts);
        }
        public async Task<Response<string>> UpdateAccount(AccountEntity updateAccount)
        {
            var account = await _accountRepository.GetByIdAsync(updateAccount.Id);
            if (account == null)
            {
                return Response<string>.Builder().SetStatusCode(400).SetMessage("Account is not exist");
            }
            await _accountRepository.UpdateAsync(account);
            return Response<string>.Builder().SetStatusCode(200).SetMessage("Update Successful").SetSuccess(true);

        }
        public async Task<Response<string>> DeleteAccount(Guid id)
        {
            
            await _accountRepository.DeleteSoftAsync(id);
            return Response<string>.Builder().SetStatusCode(200).SetMessage("Delete Successful").SetSuccess(true);
        }


    }
}
