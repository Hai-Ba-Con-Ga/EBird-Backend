using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Application.Model.Resource;
using EBird.Domain.Entities;

namespace EBird.Application.Interfaces.IRepository
{
    public interface IResourceRepository : IGenericRepository<ResourceEntity>
    {
        public Task<IEnumerable<ResourceEntity>> GetResources();
        public Task<ResourceEntity> GetResource(Guid id);
        public Task<bool> CreateResource(ResourceEntity entity);
        public Task<bool> UpdateResource(ResourceEntity entity);
        public Task<bool> DeleteResource(Guid id);
        public Task<ICollection<ResourceResponse>> GetResourcesByBird(Guid birdId);
        public Task<ICollection<ResourceResponse>> GetResourcesByAccount(Guid accountId);
    }
}