using Duende.IdentityServer.Models;
using EBird.Application.Interfaces.IMapper;
using EBird.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Model
{
    public class RoomUpdateDTO : IMapTo<RoomEntity>
    {

        [Required(ErrorMessage = "Room name is required")]
        [StringLength(50, ErrorMessage = "Room name cannot be longer than 50 characters")]
        public string Name { get; set; }

        public string Status { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(50, ErrorMessage = "City cannot be longer than 50 characters")]
        public string City { get; set; }
    }
}
