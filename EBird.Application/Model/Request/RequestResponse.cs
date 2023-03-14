using AutoMapper;
using EBird.Application.Interfaces.IMapper;
using EBird.Application.Model.Auth;
using EBird.Application.Model.Bird;
using EBird.Application.Model.Group;
using EBird.Application.Model.Place;
using EBird.Domain.Entities;
using EBird.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Model.Request
{
    public class RequestResponseDTO : IMapFrom<RequestEntity>
    {
        public int Number { get; set; }
        public Guid Id { get; set; }
        public DateTime RequestDatetime { get; set; }
        public DateTime ExpDatetime { get; set; }
        public RequestStatus Status { get; set; }
        public AccountResponse Host { get; set; }
        public BirdResponseDTO HostBird { get; set; }
        public AccountResponse Challenger { get; set; }
        public BirdResponseDTO ChallengerBird { get; set; }
        public GroupResponseDTO? Group { get; set; }
        public PlaceResponseDTO Place { get; set; }
        public RoomResponseDTO Room { get; set; }
        public string? Reference { get; set; }
        public bool? IsReady { get; set; }

        // public void MappingFrom(Profile profile)
        // {
        //     profile.CreateMap<RequestEntity, RequestResponse>()
        //         .ForMember(x => x.Status, opt => opt.MapFrom(x => x.Status.GetDescription()));
        // }
        public DateTime CreateDatetime { get; set; }
    }
}
