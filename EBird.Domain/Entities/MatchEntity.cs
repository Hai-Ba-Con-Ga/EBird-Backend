using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using EBird.Domain.Common;
using EBird.Domain.Enums;

namespace EBird.Domain.Entities
{
    public class MatchEntity : BaseEntity
    {
        [Column("MatchDateTime", TypeName = "datetime")]
        [Required]
        public DateTime MatchDatetime { get; set; }

        [Column("CreateDateTime", TypeName = "datetime")]
        [Required]
        public DateTime CreateDatetime { get; set; }

        [Column("ExpDateTime", TypeName = "datetime")]
        [Required]
        public DateTime ExpDatetime { get; set; }

        [Column("MatchStatus")]
        [EnumDataType(typeof(MatchStatus))]
        [Required]
        public MatchStatus MatchStatus { get; set; }

        [Column("HostId")]
        [Required]
        public Guid HostId { get; set; }
        public AccountEntity Host { get; set; }

        [Column("ChallengerId")]
        public Guid? ChallengerId { get; set; }
        public AccountEntity? Challenger { get; set; }

        [Column("PlaceId")]
        [Required]
        public Guid PlaceId { get; set; }
        public PlaceEntity Place { get; set; }

        [Column("GroupId")]
        public Guid? GroupId { get; set; }
        public GroupEntity? Group { get; set; }

        [Column("RoomId")]
        [Required]
        public Guid RoomId { get; set; }
        public RoomEntity Room { get; set; }

        public ICollection<MatchBirdEntity> MatchBirds { get; set; }
    }
}