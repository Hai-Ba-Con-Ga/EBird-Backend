using EBird.Application.Model;
using EBird.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Services.IServices
{
    public interface IAuthenticationServices
    {
        Task< TokenModel> CreateToken(AccountEntity account);
        string HashPassword(string password);
        Task<TokenModel> Login(string username, string password);
        Task Logout(Guid id);
        Task Signup(AccountEntity req);
        Task<TokenModel> RenewToken(TokenModel model);
        Task<AccountResponse> GetAccountById(Guid id);
    }
}
