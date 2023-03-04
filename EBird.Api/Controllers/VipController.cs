
using System.Net;
using EBird.Application.Services.IServices;
using EBird.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Response;

namespace EBird.Api.Controllers;

[Route("vip")]
[ApiController]
public class VipController : ControllerBase
{
    private readonly IVipService _vipService;
    public VipController(IVipService vipService)
    {
        _vipService = vipService;
    }
    [HttpGet]
    public async Task<ActionResult<Response<List<VipRegistrationEntity>>>> GetVips()
    {
        var response = new Response<List<VipRegistrationEntity>>();
        try
        {
            var vipList = await _vipService.GetVips();
            response = Response<List<VipRegistrationEntity>>.Builder().SetData(vipList).SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            response = Response<List<VipRegistrationEntity>>.Builder().SetData(null).SetSuccess(false).SetStatusCode((int)HttpStatusCode.InternalServerError).SetMessage(ex.Message);
        }
        return StatusCode((int)response.StatusCode, response);
    }
    [HttpGet("{vipID}")]
    public async Task<ActionResult<Response<VipRegistrationEntity>>> GetVip(Guid vipID)
    {
        var response = new Response<VipRegistrationEntity>();
        try
        {
            var vip = await _vipService.GetVip(vipID);
            response = Response<VipRegistrationEntity>.Builder().SetData(vip).SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            response = Response<VipRegistrationEntity>.Builder().SetData(null).SetSuccess(false).SetStatusCode((int)HttpStatusCode.InternalServerError).SetMessage(ex.Message);
        }
        return StatusCode((int)response.StatusCode, response);
    }
    
}
