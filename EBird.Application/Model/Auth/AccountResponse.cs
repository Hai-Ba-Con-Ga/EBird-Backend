using EBird.Domain.Entities;
using EBird.Domain.Enums;

namespace EBird.Application.Model.Auth
{
    public class AccountResponse
    {
        public Guid Id { get; init; }
        public string? Email { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public Role Role { get; set; }
        public string Username { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
