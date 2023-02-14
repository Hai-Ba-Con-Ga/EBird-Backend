using EBird.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Domain.Entities
{
    public class RequestEntity : BaseEntity
    {
        [Column("RequestDatetime", TypeName = "datetime")]
        [Required]
        public DateTime RequestDatetime { get; set; }

        [Column("CreateDatetime", TypeName = "datetime")]
        [Required]
        public DateTime CreateDatetime { get; set; }

        [Column("Status", TypeName = "nvarchar")]
        [StringLength(50)]
        [Required]
        public string Status { get; set; }
        
        //forgein key

        [ForeignKey("CreatedById")]
        public Guid CreatedById { get; set; }
        public AccountEntity CreatedBy { get; set; }

        [ForeignKey("BirdId")]
        public Guid BirdId { get; set; }
        public BirdEntity Bird { get; set; }

        [ForeignKey("GroupId")]
        public Guid? GroupId { get; set; }
        public GroupEntity? Group { get; set; }
        
        [ForeignKey("PlaceId")]
        public Guid PlaceId { get; set; }
        public PlaceEntity Place { get; set; }

        [ForeignKey("RoomId")]
        public Guid RoomId { get; set; }
        public RoomEntity Room { get; set; }
    }
}
