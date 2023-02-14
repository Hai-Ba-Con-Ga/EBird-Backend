using EBird.Domain.Common;

namespace EBird.Domain.Entities;
public class MessageEntity : BaseEntity
{
    public string Content { get; set; }
    public Guid ChatRoomId { get; set; }
    public ChatRoomEntity ChatRoom { get; set; } = null!;
    public Guid SenderId { get; set; }
    public AccountEntity Sender { get; set; } = null!;
    public DateTime Timestamp { get; set; }

}