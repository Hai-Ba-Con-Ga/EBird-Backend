using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Services.IServices
{
    public interface IFriendshipService
    {
        Task CreateInvitaion(Guid userId, Guid receiverId);
    }
}
