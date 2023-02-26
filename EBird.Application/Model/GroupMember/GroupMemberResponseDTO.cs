using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Application.Interfaces.IMapper;
using EBird.Application.Model.Group;
using EBird.Domain.Entities;

namespace EBird.Application.Model.GroupMember
{
    public class GroupMemberResponseDTO : IMapFrom<GroupMemberEntity>
    {
        public DateTime JoinDate { get; set; }
        
        public GroupResponseDTO Group { get; set; }
    }
}