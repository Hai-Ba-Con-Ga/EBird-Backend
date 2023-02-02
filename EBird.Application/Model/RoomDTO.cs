using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Model
{
    public class RoomDTO
    {
        [Required(ErrorMessage = "Room name is required")]
        [StringLength(50, ErrorMessage = "Room name cannot be longer than 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Maximum Elo is required")]
        public int MaximumELO { get; set; }

        [Required(ErrorMessage = "Minimum Elo is required")]
        public int MinimumELO { get; set; }

        //[Required(ErrorMessage = "Status is required")]
        //[StringLength(50, ErrorMessage = "Status cannot be longer than 50 characters")]
        public string Status { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(20, ErrorMessage = "City cannot be longer than 50 characters")]
        public string City { get; set; }

        [Required(ErrorMessage = " is required")]
        public DateTime CreateDateTime { get; set; }
    }
}
