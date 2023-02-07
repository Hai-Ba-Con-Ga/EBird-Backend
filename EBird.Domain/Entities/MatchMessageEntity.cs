using EBird.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EBird.Domain.Entities
{
    [Table("Match_Message")]
    public class MatchMessageEntity : BaseEntity
    {
        //PK
        [ForeignKey("MatchId")]
        public Guid MatchId { get; set; }
        public MatchEntity Match { get; set; } = null!;

        //PK
        [ForeignKey("MessageId")]
        public Guid MessageId { get; set; }
        public MessageEntity Message { get; set; } = null!;
    }
}
