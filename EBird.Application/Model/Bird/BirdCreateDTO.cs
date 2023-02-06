using Duende.IdentityServer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Model.Bird
{
    public class BirdCreateDTO : BirdRequestDTO
    {
        [Required(ErrorMessage = "Onwer is required")]
        public Guid OwnerId { get; set; }
    }
}
