using EBird.Domain.Common;

namespace EBird.Domain.Entities;
public class ChatRoomEntity : BaseEntity
{
    public string Name { get; set; }
    public ICollection<ParticipantEntity> Participants { get; set; } = null!;
    public ICollection<MessageEntity> Messages { get; set; } = null!;
}