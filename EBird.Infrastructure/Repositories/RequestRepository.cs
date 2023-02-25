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

        // public async Task<Guid> CreateRequest(RequestEntity request)
        // {
        //     request.CreateDatetime = DateTime.Now;

        //     var result = await this.CreateAsync(request);

        //     if (result == null)
        //     {
        //         throw new BadRequestException("Request not created");
        //     }
        //     return request.Id;
        // }

        // public async Task DeleteRequest(Guid id)
        // {
        //     var result = await this.DeleteAsync(id);

        //     if (result == null)
        //     {
        //         throw new BadRequestException("Request not found");
        //     }
        // }

        public async Task<RequestEntity> GetRequest(Guid id)
        {
            var entity = await dbSet
                .Include(e => e.Group)
                .Include(e => e.HostBird)
                .Include(e => e.Host)
                .Include(e => e.Place)
                .Include(e => e.Room)
                .Where(e => e.Id == id && e.IsDeleted == false)
                .FirstOrDefaultAsync();
            return entity;
        }

        public async Task<ICollection<RequestEntity>> GetRequests()
        {
            return await dbSet.AsNoTracking()
                                .OrderByDescending(r => r.CreateDatetime)
                                .Include(e => e.Group)
                                .Include(e => e.HostBird)
                                .Include(e => e.Host)
                                .Include(e => e.Place)
                                .Include(e => e.Room)
                                .Where(e => e.IsDeleted == false)
                                .ToListAsync();
        }

        public async Task<PagedList<RequestEntity>> GetRequests(RequestParameters parameters)
        {
            var requests = dbSet.AsNoTracking()
                                .OrderByDescending(r => r.CreateDatetime)
                                .Include(e => e.Group)
                                .Include(e => e.HostBird)
                                .Include(e => e.Host)
                                .Include(e => e.Place)
                                .Include(e => e.Room)
                                .Where(e => e.IsDeleted == false);

            PagedList<RequestEntity> pagedRequests = new PagedList<RequestEntity>();
            await pagedRequests.LoadData(requests, parameters.PageNumber, parameters.PageSize);

            return pagedRequests;
        }

        public async Task JoinRequest(Guid requestId, Guid userId, JoinRequestDTO joinRequestDto)
        {
            var request = await dbSet
                             .Where(r => r.Id == requestId && r.IsDeleted == false)
                             .FirstOrDefaultAsync();

            request.ChallengerId = userId;
            request.ChallengerBirdId = joinRequestDto.ChallengerBirdId;
            request.Status = Domain.Enums.RequestStatus.Matched;
            request.ExpDatetime = DateTime.Now.AddDays(1);

            _context.Requests.Update(request);
            await _context.SaveChangesAsync();
        }
        public async Task<ICollection<RequestEntity>> GetRequestsByGroupId(Guid groupId)
        {
            return await dbSet.AsNoTracking()
                                .OrderByDescending(r => r.CreateDatetime)
                                .Include(e => e.Group)
                                .Include(e => e.HostBird)
                                .Include(e => e.Host)
                                .Include(e => e.Place)
                                .Include(e => e.Room)
                                .Where(e => e.GroupId == groupId && e.IsDeleted == false)
                                .ToListAsync();
        }

        // public async Task UpdateRequest(RequestEntity entity)
        // {
        //     var result = await this.UpdateAsync(entity);

        //     if (result == 0)
        //     {
        //         throw new BadRequestException("Request isnt updated");
        //     }
        // }
    }
}
