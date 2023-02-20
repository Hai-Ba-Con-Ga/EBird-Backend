using EBird.Application.Model.Bird;
using EBird.Application.Model.PagingModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Services.IServices
{
    public interface IBirdService
    { 
        public Task<BirdResponseDTO> GetBird(Guid birdID);
        public Task<List<BirdResponseDTO>> GetBirds();
        public Task<Guid> AddBird(BirdCreateDTO birdDTO);
        public Task UpdateBird(Guid id, BirdRequestDTO birdDTO);
        public Task DeleteBird(Guid birdID);
        public Task<PagedList<BirdResponseDTO>> GetBirdsByPagingParameters(BirdParameters parameters);
        public Task<List<BirdResponseDTO>> GetAllBirdByAccount(Guid accountId);

    }
}
