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
    public class GroupRepository : GenericRepository<GroupEntity>, IGroupRepository
    {
        public GroupRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> AddGroupAsync(GroupEntity group)
        {
            group.CreateDatetime = DateTime.Now;
            
            int rowEffect =  await this.CreateAsync(group);
            
            if(rowEffect == 0)
            {
                throw new BadRequestException("Can not add group");
            }
            return true;
        }

        public async Task<GroupEntity> GetGroupActiveAsync(Guid groupID)
        {
            return await this.GetByIdActiveAsync(groupID);
        }

        public async Task<List<GroupEntity>> GetGroupsActiveAsync()
        {
            return await this.GetAllActiveAsync();
        }

        public async Task<bool> SoftDeleteGroupAsync(Guid groupID)
        {
            var group = await this.GetByIdActiveAsync(groupID);

            if(group == null)
            {
                throw new NotFoundException("Can not found group for delete");
            }
            
             await this.DeleteSoftAsync(groupID);
            return true;
        }

        public async Task<bool> UpdateGroupAsync(GroupEntity group)
        {
            var result = await this.UpdateAsync(group);
            return result != 0 ? true : throw new BadRequestException("Can not update group");
        }
    }
}
