using System.ComponentModel.DataAnnotations;
using EBird.Application.Interfaces.IMapper;
using EBird.Domain.Entities;

namespace EBird.Application.Model;
public class CreatePayment : IMapTo<PaymentEntity>
{
    [Required]
    public double Amount { get; set; }
    
    // public string PaymentType { get; set; } = "VnPay";
    public Guid AccountId { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public int LimitMonth { get; set; }
}