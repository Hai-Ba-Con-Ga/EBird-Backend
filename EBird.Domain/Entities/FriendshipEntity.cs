using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using EBird.Domain.Common;

namespace EBird.Domain.Entities
{
    [Table("Friendship")]
    public class FriendshipEntity : BaseEntity
    {
        [Column("InviterID")]
        [Required]
        public Guid InviterId { get; set; }
        public AccountEntity Inviter { get; set; }

        [Column("ReceiverID")]
        [Required]
        public Guid ReceiverId { get; set; }
        public AccountEntity Receiver { get; set; }

        [Column("AcceptanceDatetime", TypeName = "datetime")]
        public DateTime? AcceptanceDatetime { get; set; }

        [Column("IsAccept")]
        [Required]
        public bool IsAccept { get; set; } = false;

        [Column("CreateDatetime", TypeName = "datetime")]
        [Required]
        public DateTime CreateDatetime { get; set;} = DateTime.Now;

        [Column("Status")]
        public string? Status { get; set; }
    }
}