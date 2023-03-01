using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EBird.Application.Model.Request
{
    public class RequestCheckDTO
    {
        public Guid HostRequestID { get; set; }

        public Guid ChallengerRequestID { get; set; }

    }
}