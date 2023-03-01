using EBird.Application.Model.PagingModel;
using EBird.Application.Model.Request;
using EBird.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Interfaces.IRepository
{
    public interface IRequestRepository : IGenericRepository<RequestEntity>
    {
        Task<RequestEntity> GetRequest(Guid id);
        Task<PagedList<RequestEntity>> GetRequestsInAllRoom(RequestParameters parameters);
        Task<ICollection<RequestEntity>> GetRequestsInAllRoom();
        Task JoinRequest(Guid requestId, Guid userId, JoinRequestDTO joinRequestDto);
        Task<Guid> MergeRequest(Guid hostRequestId, Guid challengerRequestId);

        Task<ICollection<RequestEntity>> GetRequestsByGroupId(Guid groupId);
        Task ReadyRequest(Guid requestId);
        Task<ICollection<RequestEntity>> GetRequestByUserId(Guid userId);
    }
}
