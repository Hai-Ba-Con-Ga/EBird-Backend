using Duende.IdentityServer.Models;
using System.ComponentModel.DataAnnotations;

namespace EBird.Api.UserFeatures.Requests
{
    public class UpdateAccountRequest
    {

        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Required(ErrorMessage = "FirstName is required")]
        [StringLength(50, ErrorMessage = "FirstName cannot be longer than 50 characters")]
        public string FirstName { get; set; } = null!;
        [Required(ErrorMessage = "LastName is required")]
        [StringLength(50, ErrorMessage = "LastName cannot be longer than 50 characters")]
        public string LastName { get; set; } = null!;
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = null!;
    }
}
