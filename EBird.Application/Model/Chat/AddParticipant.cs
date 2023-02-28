namespace EBird.Application.Model.Chat
{
    public class AddParticipant
    {
        public Guid ReferenceId { get; set; }
        public List<Guid> AccountId { get; set; }
    }
}