using EBird.Application.Interfaces.IMapper;

namespace EBird.Application.Model.Chat
{
    public class UserView
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public Guid CurrentRoomId { get; set; }

    }
}