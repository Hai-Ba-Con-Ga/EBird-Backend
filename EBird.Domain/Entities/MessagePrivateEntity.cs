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
    [Table("MessagePrivate")]
    public class MessagePrivateEntity : BaseEntity
    {
        // PK
        [ForeignKey("MessageId")]
        public Guid MessageId { get; set; }
        public MessageEntity Message { get; set; } = null!;

        [ForeignKey("SentId")]
        public Guid SentId { get; set; }
        public AccountEntity Sent { get; set; } = null!;

        [ForeignKey("ReceiveId")]
        public Guid ReceiveId { get; set; }
        public AccountEntity Receive { get; set; } = null!;

        //relationship
        public ICollection<MatchMessageEntity>? MatchMessages { get; set; }
    }
}
