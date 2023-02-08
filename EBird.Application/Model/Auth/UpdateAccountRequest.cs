using Duende.IdentityServer.Models;
using EBird.Application.Interfaces.IMapper;
using EBird.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace EBird.Application.Model.Auth
{
    public class UpdateAccountRequest : IMapTo<AccountEntity>
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
