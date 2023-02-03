using EBird.Application.Interfaces.IRepository;
using EBird.Domain.Entities;
using EBird.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Infrastructure.Repositories
{
    public class RoomRepository : GenericRepository<RoomEntity>, IRoomRepository
    {
        public RoomRepository(ApplicationDbContext context) : base(context)
        {
            
        }

        public async Task<RoomEntity> AddRoomAsync(RoomEntity room)
        {
            room.CreateDateTime = DateTime.Now;
            var res = await this.CreateAsync(room);
            if (res > 0)
            {
                return room;
            }
            return null;
        }

        public async Task<RoomEntity> GetRoomActiveAsync(Guid roomId)
        {
            return await this.GetByIdActiveAsync(roomId);
        }

        public async Task<List<RoomEntity>> GetRoomsActiveAsync()
        {
            return await this.GetAllActiveAsync();
        }

        public async Task<RoomEntity> SoftDeleteRoomAsync(Guid roomId)
        {
            return await this.DeleteSoftAsync(roomId);
        }

        public async Task<int> UpdateRoomAsync(RoomEntity room)
        {
            var res = await this.UpdateAsync(room);
            return res;
        }
    }
}
