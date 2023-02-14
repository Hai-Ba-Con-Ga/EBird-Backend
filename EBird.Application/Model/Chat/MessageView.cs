using System.ComponentModel.DataAnnotations;
using EBird.Application.Interfaces.IMapper;
using EBird.Domain.Entities;

namespace EBird.Application.Model.Chat{
    public class MessageView 
    {
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        
        public Guid FromUserId { get; set; }
        public string FromFullName { get; set; }
        public Guid RoomId { get; set; }
    }
}