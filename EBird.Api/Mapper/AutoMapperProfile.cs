using AutoMapper;
using EBird.Api.UserFeatures.Requests;
using EBird.Application.Model;
using EBird.Application.Model.Bird;
using EBird.Application.Model.BirdType;
using EBird.Application.Model.Group;
using EBird.Application.Model.Notification;
using EBird.Application.Model.NotificationType;
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
            CreateMap<NotificationEntity, NotificationDTO>().ReverseMap();
            CreateMap<NotificationEntity, NotificationUpdateDTO>().ReverseMap();
            CreateMap<NotificationEntity, NotificationCreateDTO>().ReverseMap();

            CreateMap<NotificationTypeEntity, NotificationTypeRequestDTO>().ReverseMap();
            CreateMap<NotificationTypeEntity, NotificationTypeDTO>().ReverseMap();
        }
    }
}
