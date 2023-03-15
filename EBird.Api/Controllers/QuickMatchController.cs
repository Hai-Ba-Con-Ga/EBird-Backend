
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
using EBird.Application.Interfaces.IValidation;

namespace EBird.Api.Controllers;
[Route("quickmatch")]
[ApiController]
public class QuickMatchController : ControllerBase
{
    private readonly IRequestService _requestService;
    private readonly IPlaceService _placeService;
    private readonly IUnitOfValidation _unitOfValidation;
    private readonly IMapper _mapper;

    public QuickMatchController(IRequestService requestService, IPlaceService placeService, IUnitOfValidation unitOfValidation, IMapper mapper)
    {
        _requestService = requestService;
        _placeService = placeService;
        _unitOfValidation = unitOfValidation;
        _mapper = mapper;
    }

    [HttpGet("room/{roomid}/{id}")]
    public async Task<ActionResult<Response<List<Guid>>>> QuickMatchRoom(Guid id, Guid roomid)
    {
        var response = new Response<List<Guid>>();
        try
        {
            //dynamic jsonArray = MatchingService.LoadJson("REQUEST_TUPLE_MOCK_DATA.json");
            //System.Console.WriteLine(jsonArray);
            //List<RequestTuple> lq = jsonArray.ToObject<List<RequestTuple>>();
            //// Console.WriteLine(lq);
            //System.Console.WriteLine(QuickMatch(new List<RequestTuple>() { lq[1] }, lq[0])[0]);

            var list = (await _requestService.GetRequests())
                .Where(x => x.Room.Id == roomid && x.Group == null && x.Status.Equals(RequestStatus.Waiting)).ToList();
            //var finder = list.Where(x => x.Id == id);
            var finder = new RequestTuple();
            var accountIdFinder = list.Where(x => x.Id == id).ToList()?[0].Host.Id;

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
                if (id == item.Id) 
                    finder = requestTuple;
                if (item.Host.Id == accountIdFinder)
                    continue;
                listRequest.Add(requestTuple); 
            }
            var priorityRequestList = QuickMatch(listRequest, finder).Select(x => x.id).ToList();

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



    [HttpGet("group/{groupId}/{requestId}/")]
    public async Task<ActionResult<Response<List<Guid>>>> QuickMatchGroup(Guid groupId, Guid requestId)
    {
        var response = new Response<List<Guid>>();
        try
        {
            var list = (await _requestService.GetRequestsByGroupId(groupId))
                .Where(x => x.Status.Equals(RequestStatus.Waiting)).ToList();
            var finder = new RequestTuple();
            var accountIdFinder = list.Where(x => x.Id == requestId).ToList()?[0].Host.Id;

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
                if (requestId == item.Id) 
                    finder = requestTuple;
                if (item.Host.Id == accountIdFinder)
                    continue;
                listRequest.Add(requestTuple); 
            }
            var priorityRequestList = QuickMatch(listRequest, finder).Select(x => x.id).ToList();

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