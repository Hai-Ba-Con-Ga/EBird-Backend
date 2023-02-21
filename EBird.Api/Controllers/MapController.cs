using System.Net;
using EBird.Application.Exceptions;
using EBird.Application.Model.Maps;
using EBird.Application.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Response;

namespace EBird.Api.Controllers;
[Route("/map")]
[ApiController]
public class MapController : ControllerBase
{
    private readonly IMapsServices _mapService;
    public MapController(IMapsServices mapService)
    {
        _mapService = mapService;
    }
    
    [HttpGet("geocoding")]
    public async Task<ActionResult<Response<GeocodingApiLocation>>> GetGeocoding([FromQuery]string address)
    {
        var response = new Response<GeocodingApiLocation>();
        try
        {
            var result = await _mapService.GetGeocoding(address);
            response = Response<GeocodingApiLocation>.Builder().SetData(result).SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK).SetMessage("Get geocoding successfully");


        }
        catch (BadRequestException ex)
        {
            response = Response<GeocodingApiLocation>.Builder().SetData(null).SetSuccess(false).SetStatusCode((int)HttpStatusCode.BadRequest).SetMessage(ex.Message);
        }
        catch (Exception ex)
        {
            response = Response<GeocodingApiLocation>.Builder().SetData(null).SetSuccess(false).SetStatusCode((int)HttpStatusCode.InternalServerError).SetMessage(ex.Message);
        }
        return StatusCode((int)response.StatusCode, response);


    }
}