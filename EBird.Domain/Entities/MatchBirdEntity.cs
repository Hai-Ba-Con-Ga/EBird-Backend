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
    public class MatchBirdEntity : BaseEntity
    {
        [Column("BirdId")]
        [Required]
        public Guid BirdId { get; set; }
        public BirdEntity Bird { get; set; }

        [Column("MatchId")]
        [Required]
        public Guid MatchId { get; set; }
        public MatchEntity Match { get; set; }

        [Column("Result")]
        [EnumDataType(typeof(MatchBirdResult))]
        public MatchBirdResult? Result { get; set; }

        [Column("AfterElo")]
        public int? AfterElo { get; set; }

        [Column("BeforeElo")]
        [Required]
        public int BeforeElo { get; set; }

        [Column("UpdateDatetime", TypeName = "datetime")]
        [Required]
        public DateTime UpdateDatetime { get; set; }

        public ICollection<MatchResourceEntity> MatchResources { get; set; } 
    }
}