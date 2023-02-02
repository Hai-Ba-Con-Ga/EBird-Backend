using AutoMapper;
using EBird.Api.UserFeatures.Requests;
using EBird.Application.Model;
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
            CreateMap<BirdTypeEntity, BirdTypeDTO>().ReverseMap();
            CreateMap<BirdEntity, BirdDTO>().ReverseMap();
            CreateMap<GroupEntity, GroupDTO>().ReverseMap();
            CreateMap<GroupUpdateDTO, GroupEntity>();

        }
    }
}
