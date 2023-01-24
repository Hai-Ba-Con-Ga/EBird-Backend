namespace EBird.Api.UserFeatures.Requests
{
    public class UpdateAccountRequest
    {
        public string? Email { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
