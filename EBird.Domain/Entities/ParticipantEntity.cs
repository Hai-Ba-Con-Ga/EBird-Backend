using EBird.Domain.Common;

namespace EBird.Domain.Entities;
public class ParticipantEntity : BaseEntity{
    public Guid AccountId { get; set; }
    public AccountEntity Account { get; set; } = null!;
    public Guid ChatRoomId { get; set; }
    public ChatRoomEntity ChatRoom { get; set; } = null!;
}