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
        [Column("Name", TypeName = "nvarchar")]
        public string Name { get; set; }

        [Required]
        [Column("MaximumELO", TypeName = "int")]
        public int MaximumELO { get; set; }

        [Required]
        [Column("MinimumELO", TypeName = "int")]
        public int MinimumELO { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("Status", TypeName = "varchar")]
        public string Status { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("City", TypeName = "nvarchar")]
        public string City { get; set; }

        [Required]
        [Column("CreateDateTime", TypeName = "datetime")]
        public DateTime CreateDateTime { get; set; }

        
        //[ForeignKey("CreateBy")]
        //public int CreateById { get; set; }
        //public virtual User CreateBy { get; set; }
    }
}
