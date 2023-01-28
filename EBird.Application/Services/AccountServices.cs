
using EBird.Application.Interfaces;
using EBird.Application.Model;
using EBird.Application.Services.IServices;
using EBird.Domain.Entities;
using Response;

namespace EBird.Application.Services
{
    public class AccountServices : IAccountServices
    {
        private readonly IGenericRepository<AccountEntity> _accountRepository;

        public AccountServices(IGenericRepository<AccountEntity> accountRepository)
        {
            _accountRepository = accountRepository;
        }
        public async Task<Response<AccountResponse>> GetAccountById(Guid id)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            
            if (account == null)
            {
                return Response<AccountResponse>.Builder().SetStatusCode(400).SetMessage("Account is not exist");
            }
            var accountResponse = new AccountResponse()
            {
                Id = account.Id,
                CreateDateTime = account.CreateDateTime,
                Email = account.Email,
                FirstName = account.FirstName,
                LastName = account.LastName,
                Description = account.Description,
                Role = account.Role,
                Username = account.Username,
            };
            return Response<AccountResponse>.Builder().SetStatusCode(200).SetSuccess(true).SetData(accountResponse);
        }
        public async Task<Response<List<AccountResponse>>> GetAllAccount()
        {
            var accounts = await _accountRepository.GetAllAsync();
            if (accounts == null)
            {
                return Response<List<AccountResponse>>.Builder().SetStatusCode(400).SetMessage("Account is not exist");
            }
            var accountResponses = new List<AccountResponse>();
            foreach (var account in accounts)
            {
                accountResponses.Add(new AccountResponse()
                {
                    Id = account.Id,
                    CreateDateTime = account.CreateDateTime,
                    Email = account.Email,
                    FirstName = account.FirstName,
                    LastName = account.LastName,
                    Description = account.Description,
                    Role = account.Role,
                    Username = account.Username,
                });
            }
            return Response<List<AccountResponse>>.Builder().SetStatusCode(200).SetSuccess(true).SetData(accountResponses);
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
