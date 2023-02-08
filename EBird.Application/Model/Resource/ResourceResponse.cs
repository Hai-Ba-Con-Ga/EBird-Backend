using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Application.Interfaces.IMapper;
using EBird.Domain.Entities;

namespace EBird.Application.Model.Resource
{
    public class ResourceResponse : IMapFrom<ResourceEntity>
    {
        public Guid Id { get; set; }
        public string DataLink { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid CreateById { get; set; }
    }
}