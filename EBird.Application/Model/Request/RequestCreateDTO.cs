using EBird.Application.Interfaces.IMapper;
using EBird.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBird.Application.Model.Place;

namespace EBird.Application.Model.Request
{
    public class RequestCreateDTO : IMapTo<RequestEntity>
    {
        [Required]
        public DateTime RequestDatetime { get; set; }
        [Required]
        public Guid HostBirdId { get; set; }
        [Required]
        public Guid PlaceId { get; set; }
        public Guid? HostId { get; set; }
        public Guid? GroupId { get; set; }
        public Guid RoomId { get; set; }
        public PlaceRequestDTO Place { get; set; }
  
    }
}
