namespace EBird.Application.Model.Chat;
public class CreateMessage
{
    public string Content { get; set; }
    public DateTime Timestamp { get; set; }

    public Guid FromUserId { get; set; }
    public Guid RoomId { get; set; }

}