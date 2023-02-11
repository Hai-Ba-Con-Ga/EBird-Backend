using EBird.Application.Exceptions;
using EBird.Application.Interfaces.IRepository;
using EBird.Application.Model.PagingModel;
using EBird.Application.Model.Request;
using EBird.Domain.Entities;
using EBird.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Infrastructure.Repositories
{
    public class RequestRepository : GenericRepository<RequestEntity>, IRequestRepository
    {
        public RequestRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Guid> CreateRequest(RequestEntity request)
        {
            request.CreateDatetime = DateTime.Now;

            var result = await this.CreateAsync(request);

            if (result == null)
            {
                throw new BadRequestException("Request not created");
            }
            return request.Id;
        }

        public async Task DeleteRequest(Guid id)
        {
            var result = await this.DeleteAsync(id);

            if (result == null)
            {
                throw new BadRequestException("Request not found");
            }
        }

        public async Task<RequestEntity> GetRequest(Guid id)
        {
            var entity = await dbSet
                .Include(e => e.Group)
                .Include(e => e.Bird)
                .Include(e => e.CreatedBy)
                .Include(e => e.Place)
                .Where(e => e.Id == id && e.IsDeleted == false)
                .FirstOrDefaultAsync();
            return entity;
        }
        
        public async Task<ICollection<RequestEntity>> GetRequests()
        {
           return await this.GetAllActiveAsync();
        }

        public async Task<PagedList<RequestEntity>> GetRequests(RequestParameters parameters)
        {
            var requests = dbSet.AsNoTracking();
            requests = requests.OrderByDescending(r => r.CreateDatetime);

            PagedList<RequestEntity> pagedRequests = new PagedList<RequestEntity>();
            await pagedRequests.LoadData(requests, parameters.PageNumber, parameters.PageSize);

            return pagedRequests;
        }

        public async Task UpdateRequest(RequestEntity entity)
        {
            var result = await this.UpdateAsync(entity);

            if (result == 0)
            {
                throw new BadRequestException("Request isnt updated");
            }
        }
    }
}
