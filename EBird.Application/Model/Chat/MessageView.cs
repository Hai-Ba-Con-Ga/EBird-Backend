namespace EBird.Application.Model.Chat{
    public class MessageView {
        public string Content { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string FromUserId { get; set; }
        public string FromUserName { get; set; }
        public string FromFullName { get; set; }
        public string RoomId { get; set; }
    }
}