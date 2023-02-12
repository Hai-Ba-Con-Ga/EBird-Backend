using EBird.Application.Interfaces.IMapper;
using EBird.Application.Model.Auth;
using EBird.Application.Model.Bird;
using EBird.Application.Model.Group;
using EBird.Application.Model.Place;
using EBird.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Model.Request
{
    public class RequestResponse : IMapFrom<RequestEntity>
    {
        public Guid Id { get; set; }
        public DateTime RequestDatetime { get; set; }
        public string Status { get; set; }
        public AccountResponse CreatedBy { get; set; }
        public BirdResponseDTO Bird { get; set; }
        public GroupResponseDTO? Group { get; set; }
        public PlaceResponseDTO Place { get; set; }
    }
}
