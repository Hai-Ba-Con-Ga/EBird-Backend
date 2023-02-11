namespace EBird.Application.Model.Chat
{
    public class AddParticipant
    {
        public Guid ChatRoomId { get; set; }
        public List<Guid> AccountId { get; set; }
    }
}