
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
[Route("automatch")]
[ApiController]
public class AutoMatchController : ControllerBase
{
    private readonly IRequestService _requestService;
    private readonly IMapper _mapper;

    public AutoMatchController(IRequestService requestService, IMapper mapper)
    {
        _requestService = requestService;
        _mapper = mapper;
    }

    [HttpGet("group/{groupid}")]
    public async Task<ActionResult<Response<List<Guid>>>> AutoMatchGroup(Guid groupid)
    {
        var response = new Response<List<Guid>>();
        try
        {
            var list = (await _requestService.GetRequestsByGroupId(groupid))
                .Where(x => x.Status.Equals(RequestStatus.Waiting)).ToList();
            var finder = new RequestTuple();

            var listRequest = new List<RequestTuple>();

            foreach(var item in list)
            {
                var requestTuple = new RequestTuple(
                    item.Id,
                    ((double)item.Place.Latitude, (double)item.Place.Longitude),
                    item.HostBird.Elo,
                    item.RequestDatetime,
                    false
                );
                listRequest.Add(requestTuple); 
            }
            var priorityRequestList = BinarySearch(listRequest).ToList();


            //var requests = await _requestService.GetRequests();
            response = Response<List<Guid>>.Builder()
                        .SetSuccess(true)
                        .SetStatusCode((int)HttpStatusCode.OK)
                        .SetMessage("Requests are retrieved successfully")
                        .SetData(priorityRequestList);
        }
        catch (NotFoundException ex)
        {
            response = Response<List<Guid>>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int)HttpStatusCode.BadRequest)
                        .SetMessage(ex.Message);
        }
        catch (Exception ex)
        {
            response = Response<List<Guid>>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int)HttpStatusCode.InternalServerError)
                        .SetMessage(ex.Message);
        }
        return response;
    }
}