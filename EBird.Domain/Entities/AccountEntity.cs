using EBird.Domain.Common;
using EBird.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EBird.Domain.Entities
{
    [Table("Account")]
    public class AccountEntity : BaseEntity
    {
        [Column(TypeName = "varchar")]
        [MaxLength(255)]
        public string? Password { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(50)]
        public string Email { get; set; } = null!;

        [Column(TypeName = "datetime")]
        public DateTime CreateDateTime { get; set; } = DateTime.Now;

        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string FirstName { get; set; } = null!;

        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string LastName { get; set; } = null!;

        [Column("Role", TypeName = "varchar")]
        [MaxLength(20)]
        public string RoleString
        {
            get { return Role.ToString(); }
            private set { Role = Enum.Parse<RoleAccount>(value); }
        }
        [NotMapped]
        public RoleAccount Role { get; set; } = RoleAccount.User;

        [Column(TypeName = "varchar")]
        [MaxLength(50)]
        public string? Username { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(200)]
        public string? Description { get; set; }
        public ICollection<RefreshTokenEntity> RefreshTokens { get; set; } = null!;
        public ICollection<GroupEntity> Groups { get; set; } = null!;
        public ICollection<RoomEntity> Rooms { get; set; } = null!;
        public ICollection<BirdEntity> Birds { get; set; } = null!;
        public ICollection<RuleEntity> Rules { get; set; } = null!;
        public ICollection<ResourceEntity> Resources { get; set; }
        public ICollection<AccountResourceEntity> AccountResources { get; set; }

        public ICollection<ParticipantEntity> Participants { get; set; } = null!;
        public ICollection<MessageEntity> Messages { get; set; } = null!;
        public ICollection<RequestEntity> HostRequests { get; set; }
        public ICollection<RequestEntity> ChallengerRequests { get; set; }
        public ICollection<NotificationEntity> Notifications { get; set; } = null!;
        public ICollection<ReportEntity> ReportCreates { get; set; } = null!;
        public ICollection<ReportEntity> ReportHandles { get; set; } = null!;

        public ICollection<MatchEntity> MatchesWithHost { get; set; }
        public ICollection<MatchEntity> MatchesWithChallenger { get; set; }

    }
}
