using EBird.Application.Interfaces.IMapper;
using EBird.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EBird.Application.Model.Notification
{
    public class NotificationUpdateDTO : IMapTo<NotificationEntity>
    {

        public string Content { get; set; }
    }
}