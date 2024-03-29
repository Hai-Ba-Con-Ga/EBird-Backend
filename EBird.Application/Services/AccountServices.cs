﻿
using AutoMapper;
using Duende.IdentityServer.Models;
using EBird.Application.Exceptions;
using EBird.Application.Interfaces;
using EBird.Application.Interfaces.IRepository;
using EBird.Application.Interfaces.IValidation;
using EBird.Application.Model.Auth;
using EBird.Application.Model.PagingModel;
using EBird.Application.Model.Resource;
using EBird.Application.Services.IServices;
using EBird.Domain.Entities;
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
        private readonly IUnitOfValidation _validation;
        private readonly IWapperRepository _repository;

        public AccountServices(IGenericRepository<AccountEntity> accountRepository, IGenericRepository<VerifcationStoreEntity> verifcationStoreRepository, IMapper mapper, IAuthenticationServices authenticationServices, IEmailServices emailServices, IUnitOfValidation validation, IWapperRepository repository)
        {
            _accountRepository = accountRepository;
            _verifcationStoreRepository = verifcationStoreRepository;
            _mapper = mapper;
            _authenticationServices = authenticationServices;
            _emailServices = emailServices;
            _validation = validation;
            _repository = repository;
        }

        public async Task<AccountResponse> GetAccountById(Guid id)
        {
            var account = await _accountRepository.GetByIdActiveAsync(id);

            if(account == null)
            {
                throw new NotFoundException("Account is not exist");
            }
            var accountResponse = new AccountResponse();
            _mapper.Map<AccountEntity, AccountResponse>(account, accountResponse);

            var resources = await _repository.Resource.GetResourcesByAccount(account.Id);

            accountResponse.Resources = _mapper.Map<ICollection<ResourceResponse>>(resources);

            return accountResponse;
        }

        public async Task<List<AccountResponse>> GetAllAccount()
        {
            var accounts = await _accountRepository.GetAllActiveAsync();
            if(accounts == null)
            {
                throw new NotFoundException("Account is not exist");
            }
            var accountList = new List<AccountResponse>();

            foreach(var account in accounts)
            {
                var accountResponse = new AccountResponse();

                _mapper.Map<AccountEntity, AccountResponse>(account, accountResponse);

                var resources = await _repository.Resource.GetResourcesByAccount(account.Id);

                accountResponse.Resources = _mapper.Map<ICollection<ResourceResponse>>(resources);

                accountList.Add(accountResponse);
            }
            return accountList;
        }

        public async Task UpdateAccount(AccountEntity updateAccount)
        {
            var account = await _accountRepository.GetByIdActiveAsync(updateAccount.Id);
            if(account == null)
            {
                throw new NotFoundException("Account is not exist");
            }
            await _accountRepository.UpdateAsync(account);

        }
        public async Task DeleteAccount(Guid id)
        {
            await _validation.Base.ValidateAccountId(id);

            var account = await _accountRepository.DeleteSoftAsync(id);

            if(account == null)
            {
                throw new NotFoundException("Account is not exist");
            }
        }
        public async Task ChangePassword(Guid id, ChangePasswordModel model)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            if(account == null)
            {
                throw new NotFoundException("The account is not exist");
            }
            var match = _authenticationServices.HashPassword(model.OldPassword).Equals(account.Password);
            if(!match)
            {
                throw new BadRequestException("The password is incorrect");
            }

            account.Password = _authenticationServices.HashPassword(model.NewPassword);
            await _accountRepository.UpdateAsync(account);

        }
        public async Task ForgotPassword(string username)
        {
            var account = await _accountRepository.FindWithCondition(p => p.Username.Equals(username));
            if(account == null)
            {
                throw new NotFoundException("The account is not exist");
            }
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

        }
        public async Task ResetPassword(ResetPasswordModel model)
        {
            var verification = await _verifcationStoreRepository.FindWithCondition(x => x.AccountId.Equals(model.AccountId) && x.Code.Equals(model.Code));
            if(verification == null)
            {
                throw new NotFoundException("Code is not exist");
            }
            var account = await _accountRepository.GetByIdAsync(model.AccountId);
            if(account == null)
            {
                throw new NotFoundException("The account is not exist");
            }
            account.Password = _authenticationServices.HashPassword(model.Password);
            await _accountRepository.UpdateAsync(account);
            await _verifcationStoreRepository.DeleteAsync(verification.Id);


        }
        public async Task CheckEmail(string email)
        {
            var account = await _accountRepository.FindWithCondition(x => x.Email.Equals(email));
            if(account == null)
            {
                throw new NotFoundException("Email is not exist");

            }
        }
        public async Task ChangeRoleAdmin(Guid id)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            if(account == null)
            {
                throw new NotFoundException("The account is not exist");
            }
            account.Role = Domain.Enums.RoleAccount.Admin;
            await _accountRepository.UpdateAsync(account);
        }

        public async Task<IList<AccountResponse>> GetAllAccountWithPagination(AccountParameters parameters)
        {
            _validation.Base.ValidateParameter(parameters);

            PagedList<AccountEntity> accountList = await _repository.Account.GetAllWithPagination(parameters);

            PagedList<AccountResponse> accountResponseList = _mapper.Map<PagedList<AccountResponse>>(accountList);
            accountResponseList.MapMetaData(accountList);

            foreach(var accountReponse in accountResponseList)
            {
                var resources = await _repository.Resource.GetResourcesByAccount(accountReponse.Id);

                accountReponse.Resources = _mapper.Map<ICollection<ResourceResponse>>(resources);
            }

            return accountResponseList;
        }

        public async Task<AccountResponse> GetAccountByUsername(string username)
        {
            var account = await _repository.Account.GetAccountByUsername(username);

            var accountReponse = _mapper.Map<AccountResponse>(account);

            var resources = await _repository.Resource.GetResourcesByAccount(account.Id);
            
            accountReponse.Resources = _mapper.Map<ICollection<ResourceResponse>>(resources);

            return accountReponse;
        }

    }
}
