using EBird.Application.Services.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Services.IServices
{
    public interface IMatchingService
    {
        //public List<RequestTuple> QuickMatch(List<RequestTuple> listRequest, RequestTuple finder);
        public Task<List<(Guid, Guid)>> BinarySearch(List<RequestTuple> listRequest);
        public dynamic LoadJson(string f);
    }
}
