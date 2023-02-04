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

        public async Task<RoomCreateDTO> AddRoom(RoomCreateDTO roomCreateDTO)
        {
            await BaseValidation.ValidateAccountId(roomCreateDTO.CreateById, _repository);
            var roomEntity = _mapper.Map<RoomEntity>(roomCreateDTO);
            var res = await _repository.Room.AddRoomAsync(roomEntity);

            if (res == null)
            {
                throw new BadRequestException("Room is not added");
            }

            return _mapper.Map<RoomCreateDTO>(res);
        }

        public async Task<RoomDTO> DeleteRoom(Guid roomId)
        {
            var res = await _repository.Room.SoftDeleteRoomAsync(roomId);
            if (res == null)
            {
                throw new NotFoundException("Can not found room for delete");
            }
            return _mapper.Map<RoomDTO>(res);
        }

        public async Task<RoomDTO> GetRoom(Guid roomId)
        {
            var roomDTO = await _repository.Room.GetRoomActiveAsync(roomId);
            if (roomDTO == null)
            {
                throw new NotFoundException("Can not found the room");
            }
            return _mapper.Map<RoomDTO>(roomDTO);
        }

        public async Task<List<RoomDTO>> GetRooms()
        {
            var roomDTOList = await _repository.Room.GetAllActiveAsync();
            if (roomDTOList.Count == 0)
            {
                throw new NotFoundException("Can not found rooms list");
            }
            return _mapper.Map<List<RoomDTO>>(roomDTOList);
        }

        public async Task<RoomUpdateDTO> UpdateRoom(Guid id, RoomUpdateDTO roomUpdateDTO)
        {
            //await BaseValidation.ValidateAccountId(roomDTO.CreateById, _repository);
            var roomEntity = await _repository.Room.GetRoomActiveAsync(id);
            if (roomEntity == null)
            {
                throw new NotFoundException("Can not found room for updating");
            }

            _mapper.Map(roomUpdateDTO, roomEntity);

            var res = await _repository.Room.UpdateRoomAsync(roomEntity);

            if (res == null)
            {
                throw new BadRequestException("Update Fail");
            }
            return roomUpdateDTO;
        }
    }
}
