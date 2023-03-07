using System.ComponentModel.DataAnnotations.Schema;
using EBird.Domain.Common;

namespace EBird.Domain.Entities
{
    [Table("VipRegistration")]
    public class VipRegistrationEntity : BaseEntity
    {
        public Guid AccountId { get; set; }
        public AccountEntity Account { get; set; } = null!;
        public Guid PaymentId { get; set; }
        public PaymentEntity Payment { get; set; } = null!;
        
        public DateTime CreatedDate { get; set; }
        public DateTime ExpiredDate { get; set; }
        [Column(TypeName = "text")]
        public string? Description { get; set; }
    }
}