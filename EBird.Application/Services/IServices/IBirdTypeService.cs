using EBird.Application.Model;
using EBird.Domain.Entities;

namespace EBird.Application.Services.IServices
{
    public interface IBirdTypeService
    {
        public Task<Response<BirdTypeDTO>> GetBirdType(string birdTypeCode);
        public Task<Response<BirdTypeDTO>> GetBirdType(Guid birdTypeID);

        public Task<Response<List<BirdTypeDTO>>> GetAllBirdType();

        public Task<Response<BirdTypeDTO>> InsertBirdType(BirdTypeDTO birdTypeDTO);

        public Task<Response<BirdTypeDTO>> UpdateBirdType(Guid id, BirdTypeDTO birdTypeDTO);

        public Task<Response<BirdTypeDTO>> DeleteBirdType(Guid birdTypeID);

        public Task<Response<BirdTypeDTO>> DeleteBirdType(string birdTypeCode);

    }
}
