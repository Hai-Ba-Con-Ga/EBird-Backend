using EBird.Application.Model;
using EBird.Domain.Entities;

namespace EBird.Application.Services.IServices
{
    public interface IBirdTypeService
    {
        public Task<BirdTypeDTO> GetBirdType(string birdTypeCode);
        public Task<BirdTypeDTO> GetBirdType(Guid birdTypeID);

        public Task<List<BirdTypeDTO>> GetAllBirdType();

        public Task<BirdTypeDTO> AddBirdType(BirdTypeDTO birdTypeDTO);

        public Task<BirdTypeDTO> UpdateBirdType(Guid id, BirdTypeDTO birdTypeDTO);

        public Task<BirdTypeDTO> DeleteBirdType(Guid birdTypeID);

        public Task<BirdTypeDTO> DeleteBirdType(string birdTypeCode);

    }
}
