
using AutoMapper;
using EBird.Application.Interfaces;
using EBird.Application.Model;
using EBird.Application.Services.IServices;
using EBird.Domain.Entities;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Manage.Internal;
using Response;
using System.Net;

namespace EBird.Application.Services
{
    public class AccountServices : IAccountServices
    {
        private readonly IGenericRepository<AccountEntity> _accountRepository;
        private readonly IGenericRepository<VerifcationStoreEntity> _verifcationStoreRepository;

        private readonly IMapper _mapper;
        private readonly IAuthenticationServices _authenticationServices;
        private readonly IEmailServices _emailServices;

        public AccountServices(IGenericRepository<AccountEntity> accountRepository, IGenericRepository<VerifcationStoreEntity> verifcationStoreRepository, IMapper mapper, IAuthenticationServices authenticationServices, IEmailServices emailServices)
        {
            _accountRepository = accountRepository;
            _verifcationStoreRepository = verifcationStoreRepository;
            _mapper = mapper;
            _authenticationServices = authenticationServices;
            _emailServices = emailServices;
        }

        public async Task<Response<AccountResponse>> GetAccountById(Guid id)
        {
            var account = await _accountRepository.GetByIdActiveAsync(id);

            if (account == null)
            {
                return Response<AccountResponse>.Builder().SetStatusCode((int)HttpStatusCode.BadRequest).SetMessage("Account is not exist").SetSuccess(false);
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
                return Response<List<AccountResponse>>.Builder().SetStatusCode((int)HttpStatusCode.BadRequest).SetMessage("Account is not exist").SetSuccess(false);
            }
            var accountResponses = new List<AccountResponse>();
            
            foreach (var account in accounts)
            {
                var accountResponse = new AccountResponse();
                _mapper.Map<AccountEntity, AccountResponse>(account, accountResponse);
                accountResponses.Add(accountResponse);
            }
            return Response<List<AccountResponse>>.Builder().SetStatusCode((int)HttpStatusCode.OK).SetSuccess(true).SetData(accountResponses);
        }
        public async Task<Response<string>> UpdateAccount(AccountEntity updateAccount)
        {
            var account = await _accountRepository.GetByIdActiveAsync(updateAccount.Id);
            if (account == null)
            {
                return Response<string>.Builder().SetStatusCode((int)HttpStatusCode.BadRequest).SetMessage("Account is not exist").SetSuccess(false);
            }
            await _accountRepository.UpdateAsync(account);
            return Response<string>.Builder().SetStatusCode((int)HttpStatusCode.OK).SetMessage("Update Successful").SetSuccess(true);

        }
        public async Task<Response<string>> DeleteAccount(Guid id)
        {
            var account = await _accountRepository.DeleteSoftAsync(id);
            if (account == null)
            {
                return Response<string>.Builder().SetStatusCode((int)HttpStatusCode.BadRequest).SetMessage("Delete Failed").SetSuccess(false);
            }
            return Response<string>.Builder().SetStatusCode((int)HttpStatusCode.OK).SetMessage("Delete Successful").SetSuccess(true);
        }
        public async Task<Response<string>> ChangePassword(Guid id, Model.ChangePasswordModel model)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            if (account == null)
            {
                throw new Exception("The account is not exist");
            }
            var match = _authenticationServices.HashPassword(model.OldPassword).Equals(account.Password);
            if (!match)
            {
                throw new Exception("The password is incorrect");
            }

            account.Password = _authenticationServices.HashPassword(model.NewPassword);
            await _accountRepository.UpdateAsync(account);
            return Response<string>.Builder().SetStatusCode((int)HttpStatusCode.OK).SetMessage("Change Password Successful").SetSuccess(true);
        }
        public async Task<Response<string>> ForgotPassword(string username)
        {
            var account = await _accountRepository.FindWithCondition(p => p.Username.Equals(username));
            if (account == null)
            {
                throw new Exception("The account is not exist");
            }
            Console.WriteLine(account.Id);
            var code = new Random().Next(10000, 99999).ToString();
            var model = new SendForgotPasswordModel()
            {
                Code = code,
                Email = account.Email,
                FirstName = account.FirstName,
                UserName = account.LastName,
                ResetPasswordLink = $"https://localhost:7173/resetPassword/{account.Id}/Code/{code}"
            };
            var verifcation = new VerifcationStoreEntity()
            {
                AccountId = account.Id,
                Code = code,
            };
            await _verifcationStoreRepository.CreateAsync(verifcation);

            await _emailServices.SendForgotPassword(model);
            return Response<string>.Builder().SetStatusCode((int)HttpStatusCode.OK).SetSuccess(true);
        }
        public async Task<Response<string>> ResetPassword(ResetPasswordModel model)
        {
            var verification = await _verifcationStoreRepository.FindWithCondition(x => x.AccountId.Equals(model.AccountId) && x.Code.Equals(model.Code));
            if(verification == null)
            {
                throw new Exception("Code is not exist");
            }
            var account = await _accountRepository.GetByIdAsync(model.AccountId);
            if (account == null)
            {
                throw new Exception("The account is not exist");
            }
            account.Password = _authenticationServices.HashPassword(model.Password);
            await _accountRepository.UpdateAsync(account);
            await _verifcationStoreRepository.DeleteAsync(verification.Id);
            return Response<string>.Builder().SetStatusCode((int)HttpStatusCode.OK).SetSuccess(true).SetMessage("Reset Password Successful");

        }
        public async Task<Response<string>> CheckEmail(string email)
        {
            var account = await _accountRepository.FindWithCondition(x=> x.Email.Equals(email)); 
            if (account == null)
            {
                return Response<string>.Builder().SetStatusCode((int)HttpStatusCode.BadRequest).SetMessage("Email is not exist").SetSuccess(false);

            }
            return Response<string>.Builder().SetStatusCode((int)HttpStatusCode.OK).SetSuccess(true);

        }


    }
}
