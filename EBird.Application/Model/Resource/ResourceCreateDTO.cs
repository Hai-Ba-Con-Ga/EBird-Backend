using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EBird.Application.Model.Resource
{
    public class ResourceCreateDTO : ResourceRequestDTO
    {
         
        [Required]
        public Guid CreateById { get; set; }
    }
}