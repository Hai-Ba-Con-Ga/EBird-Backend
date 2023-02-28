
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
    private readonly IPlaceService _placeService;
    private readonly IMapper _mapper;

    public QuickMatchController(IRequestService requestService, IMapper mapper)
    {
        _requestService = requestService;
        _mapper = mapper;
    }

    [HttpGet("room/{id}")]
    public async Task<ActionResult<Response<List<Guid>>>> QuickMatchRoom(Guid id)
    {
        var response = new Response<List<Guid>>();
        try
        {
            //dynamic jsonArray = MatchingService.LoadJson("REQUEST_TUPLE_MOCK_DATA.json");
            //System.Console.WriteLine(jsonArray);
            //List<RequestTuple> lq = jsonArray.ToObject<List<RequestTuple>>();
            //// Console.WriteLine(lq);
            //System.Console.WriteLine(QuickMatch(new List<RequestTuple>() { lq[1] }, lq[0])[0]);

            var list = await _requestService.GetRequests();
            //var finder = list.Where(x => x.Id == id);
            var finder = new RequestTuple();

            var listRequest = new List<RequestTuple>();

            //Console.WriteLine(list.Count + "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
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
                else 
                    listRequest.Add(requestTuple); 
                //Console.WriteLine(requestTuple);

                //requestTuple = lq[0];
                //Console.WriteLine(requestTuple);
            }
            var priorityRequestList = QuickMatch(listRequest, finder).Select(x => x.id).ToList();


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