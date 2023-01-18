using AutoMapper;
using EBird.Api.UserFeatures.Requests;
using EBird.Domain.Entities;

namespace EBird.Api.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<SignupRequest, AccountEntity>();
        
        
        }
    }
}
