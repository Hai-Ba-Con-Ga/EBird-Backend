using EBird.Domain.Entities;

namespace EBird.Api.UserFeatures.Requests
{
    public class SignupRequest
    {
        public string Password { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string? Email { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
