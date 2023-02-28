using System.ComponentModel.DataAnnotations;
using EBird.Domain.Enums;

namespace EBird.Application.Model.Chat;

    public class CreateChatRoomGroup
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "ReferenceId is required")]
        public Guid ReferenceId { get; set; }

        public TypeChatRoom TypeChatRoom { get; set; } = TypeChatRoom.Group;
    }
