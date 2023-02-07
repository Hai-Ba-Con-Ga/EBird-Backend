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
    [Table("Message")]
    public class MessageEntity : BaseEntity
    {
        [Required]
        [Column(TypeName = "text")]
        public string Content { get; set; } = null!;

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime CreateDatetime { get; set; }

        //relationship
        public ICollection<MessageGroupEntity>? MessageGroups { get; set; }
        public ICollection<MessagePrivateEntity>? MessagePrivates { get; set; }
    }
}
