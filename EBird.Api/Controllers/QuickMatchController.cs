
using AutoMapper;
using EBird.Application.Services.IServices;
using EBird.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Response;
using EBird.Application.Exceptions;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using EBird.Domain.Enums;
using System.Security.Claims;
using EBird.Application.Model.Request;
using EBird.Api.Configurations;
using EBird.Application.Services;
using static EBird.Application.Services.MatchingService;
using EBird.Application.Services.Algorithm;

namespace EBird.Api.Controllers;
[Route("quickmatch")]
[ApiController]
public class QuickMatchController : ControllerBase
{
    private readonly IRequestService _requestService;
    private readonly IMapper _mapper;

    public QuickMatchController(IRequestService requestService, IMapper mapper)
    {
        _requestService = requestService;
        _mapper = mapper;
    }

    [HttpGet("room")]
    public async Task<ActionResult<Response<List<RequestEntity>>>> QuickMatchRoom()
    {
        var response = new Response<List<RequestEntity>>();
        try
        {
            dynamic jsonArray = MatchingService.LoadJson("REQUEST_TUPLE_MOCK_DATA.json");
            System.Console.WriteLine(jsonArray);
            List<RequestTuple> listRequest = jsonArray.ToObject<List<RequestTuple>>();
            // Console.WriteLine(listRequest);
            System.Console.WriteLine(MatchingService.QuickMatch(new List<RequestTuple>() { listRequest[1] }, listRequest[0]));


            //var requests = await _requestService.GetRequests();
            response = Response<List<RequestEntity>>.Builder()
                        .SetSuccess(true)
                        .SetStatusCode((int)HttpStatusCode.OK)
                        .SetMessage("Requests are retrieved successfully")
                        .SetData();
        }
        catch (NotFoundException ex)
        {
            response = Response<List<RequestEntity>>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int)HttpStatusCode.BadRequest)
                        .SetMessage(ex.Message);
        }
        catch (Exception ex)
        {
            response = Response<List<RequestEntity>>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int)HttpStatusCode.InternalServerError)
                        .SetMessage(ex.Message);
        }
        return response;
    }

}