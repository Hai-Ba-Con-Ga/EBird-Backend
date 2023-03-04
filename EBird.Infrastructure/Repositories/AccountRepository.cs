using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Application.Model.Auth;
using EBird.Application.Model.PagingModel;
using EBird.Application.Services.IServices;
using EBird.Domain.Entities;
using EBird.Infrastructure.Context;

namespace EBird.Infrastructure.Repositories
{
    public class AccountRepository : GenericRepository<AccountEntity>, IAccountRepository
    {
        public AccountRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<PagedList<AccountEntity>> GetAllWithPagination(AccountParameters parameters)
        {
            PagedList<AccountEntity> pageList = new PagedList<AccountEntity>();

            if (parameters.PageSize == 0)
            {
                var list = _context.Accounts
                            .Where(x => x.IsDeleted == false)
                            .OrderByDescending(x => x.CreateDateTime);
              await  pageList.LoadData(list);
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
    }
}