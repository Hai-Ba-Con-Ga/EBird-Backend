using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Application.Model.Place;

namespace EBird.Application.Services.IServices
{
    public interface IPlaceService
    {
        public Task<Guid> CreatePlace(PlaceRequestDTO request);
        public Task DeletepPlace(Guid placeId);
        public Task UpdatePlace(Guid placeId, PlaceRequestDTO request);
        public Task<ICollection<PlaceResponseDTO>> GetPlaces();
        public Task<PlaceResponseDTO> GetPlace(Guid id);
    }
}