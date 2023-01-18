using EBird.Domain.Common;
using EBird.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EBird.Domain.Entities
{
    [Table("Account")]
    public class AccountEntity : BaseEntity
    {
        [Column(TypeName = "varchar")]
        [MaxLength(50)]
        public string Password { get; set; } = null!;

        [Column(TypeName = "varchar")]
        [MaxLength(50)]
        public string? Email { get; set; }

        public DateTime CreateDateTime { get; set; }
        
        [MaxLength(50)]
        public string FirstName { get; set; } = null!;

        [MaxLength(50)]
        public string LastName { get; set; } = null!;

        [Column("Role",TypeName = "varchar")]
        [MaxLength(20)]
        public string RoleString
        {
            get { return Role.ToString(); }
            private set { Role = Enum.Parse<Role>(value); }
        }
        [NotMapped]
        public Role Role { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(50)]
        public string Username { get; set; } = null!;
        
        public string Description { get; set; } = null!;
    }
}
