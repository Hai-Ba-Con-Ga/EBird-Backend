using Duende.IdentityServer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Model
{
    public class RoomCreateDTO : RoomUpdateDTO
    {
        [Required(ErrorMessage = "CreateById is required")]
        public Guid CreateById { get; set; }
    }
}
