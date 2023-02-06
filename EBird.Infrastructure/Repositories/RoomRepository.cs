using EBird.Application.Exceptions;
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

        public async Task<bool> AddRoomAsync(RoomEntity room)
        {
            room.CreateDateTime = DateTime.Now;
            var rowEffect = await this.CreateAsync(room);
            if (rowEffect == 0)
            {
                throw new BadRequestException("Can not add room");
            }
            return true;
        }

        public async Task<RoomEntity> GetRoomActiveAsync(Guid roomId)
        {
            return await this.GetByIdActiveAsync(roomId);
        }

        public async Task<List<RoomEntity>> GetRoomsActiveAsync()
        {
            return await this.GetAllActiveAsync();
        }

        public async Task<bool> SoftDeleteRoomAsync(Guid roomId)
        {
            var result = await this.DeleteSoftAsync(roomId);

            if(result == null) throw new BadRequestException("Can not delete room");
            
            return true;
        }

        public async Task<bool> UpdateRoomAsync(RoomEntity room)
        {
            var rowEffect = await this.UpdateAsync(room);

            if(rowEffect == 0) throw new BadRequestException("Can not update room");
            
            return true;
        }
    }
}
