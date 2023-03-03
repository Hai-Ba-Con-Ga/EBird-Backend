using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EBird.Domain.Common;
using EBird.Domain.Enums;

namespace EBird.Domain.Entities;
[Table("Payment")]
public class PaymentEntity : BaseEntity
{
    [Column("CreatedDate", TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }
    [Column("Amount", TypeName = "float")]
    public double Amount { get; set; }


    [Column("Status", TypeName = "varchar")]
    [MaxLength(50)]
    public string StatusString
    {
        get { return Status.ToString(); }
        private set { Status = Enum.Parse<PaymentStatus>(value); }
    }
    [NotMapped]
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    [Column("PaymentType", TypeName = "varchar")]
    [MaxLength(50)]
    public string PaymentType { get; set; } = "VnPay";

    [Column("Description", TypeName = "text")]
    public string? Description { get; set; } 
    public Guid AccountId { get; set; }
    public AccountEntity Account { get; set; } = null!;
}