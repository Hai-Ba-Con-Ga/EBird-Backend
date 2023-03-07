using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using EBird.Application.Interfaces.IMapper;
using EBird.Domain.Entities;

namespace EBird.Application.Model.Resource
{
    public class ResourceRequestDTO : IMapTo<ResourceEntity>
    {
        [MaxLength(255)]
        [Required]
        public string DataLink { get; set; }

        [MaxLength(100)]
        public string Description { get; set; }

        [MaxLength(255)]
        public string Thumnail { get; set; }
    }
}