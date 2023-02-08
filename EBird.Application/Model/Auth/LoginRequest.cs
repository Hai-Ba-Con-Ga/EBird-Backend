using Duende.IdentityServer.Models;
using System.ComponentModel.DataAnnotations;

namespace EBird.Application.Model.Auth
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(20, ErrorMessage = "Username cannot be longer than 20 characters")]
        public string Username { get; set; } = null!;
        [Required(ErrorMessage = "Password is required")]
        [StringLength(20, ErrorMessage = "Password cannot be longer than 20 characters")]
        public string Password { get; set; } = null!;
    }
}
