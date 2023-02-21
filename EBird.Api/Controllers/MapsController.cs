using EBird.Application.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace EBird.Api.Controllers;
[Route("/map")]
[ApiController]
public class MapsController : ControllerBase
{
    private readonly IMapsServices _mapsServices;
    public MapsController(IMapsServices mapService)
    {
        _mapsServices = mapService;
    }
    [HttpGet]
    public async Task<IActionResult> Get()
    {
         await _mapsServices.GetAddress();
        return Ok();
    }
}
