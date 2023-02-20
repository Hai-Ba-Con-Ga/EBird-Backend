using EBird.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EBird.Application.Interfaces.IRepository
{
    public interface IGroupRepository : IGenericRepository<GroupEntity>
    {
        Task<List<GroupEntity>> GetGroupsActiveAsync();
        Task<GroupEntity> GetGroupActiveAsync(Guid groupID);
        Task<bool> AddGroupAsync(GroupEntity group);
        Task<bool> UpdateGroupAsync(GroupEntity group);
        Task<bool> SoftDeleteGroupAsync(Guid groupID);

        //Task<List<GroupEntity>> GetGroupsByAccountAsync(Guid accountID);
    }
}
