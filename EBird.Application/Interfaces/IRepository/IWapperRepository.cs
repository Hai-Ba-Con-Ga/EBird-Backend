using EBird.Application.Interfaces.IRepository;
using EBird.Application.Services.IServices;
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
        public IGroupRepository Group { get; }
        public IRoomRepository Room { get; }
        public IResourceRepository Resource { get; }
        public IPlaceRepository Place { get; }
        public IRequestRepository Request { get; }
        public INotificationRepository Notification { get; }
        public INotificationTypeRepository NotificationType { get; }
        public IPostRepository Post { get; }
        public IMatchDetailRepository MatchDetail { get; }
        public IMatchRepository Match { get; }
        public IGroupMemberRepository GroupMember { get; }
        public IAccountRepository Account { get; }
        public IFriendshipRepository Friendship { get; }
    }
}
