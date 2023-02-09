using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EBird.Application.Exceptions;
using EBird.Application.Interfaces;
using EBird.Application.Model.Place;
using EBird.Application.Services.IServices;
using EBird.Domain.Entities;

namespace EBird.Application.Services
{
    public class PlaceService : IPlaceService
    {
        private IMapper _mapper;
        private IWapperRepository _repository;

        public PlaceService(IMapper mapper, IWapperRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task CreatePlace(PlaceRequestDTO request)
        {
            if(request == null)
            {
                throw new BadRequestException("request is null");
            }

            var placeEntity = _mapper.Map<PlaceEntity>(request);

            await _repository.Place.CreatePlace(placeEntity);

        }

        public async Task DeletepPlace(Guid placeId)
        {
            await _repository.Place.DeletePlace(placeId);
        }

        public async Task<PlaceResponseDTO> GetPlace(Guid id)
        {
            var placeEntity = await _repository.Place.GetPlace(id);
            return _mapper.Map<PlaceResponseDTO>(placeEntity);
        }

        public async Task<ICollection<PlaceResponseDTO>> GetPlaces()
        {
            var result = await _repository.Place.GetPlaces();
            return _mapper.Map<ICollection<PlaceResponseDTO>>(result);
        }

        public async Task UpdatePlace(Guid placeId, PlaceRequestDTO request)
        {
            var entity = await _repository.Place.GetPlace(placeId);
            _repository.Place.UpdatePlace(entity);
        }
    }
}