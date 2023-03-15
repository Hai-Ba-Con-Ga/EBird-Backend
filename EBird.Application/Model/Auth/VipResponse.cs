using EBird.Application.Interfaces.IMapper;
using EBird.Domain.Entities;

namespace EBird.Application.Model;
public class VipResponse : IMapFrom<VipRegistrationEntity>
{
    public Guid PaymentId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ExpiredDate { get; set; }
    public string? Description { get; set; }
}