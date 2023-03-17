using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Application.Exceptions;
using EBird.Application.Model.Auth;
using EBird.Application.Model.PagingModel;
using EBird.Application.Services.IServices;
using EBird.Domain.Entities;
using EBird.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EBird.Infrastructure.Repositories
{
    public class AccountRepository : GenericRepository<AccountEntity>, IAccountRepository
    {
        public AccountRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<AccountEntity> GetAccountByUsername(string username)
        {
          return await  dbSet.Include(a => a.Resources)
                             .FirstOrDefaultAsync(a => a.Username.Equals(username));
        }
        
        public async Task<PagedList<AccountEntity>> GetAllWithPagination(AccountParameters parameters)
        {
            PagedList<AccountEntity> pageList = new PagedList<AccountEntity>();

            if(parameters.PageSize == 0)
            {
                var list = _context.Accounts
                            .Where(x => x.IsDeleted == false)
                            .OrderByDescending(x => x.CreateDateTime);
                await pageList.LoadData(list);
            }
            else
            {
                var list = _context.Accounts
                            .Where(x => x.IsDeleted == false)
                            .OrderByDescending(x => x.CreateDateTime);
                await pageList.LoadData(list, parameters.PageNumber, parameters.PageSize);
            }

            return pageList;
        }

        public async Task<bool> IsValidToJoinGroup(Guid userId, Guid groupId)
        {
            var account = await _context.Accounts
                            .Include(a => a.Birds)
                            .Where(a => a.Id == userId && a.IsDeleted == false).FirstOrDefaultAsync()
                            ?? throw new BadRequestException("Account is not exist");

            var group = await _context.Groups
                            .FirstOrDefaultAsync(g => g.Id == groupId)
                            ?? throw new BadRequestException("Group is not exist");

            return account.Birds.Any(b => b.Elo >= group.MinELO && b.Elo <= group.MaxELO);
        }

        public async Task<bool> IsVipAccount(Guid userId)
        {
            var account = await dbSet
                            .Include(a => a.VipRegistrations)
                            .Where(a => a.Id == userId && a.IsDeleted == false).FirstOrDefaultAsync();

            if(account == null)
            {
                throw new BadRequestException("Account is not exist");
            }

            var vipRegistration = account.VipRegistrations
                                    .OrderByDescending(v => v.CreatedDate)
                                    .FirstOrDefault(v => v.IsDeleted == false);

            if(vipRegistration?.ExpiredDate > DateTime.Now)
            {
                return true;
            }

            return false;
        }
    }
}