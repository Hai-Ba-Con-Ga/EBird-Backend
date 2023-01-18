using EBird.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Domain.Entities
{
    [Table("RefreshToken")]
    public class RefreshTokenEntity : BaseEntity
    {
        public Guid AccountId { get; set; }
        [ForeignKey(nameof(AccountId))]
        public AccountEntity Account { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string JwtId { get; set; } = null!;
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime IssuedAt { get; set; }
        public DateTime ExpiredAt { get; set; }

    }
}
