using EBird.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Domain.Entities
{
    [Table("Group")]
    public class GroupEntity : BaseEntity
    {
        [Column("GroupName", TypeName = "nvarchar")]
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [Column("GroupMaxELO")]
        [Required]
        public int MaxELO { get; set; }

        [Column("GroupMinELO")]
        [Required]
        public int MinELO { get; set; }

        [Column("GroupStatus", TypeName = "varchar")]
        [MaxLength(20)]
        public string Status { get; set; }

        [Column("GroupCreateDatetime")]
        [Required]
        public DateTime CreateDatetime { get; set; }

        //forgein key
        [ForeignKey("CreatedById")]
        public Guid CreatedById { get; set; }
        public AccountEntity CreatedBy;

        public ICollection<RequestEntity> Requests { get; set; }
        public ICollection<MatchEntity> Matches { get; set; }
        public ICollection<GroupMemberEntity> GroupMembers { get; set; }
    }
}
