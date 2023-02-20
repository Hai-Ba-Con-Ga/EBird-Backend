using AutoMapper;
using Duende.IdentityServer.Extensions;
using EBird.Application.Exceptions;
using EBird.Application.Interfaces;
using EBird.Application.Model;
using EBird.Application.Services.IServices;
using EBird.Application.Validation;
using EBird.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Services
{
    public class RoomService : IRoomService
    {
        private IWapperRepository _repository;
        private IMapper _mapper;

        public RoomService(IWapperRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Guid> AddRoom(Guid createById, RoomCreateDTO roomCreateDTO)
        {
            //await BaseValidation.ValidateAccountId(roomCreateDTO.CreateById, _repository);

            //var roomEntity = _mapper.Map<RoomEntity>(roomCreateDTO);
            var roomEntity = new RoomEntity()
            {
                Name = roomCreateDTO.Name,
                Status = roomCreateDTO.Status,
                City = roomCreateDTO.City,
                CreateById = createById
            };

            await _repository.Room.AddRoomAsync(roomEntity);

            return roomEntity.Id;
        }

        public async Task DeleteRoom(Guid roomId)
        {
            await _repository.Room.SoftDeleteRoomAsync(roomId);
        }

        public async Task<RoomResponseDTO> GetRoom(Guid roomId)
        {
            var roomDTO = await _repository.Room.GetRoomActiveAsync(roomId);
            if (roomDTO == null)
            {
                throw new NotFoundException("Can not found the room");
            }
            return _mapper.Map<RoomResponseDTO>(roomDTO);
        }

        public async Task<List<RoomResponseDTO>> GetRooms()
        {
            var roomDTOList = await _repository.Room.GetAllActiveAsync();
            if (roomDTOList.Count == 0)
            {
                throw new NotFoundException("Can not found rooms list");
            }
            return _mapper.Map<List<RoomResponseDTO>>(roomDTOList);
        }

        public async Task UpdateRoom(Guid id, RoomUpdateDTO roomUpdateDTO)
        {
            //await BaseValidation.ValidateAccountId(roomDTO.CreateById, _repository);
            var roomEntity = await _repository.Room.GetRoomActiveAsync(id);
            
            if (roomEntity == null)
            {
                throw new NotFoundException("Can not found room for updating");
            }

            _mapper.Map(roomUpdateDTO, roomEntity);

            await _repository.Room.UpdateRoomAsync(roomEntity);
        }
    }
}
