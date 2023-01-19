using EBird.Application.Model;
using EBird.Domain.Entities;

namespace EBird.Application.Services.IServices
{
    public interface IBirdTypeService
    {
        public Task<Response<BirdTypeEntity>> GetBirdType(string birdTypeCode);
        public Task<Response<BirdTypeEntity>> GetBirdType(Guid birdTypeID);

        public Task<Response<List<BirdTypeEntity>>> GetAllBirdType();

        public Task<Response<BirdTypeEntity>> InsertBirdType(BirdTypeEntity birdType);

        public Task<Response<BirdTypeEntity>> UpdateBirdType(BirdTypeEntity birdType);

        public Task<Response<BirdTypeEntity>> DeleteBirdType(Guid birdTypeID);

        public Task<Response<BirdTypeEntity>> DeleteBirdType(string birdTypeCode);

    }
}
