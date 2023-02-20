using Duende.IdentityServer.Models;
using EBird.Application.Model.Resource;
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
        public Guid? OwnerId { get; set; }
        public List<ResourceCreateDTO>? ListResource {get; set; }
    }
}
