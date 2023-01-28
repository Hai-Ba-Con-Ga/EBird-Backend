using EBird.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Interfaces.IRepository
{
    public interface IBirdTypeRepository : IGenericRepository<BirdTypeEntity>
    {
        Task<BirdTypeEntity> SoftDeleteAsync(string birdTypeCode);
        Task<BirdTypeEntity> GetBirdTypeByCodeAsync(string birdTypeCode);
        Task<bool> IsExistBirdTypeCode(string birdTypeCode);
        Task<List<BirdTypeEntity>> GetAllBirdTypeActiveAsync();
        Task<BirdTypeEntity> GetBirdTypeActiveAsync(Guid id);
        Task<BirdTypeEntity> AddBirdTypeAsync(BirdTypeEntity birdType);

    }
}
