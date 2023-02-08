using EBird.Application.Model.BirdType;
using EBird.Domain.Entities;

namespace EBird.Application.Services.IServices
{
    public interface IBirdTypeService
    {
        public Task<BirdTypeResponseDTO> GetBirdType(string birdTypeCode);
        public Task<BirdTypeResponseDTO> GetBirdType(Guid birdTypeID);
        public Task<List<BirdTypeResponseDTO>> GetAllBirdType();
        public Task AddBirdType(BirdTypeRequestDTO birdTypeDTO);
        public Task UpdateBirdType(Guid id, BirdTypeRequestDTO birdTypeDTO);
        public Task DeleteBirdType(Guid birdTypeID);
        public Task DeleteBirdType(string birdTypeCode);

    }
}
