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
        Task<PagedList<RequestEntity>> GetRequests(RequestParameters parameters);
        Task<ICollection<RequestEntity>> GetRequests();
    }
}
