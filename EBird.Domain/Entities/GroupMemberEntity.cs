using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using EBird.Domain.Common;

namespace EBird.Domain.Entities
{
    [Table("GroupMember")]
    public class GroupMemberEntity : BaseEntity
    {
        public DateTime JoinDate { get; set; }

        [ForeignKey("GroupId")]
        public Guid GroupId { get; set; }
        public GroupEntity Group { get; set; }

        [ForeignKey("UserId")]
        public Guid UserId { get; set; }
        public AccountEntity User { get; set; }
    }
}