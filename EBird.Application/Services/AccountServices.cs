
using AutoMapper;
using EBird.Application.Interfaces;
using EBird.Application.Model;
using EBird.Application.Services.IServices;
using EBird.Domain.Entities;
using Response;
using System.Net;

namespace EBird.Application.Services
{
    public class AccountServices : IAccountServices
    {
        private readonly IGenericRepository<AccountEntity> _accountRepository;
        private readonly IMapper _mapper;

        public AccountServices(IGenericRepository<AccountEntity> accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        public async Task<Response<AccountResponse>> GetAccountById(Guid id)
        {
            var account = await _accountRepository.GetByIdActiveAsync(id);
            
            if (account == null)
            {
                return Response<AccountResponse>.Builder().SetStatusCode((int)HttpStatusCode.BadRequest).SetMessage("Account is not exist");
            }
            var accountResponse = new AccountResponse();
            _mapper.Map<AccountEntity, AccountResponse>(account, accountResponse);
            
            return Response<AccountResponse>.Builder().SetStatusCode((int)HttpStatusCode.OK).SetSuccess(true).SetData(accountResponse);
        }
        public async Task<Response<List<AccountResponse>>> GetAllAccount()
        {
            var accounts = await _accountRepository.GetAllActiveAsync();
            if (accounts == null)
            {
                return Response<List<AccountResponse>>.Builder().SetStatusCode((int)HttpStatusCode.BadRequest).SetMessage("Account is not exist");
            }
            var accountResponses = new List<AccountResponse>();
            var accountResponse = new AccountResponse();
            foreach (var account in accounts)
            {
                _mapper.Map<AccountEntity, AccountResponse>(account, accountResponse);
                accountResponses.Add(accountResponse);
            }
            return Response<List<AccountResponse>>.Builder().SetStatusCode((int)HttpStatusCode.OK).SetSuccess(true).SetData(accountResponses);
        }
        public async Task<Response<string>> UpdateAccount(AccountEntity updateAccount)
        {
            var account = await _accountRepository.GetByIdAsync(updateAccount.Id);
            if (account == null)
            {
                return Response<string>.Builder().SetStatusCode((int)HttpStatusCode.BadRequest).SetMessage("Account is not exist");
            }
            await _accountRepository.UpdateAsync(account);
            return Response<string>.Builder().SetStatusCode((int)HttpStatusCode.OK).SetMessage("Update Successful").SetSuccess(true);

        }
        public async Task<Response<string>> DeleteAccount(Guid id)
        {
            await _accountRepository.DeleteSoftAsync(id);
            return Response<string>.Builder().SetStatusCode((int)HttpStatusCode.OK).SetMessage("Delete Successful").SetSuccess(true);
        }


    }
}
