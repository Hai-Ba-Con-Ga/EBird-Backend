using EBird.Application.Model;
using EBird.Application.Model.PagingModel;
using EBird.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Interfaces.IRepository
{
    public interface IBirdRepository : IGenericRepository<BirdEntity>
    {
        Task<List<BirdEntity>> GetBirdsActiveAsync();
        Task<BirdEntity> GetBirdActiveAsync(Guid birdID);
        Task<BirdEntity> AddBirdAsync(BirdEntity bird);
        Task<BirdEntity> UpdateBirdAsync(BirdEntity bird);
        Task<BirdEntity> SoftDeleteBirdAsync(Guid birdID);
        Task<List<BirdEntity>> GetBirdsActiveAsync(BirdParameters birdParameters);
        Task<List<BirdEntity>> GetAllBirdActiveByAccountId(Guid accountId);
    }
}
