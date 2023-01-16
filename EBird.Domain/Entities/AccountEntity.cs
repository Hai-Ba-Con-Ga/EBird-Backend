using EBird.Domain.Common;
using EBird.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Domain.Entities
{
    public class AccountEntity : BaseEntity
    {
        public string Password { get; set; } = null!;
        public string? Email { get; set; }
        public string Status { get; set; } = null!;
        public DateTime CreateDateTime { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        [Column("Role")]
        public string RoleString
        {
            get { return Role.ToString(); }
            private set { Role = Enum.Parse<Role>(value) ; }
        }

        [NotMapped]
        public Role Role { get; set; }
        public string Username { get; set; } = null!;
        public string Description { get; set; } = null!;

    }
}
