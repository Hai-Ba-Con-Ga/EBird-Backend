using EBird.Application.Exceptions;
using EBird.Application.Interfaces.IRepository;
using EBird.Application.Services.IServices;
using EBird.Domain.Entities;

namespace EBird.Application.Services;
public class VipService : IVipService
{
    private readonly IGenericRepository<VipRegistrationEntity> _vipRegistrationRepository;
    public VipService(IGenericRepository<VipRegistrationEntity> vipRegistrationRepository)
    {
        _vipRegistrationRepository = vipRegistrationRepository;
    }
    public async Task<VipRegistrationEntity> GetVip(Guid vipID)
    {
        var vipRegistration = await _vipRegistrationRepository.GetByIdActiveAsync(vipID);
        if (vipRegistration == null)
        {
            throw new NotFoundException("Vip not found");
        }
        return vipRegistration;
    }

    public async Task<List<VipRegistrationEntity>> GetVips()
    {
        var vipRegistrations = await _vipRegistrationRepository.GetAllActiveAsync();
        if (vipRegistrations == null)
        {
            throw new NotFoundException("Vips not found");
        }
        return vipRegistrations;
    }

}