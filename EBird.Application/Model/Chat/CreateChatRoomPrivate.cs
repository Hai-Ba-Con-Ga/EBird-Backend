using System.ComponentModel.DataAnnotations;
using EBird.Domain.Enums;

namespace EBird.Application.Model.Chat
{
    public class CreateChatRoomPrivate
    {
        public Guid ReceiverId { get; set; }
        public TypeChatRoom TypeChatRoom { get; set; } = TypeChatRoom.Request;
    }
}