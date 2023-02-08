using EBird.Application.Interfaces.IMapper;
using EBird.Domain.Entities;
using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace EBird.Application.Model.Auth
{
    public class SignupRequest : IMapTo<AccountEntity>
    {
        [Required (ErrorMessage ="Password is required")]
        [StringLength(20, ErrorMessage = "Password cannot be longer than 20 characters")]
        public string Password { get; set; } = null!;
        [Required(ErrorMessage = "Username is required")]
        [StringLength(20, ErrorMessage = "Username cannot be longer than 20 characters")]
        public string Username { get; set; } = null!;
        [Required(ErrorMessage = "Email is required")]
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
