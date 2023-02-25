using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBird.Application.Model.PagingModel;
using EBird.Application.Model.Request;

namespace EBird.Application.Services.IServices
{
    public interface IRequestService
    {
        public Task<Guid> CreateRequest(RequestCreateDTO request);
        public Task UpdateRequest(Guid id, RequestUpdateDTO request);
        public Task DeleteRequest(Guid id);
        public Task<RequestResponse> GetRequest(Guid id);
        public Task<PagedList<RequestResponse>> GetRequests(RequestParameters parameters);
        public Task<ICollection<RequestResponse>> GetRequests();
        public Task<ICollection<RequestResponse>> GetRequestsByGroupId(Guid groupId);
        public Task JoinRequest(Guid requestId, Guid userId, JoinRequestDTO joinRequestDto);
        public Task<Guid> MergeRequest(Guid hostRequestId, Guid challengerRequestId);
    }
}
