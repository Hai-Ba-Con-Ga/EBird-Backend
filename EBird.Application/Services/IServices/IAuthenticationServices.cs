using EBird.Application.Model;
using EBird.Domain.Entities;
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
        
    }
}
