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
        public Task<RoomResponseDTO> GetRoom(Guid roomId);
        public Task<List<RoomResponseDTO>> GetRooms();
        public Task AddRoom(RoomCreateDTO roomDTO);
        public Task UpdateRoom(Guid id, RoomUpdateDTO roomDTO);
        public Task DeleteRoom(Guid roomId);
    }
}
