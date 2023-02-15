using System.ComponentModel.DataAnnotations.Schema;
using EBird.Domain.Common;

namespace EBird.Domain.Entities;
public class MessageEntity : BaseEntity
{
    [Column("Content", TypeName ="text")]
    public string Content { get; set; }
    public Guid ChatRoomId { get; set; }
    public ChatRoomEntity ChatRoom { get; set; } = null!;
    public Guid SenderId { get; set; }
    public AccountEntity Sender { get; set; } = null!;
    [Column("Timestamp", TypeName ="datetime")]
    public DateTime Timestamp { get; set; }

}