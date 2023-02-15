using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EBird.Domain.Common;
using EBird.Domain.Enums;

namespace EBird.Domain.Entities;
public class ChatRoomEntity : BaseEntity
{
    [Column("Name", TypeName = "nvarchar")]
    [MaxLength(100)]
    public string Name { get; set; }
    [Column("Type", TypeName = "varchar")]
    [MaxLength(20)]
    public string TypeString
    {
        get { return TypeChatRoom.ToString(); }
        private set { TypeChatRoom = Enum.Parse<TypeChatRoom>(value); }
    }
    [NotMapped]
    public TypeChatRoom TypeChatRoom { get; set; } = TypeChatRoom.Group;
    public ICollection<ParticipantEntity> Participants { get; set; } = null!;
    public ICollection<MessageEntity> Messages { get; set; } = null!;
}