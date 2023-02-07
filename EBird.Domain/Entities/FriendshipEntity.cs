using EBird.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EBird.Domain.Entities
{
    [Table("Friendship")]
    public class FriendshipEntity : BaseEntity
    {
        [Required]
        public DateTime CreateDateTime { get; set; }

        [MaxLength(50)]
        public string? Status { get; set; }

        //PK
        [ForeignKey("InviterId")]
        public Guid InviterId { get; set; }
        public AccountEntity Inviter { get; set; } = null!;

        //PK
        [ForeignKey("RecieverId")]
        public Guid RecieverId { get; set; }
        public AccountEntity Reciever { get; set; } = null!;
    }
}
