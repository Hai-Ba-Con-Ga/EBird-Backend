using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EBird.Application.Model.Request
{
    public class RequestMergeDTO
    {
        public Guid hostRequestId { get; set; }
        public Guid challengerRequestId { get; set; }
    }
}