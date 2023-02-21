using EBird.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace EBird.Api.Controllers;

[Route("map")]
[ApiController]
public class MapsController : ControllerBase
{
    private readonly IMapsServices _mapsServices;

    public MapsController(IMapsServices mapsServices)
    {
        _mapsServices = mapsServices;
    }

    [HttpGet]
    public async Task<IActionResult> GetAddress()
    {
        await _mapsServices.GetAddress();
        return Ok();
    }
}
