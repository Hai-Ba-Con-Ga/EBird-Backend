using EBird.Domain.Common;
using EBird.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EBird.Domain.Entities
{
    public class RequestEntity : BaseEntity
    {

        [Column("Number", TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Number { get; set; }

        [Column("RequestDatetime", TypeName = "datetime")]
        [Required]
        public DateTime RequestDatetime { get; set; }

        [Column("CreateDatetime", TypeName = "datetime")]
        [Required]
        public DateTime CreateDatetime { get; set; } = DateTime.Now;

        [Column("ExpDatetime", TypeName = "datetime")]
        [Required]
        public DateTime ExpDatetime { get; set; } = DateTime.Now.AddDays(1);

        [Column("Status")]
        [MaxLength(50)]
        [Required]
        public RequestStatus Status { get; set; } = RequestStatus.Waiting;

        //forgein key

        [ForeignKey("HostId")]
        public Guid HostId { get; set; }
        public AccountEntity Host { get; set; }

        [ForeignKey("HostBirdId")]
        public Guid HostBirdId { get; set; }
        public BirdEntity HostBird { get; set; }

        [ForeignKey("ChallengerId")]
        public Guid? ChallengerId { get; set; }
        public AccountEntity? Challenger { get; set; }

        [ForeignKey("ChallengerBirdId")]
        public Guid? ChallengerBirdId { get; set; }
        public BirdEntity? ChallengerBird { get; set; }

        [ForeignKey("GroupId")]
        public Guid? GroupId { get; set; }
        public GroupEntity? Group { get; set; }

        [ForeignKey("PlaceId")]
        public Guid PlaceId { get; set; }
        public PlaceEntity Place { get; set; }

        [ForeignKey("RoomId")]
        [Required]
        public Guid RoomId { get; set; }
        public RoomEntity Room { get; set; }
    }
}
