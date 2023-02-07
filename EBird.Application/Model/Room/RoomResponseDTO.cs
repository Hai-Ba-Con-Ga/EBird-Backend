using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using EBird.Application.Interfaces.IMapper;
using EBird.Domain.Entities;

namespace EBird.Application.Model
{
    public class RoomResponseDTO : IMapFrom<RoomEntity>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string City { get; set; }
        public DateTime CreateDateTime { get; set; }
        public Guid CreateById { get; set; }
    }
}
