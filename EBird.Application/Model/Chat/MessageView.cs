using System.ComponentModel.DataAnnotations;

namespace EBird.Application.Model.Chat{
    public class MessageView {
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        
        public Guid FromUserId { get; set; }
        public string FromUserName { get; set; }
        public string FromFullName { get; set; }
        public Guid RoomId { get; set; }
    }
}