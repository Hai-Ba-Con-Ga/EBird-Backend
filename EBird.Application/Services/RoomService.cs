using EBird.Application.Model;
using EBird.Application.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Services
{
    public class RoomService : IRoomService
    {
        public Task<RoomDTO> AddRoom(RoomDTO roomDTO)
        {
            throw new NotImplementedException();
        }

        public Task<RoomDTO> DeleteRoom(Guid roomId)
        {
            throw new NotImplementedException();
        }

        public Task<RoomDTO> GetRoom(Guid roomId)
        {
            throw new NotImplementedException();
        }

        public Task<List<RoomDTO>> GetRooms()
        {
            throw new NotImplementedException();
        }

        public Task<RoomDTO> UpdateRoom(Guid id, RoomDTO roomDTO)
        {
            throw new NotImplementedException();
        }
    }
}
