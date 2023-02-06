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
        //guid id
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Room name is required")]
        [StringLength(50, ErrorMessage = "Room name cannot be longer than 50 characters")]
        public string Name { get; set; }

        public string Status { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(50, ErrorMessage = "City cannot be longer than 50 characters")]
        public string City { get; set; }

        [Required(ErrorMessage = "Create Time is required")]
        public DateTime CreateDateTime { get; set; }

        [Required(ErrorMessage = "CreateById is required")]
        public Guid CreateById { get; set; }
    }
}
