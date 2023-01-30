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
        Task<TokenModel> CreateToken(AccountEntity account);
        string HashPassword(string password);
        Task<ActionResult<Response<TokenModel>>> Login(string username, string password);
        Task<ActionResult<Response<string>>> Logout(Guid id);
        Task<ActionResult<Response<string>>> Signup(AccountEntity req);
        Task<ActionResult<Response<TokenModel>>> RenewToken(TokenModel model);
        Task<Response<AccountResponse>> GetAccountById(Guid id);
    }
}
