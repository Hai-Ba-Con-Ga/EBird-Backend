using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using EBird.Application.Model.Place;

namespace EBird.Application.Model.Match
{
    public class MatchCreateDTO
    {
        // public Guid HostId { get; set; }
        // public Guid BirdHostId { get; set; }
        // public Guid? GroupId { get; set; }
        // public Guid RoomId { get; set; }
        // override public PlaceRequestDTO Place { get; set; }

        [Required]
        public Guid RequestId { get; set; }
        public Guid? UserId { get; set; }
    }
}