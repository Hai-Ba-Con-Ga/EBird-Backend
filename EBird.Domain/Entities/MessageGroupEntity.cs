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
    [Table("MessageGroup")]
    public class MessageGroupEntity : BaseEntity
    {
        //PK
        [ForeignKey("MessageId")]
        public Guid MessageId { get; set; }
        public MessageEntity Message { get; set; } = null!;

        [ForeignKey("SentId")]
        public Guid SentId { get; set; }
        public GroupEntity Sent { get; set; } = null!;

        [ForeignKey("ReceiveId")]
        public Guid ReceiveId { get; set; }
        public GroupEntity Receive { get; set; } = null!;

    }
}
