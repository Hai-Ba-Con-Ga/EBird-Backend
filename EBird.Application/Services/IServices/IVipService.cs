using EBird.Domain.Entities;

namespace EBird.Application.Services.IServices;
public interface IVipService
{
    
    Task<List<VipRegistrationEntity>> GetVips();
    Task<VipRegistrationEntity> GetVip(Guid vipID);
}