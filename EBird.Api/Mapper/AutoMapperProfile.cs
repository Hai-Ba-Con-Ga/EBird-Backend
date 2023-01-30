using AutoMapper;
using EBird.Application.Model;
using EBird.Domain.Entities;

namespace EBird.Api.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<BirdTypeEntity, BirdTypeDTO>().ReverseMap();
            CreateMap<BirdEntity, BirdDTO>().ReverseMap();
        }
    }
}
