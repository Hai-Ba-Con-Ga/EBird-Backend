using EBird.Application.Interfaces.IMapper;
using EBird.Domain.Entities;
using EBird.Domain.Enums;

namespace EBird.Application.Model.Auth
{
    public class AccountResponse : IMapFrom<AccountEntity>
    {
        public Guid Id { get; init; }
        public string? Email { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public RoleAccount Role { get; set; }
        public string Username { get; set; } = null!;
        public string Description { get; set; } = null!;

        public VipResponse? Vip { get; set; }
         
    }
}
