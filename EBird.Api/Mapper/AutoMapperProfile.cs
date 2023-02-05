using AutoMapper;
using EBird.Api.UserFeatures.Requests;
using EBird.Application.Model;
using EBird.Application.Model.Bird;
using EBird.Application.Model.BirdType;
using EBird.Application.Model.Group;
using EBird.Application.Model.PagingModel;
using EBird.Domain.Entities;

namespace EBird.Api.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<SignupRequest, AccountEntity>();
            CreateMap<UpdateAccountRequest, AccountEntity>();
            CreateMap<AccountEntity, AccountResponse>();
            //Bird type
            CreateMap<BirdTypeEntity, BirdTypeResponseDTO>();
            CreateMap<BirdTypeRequestDTO, BirdTypeEntity>();
            //bird
            CreateMap<BirdRequestDTO, BirdEntity>();
            CreateMap<BirdCreateDTO, BirdEntity>();
            CreateMap<BirdEntity, BirdResponseDTO>();
            //group
            CreateMap<GroupEntity, GroupResponseDTO>().ReverseMap();
            CreateMap<GroupRequestDTO, GroupEntity>();
            CreateMap<GroupCreateDTO, GroupEntity>();
        }
    }
}
