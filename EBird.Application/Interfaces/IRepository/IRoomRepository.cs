using EBird.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EBird.Application.Interfaces.IRepository
{
    public interface IRoomRepository : IGenericRepository<RoomEntity>
    {
        Task<List<RoomEntity>> GetRoomsActiveAsync();
        Task<RoomEntity> GetRoomActiveAsync(Guid roomId);
        Task<bool> AddRoomAsync(RoomEntity room);
        Task<bool> UpdateRoomAsync(RoomEntity room);
        Task<bool> SoftDeleteRoomAsync(Guid roomId);
    }
}
