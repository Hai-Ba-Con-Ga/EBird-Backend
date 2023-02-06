using EBird.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Domain.Entities
{
    [Table("Room")]
    public class RoomEntity : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        [Column("RoomName", TypeName = "nvarchar")]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("RoomStatus", TypeName = "varchar")]
        public string Status { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("RoomCity", TypeName = "nvarchar")]
        public string City { get; set; }

        [Required]
        [Column("RoomCreateDateTime", TypeName = "datetime")]
        public DateTime CreateDateTime { get; set; } = DateTime.Now;


        //forgein key with account table
        [ForeignKey("RoomCreateById")]
        public Guid CreatedById { get; set; }
        public AccountEntity CreatedBy { get; set; }
    }
}
