using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Application.Interfaces.IRepository;
using EBird.Application.Model.Auth;
using EBird.Application.Model.PagingModel;
using EBird.Domain.Entities;

namespace EBird.Application.Services.IServices
{
    public interface IAccountRepository : IGenericRepository<AccountEntity>
    {
        Task<AccountEntity> GetAccountByUsername(string username);
        Task<PagedList<AccountEntity>> GetAllWithPagination(AccountParameters parameters);
        Task<bool> IsValidToJoinGroup(Guid userId, Guid groupId);
        Task<bool> IsVipAccount(Guid userId);
    }
}