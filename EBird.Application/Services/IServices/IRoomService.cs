using EBird.Application.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Services.IServices
{
    public interface IRoomService
    {
        public Task<RoomDTO> GetRoom(Guid roomId);
        public Task<List<RoomDTO>> GetRooms();
        public Task<RoomCreateDTO> AddRoom(RoomCreateDTO roomDTO);
        public Task<RoomUpdateDTO> UpdateRoom(Guid id, RoomUpdateDTO roomDTO);
        public Task<RoomDTO> DeleteRoom(Guid roomId);
    }
}
