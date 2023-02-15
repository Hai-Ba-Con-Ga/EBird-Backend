using EBird.Application.Interfaces.IRepository;
using EBird.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Interfaces
{
    public interface IWapperRepository
    {
        public IBirdTypeRepository BirdType { get; }
        public IBirdRepository Bird { get; }
        public IGenericRepository<AccountEntity> Account { get; }
        public IGroupRepository Group { get; }
        public IRoomRepository Room { get; }
        public IResourceRepository Resource { get; }
        public IPlaceRepository Place { get; }
        public IRequestRepository Request { get; }
        public INotificationRepository Notification { get; }
        public INotificationTypeRepository NotificationType { get; }
        public IGenericRepository<MatchBirdEntity> MatchBird { get; }
        public IMatchRepository Match { get; }
    }
}
