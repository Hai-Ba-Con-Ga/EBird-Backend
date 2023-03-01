using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Application.Model.PagingModel;

namespace EBird.Application.Model.Request
{
    public class RequestParameters : QueryStringParameters
    {
        public Guid? RoomId { get; set; }
    }
}