using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Application.Exceptions;
using EBird.Application.Interfaces.IRepository;
using EBird.Application.Model.Resource;
using EBird.Domain.Entities;
using EBird.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EBird.Infrastructure.Repositories
{
    public class ResourceRepository : GenericRepository<ResourceEntity>, IResourceRepository
    {
        public ResourceRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> CreateResource(ResourceEntity entity)
        {
            entity.CreateDate = DateTime.Now;
            var rowEffect = await this.CreateAsync(entity);
            if (rowEffect == 0)
            {
                throw new BadRequestException("Can not create resource");
            }
            return true;
        }

        public async Task<bool> DeleteResource(Guid id)
        {
            var entity = await this.GetResource(id);
            entity.IsDeleted = true;
            if (await this.UpdateResource(entity))
            {
                throw new BadRequestException("Can not delete resource");
            }
            return true;
        }

        public async Task<ResourceEntity> GetResource(Guid id)
        {
            var entity = await this.GetByIdAsync(id);
            if (entity == null) throw new NotFoundException("Can not find resource");
            return entity;
        }

        public async Task<IEnumerable<ResourceEntity>> GetResources()
        {
            return await this.GetAllAsync();
        }

        public async Task<bool> UpdateResource(ResourceEntity entity)
        {
            var rowEffect = await this.UpdateAsync(entity);
            if (rowEffect == 0) throw new BadRequestException("Can not update resource");
            return true;
        }

        public async Task<ICollection<ResourceResponse>> GetResourcesByBird(Guid birdId)
        {
            var resultQeury = from br in _context.BirdResources
                            join r in _context.Resources on br.ResourceId equals r.Id
                            where br.BirdId == birdId
                            select new ResourceResponse
                            {
                                Id = r.Id,
                                DataLink = r.Datalink,
                                Description = r.Description,
                                CreateDate = r.CreateDate,
                                CreateById = r.CreateById
                            };
            return await resultQeury.ToListAsync();
        } 
    }
}