using EBird.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EBird.Domain.Entities
{
    [Table("GroupMember")]
    public class GroupMemberEntity : BaseEntity
    {
        [Required]
        public DateTime JoinDateTime { get; set; }

        [Required]
        [MaxLength(50)]
        public string Role { get; set; } = null!;

        //PK
        [ForeignKey("GroupId")]
        public Guid GroupId { get; set; }
        public GroupEntity Group { get; set; } = null!;

        //PK
        [ForeignKey("AccountId")]
        public Guid AccountId { get; set; }
        public AccountEntity Account { get; set; } = null!;
    }
}
