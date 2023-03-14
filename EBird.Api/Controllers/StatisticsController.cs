using EBird.Api.Controllers.Exentions;
using EBird.Application.Exceptions;
using EBird.Application.Model;
using EBird.Application.Model.Auth;
using EBird.Application.Model.Bird;
using EBird.Application.Model.Match;
using EBird.Application.Model.PagingModel;
using EBird.Application.Model.Request;
using EBird.Application.Model.Resource;
using EBird.Application.Services.IServices;
using EBird.Domain.Entities;
using EBird.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Response;
using System.Globalization;
using System.Net;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EBird.Api.Controllers;
[Route("statistics")]
[ApiController]
public class StatisticsController : ControllerBase
{
    private readonly IBirdService _birdService;
    private readonly IRequestService _requestService;
    private readonly IMatchService _matchService;
    private readonly IAccountServices _accountService;
    private readonly IPaymentService _paymentService;

    public StatisticsController(IBirdService birdService, IRequestService requestService, IMatchService matchService, IAccountServices accountService, IPaymentService paymentService)
    {
        _birdService = birdService;
        _requestService = requestService;
        _matchService = matchService;
        _accountService = accountService;
        _paymentService = paymentService;
    }



    //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleAccount.Admin))]
    [HttpGet("bird")]
    public async Task<ActionResult<Response<List<Tuple<DateTime,List<BirdResponseDTO>>>>>> GetBird()
    {
        Response<List<Tuple<DateTime,List<BirdResponseDTO>>>> response = null;
        try
        {
            List<BirdResponseDTO> rawList = await _birdService.GetBirds();
            rawList = rawList.OrderBy(x => x.CreatedDatetime).ToList();

            var start = rawList.FirstOrDefault()?.CreatedDatetime.Date ?? DateTime.Now;
            var result = new List<Tuple<DateTime,List<BirdResponseDTO>>>();
            int index = 0;

            for (DateTime i = start; i <= DateTime.Now.AddHours(1); i = i.AddDays(1))
            {
                var dateList = new List<BirdResponseDTO>();
                while (index < rawList.Count && i <= rawList[index].CreatedDatetime
                    && rawList[index].CreatedDatetime < i.AddDays(1))
                {
                    dateList.Add(rawList[index]);
                    index++;
                }
                result.Add(Tuple.Create<DateTime,List<BirdResponseDTO>>(i.Date, dateList));
            }

            response = Response<List<Tuple<DateTime,List<BirdResponseDTO>>>>.Builder()
            .SetSuccess(true)
            .SetStatusCode((int)HttpStatusCode.OK)
            .SetMessage("Get successful")
            .SetData(result);
            return StatusCode((int)response.StatusCode, response);
        }
        catch (Exception ex)
        {
            if (ex is BadRequestException || ex is NotFoundException)
            {
                response = Response<List<Tuple<DateTime,List<BirdResponseDTO>>>>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.BadRequest).SetMessage(ex.Message);
                return StatusCode((int)response.StatusCode, response);
            }
            response = Response<List<Tuple<DateTime,List<BirdResponseDTO>>>>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.InternalServerError).SetMessage("Internal Server Error");
            return StatusCode((int)response.StatusCode, response);
        }
    }

    //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleAccount.Admin))]
    [HttpGet("request")]
    public async Task<ActionResult<Response<List<Tuple<DateTime,List<RequestResponseDTO>>>>>> GetRequest()
    {
        Response<List<Tuple<DateTime,List<RequestResponseDTO>>>> response = null;
        try
        {
            List<RequestResponseDTO> rawList = (await _requestService.GetRequests()).ToList();
            rawList = rawList.OrderBy(x => x.CreateDatetime).ToList();

            var start = rawList.FirstOrDefault()?.CreateDatetime.Date ?? DateTime.Now;
            var result = new List<Tuple<DateTime,List<RequestResponseDTO>>>();
            int index = 0;

            for (DateTime i = start; i <= DateTime.Now.AddHours(1); i = i.AddDays(1))
            {
                var dateList = new List<RequestResponseDTO>();
                while (index < rawList.Count && i <= rawList[index].CreateDatetime
                    && rawList[index].CreateDatetime < i.AddDays(1))
                {
                    dateList.Add(rawList[index]);
                    index++;
                }
                result.Add(Tuple.Create<DateTime,List<RequestResponseDTO>>(i.Date, dateList));
            }

            response = Response<List<Tuple<DateTime,List<RequestResponseDTO>>>>.Builder()
            .SetSuccess(true)
            .SetStatusCode((int)HttpStatusCode.OK)
            .SetMessage("Get successful")
            .SetData(result);
            return StatusCode((int)response.StatusCode, response);
        }
        catch (Exception ex)
        {
            if (ex is BadRequestException || ex is NotFoundException)
            {
                response = Response<List<Tuple<DateTime,List<RequestResponseDTO>>>>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.BadRequest).SetMessage(ex.Message);
                return StatusCode((int)response.StatusCode, response);
            }
            response = Response<List<Tuple<DateTime,List<RequestResponseDTO>>>>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.InternalServerError).SetMessage("Internal Server Error");
            return StatusCode((int)response.StatusCode, response);
        }
    }

    //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleAccount.Admin))]
    [HttpGet("account")]
    public async Task<ActionResult<Response<List<Tuple<DateTime,List<AccountResponse>>>>>> GetAccount()
    {
        Response<List<Tuple<DateTime,List<AccountResponse>>>> response = null;
        try
        {
            List<AccountResponse> rawList = (await _accountService.GetAllAccount()).ToList();
            rawList = rawList.OrderBy(x => x.CreateDateTime).ToList();

            var start = rawList.FirstOrDefault()?.CreateDateTime.Date ?? DateTime.Now;
            var result = new List<Tuple<DateTime,List<AccountResponse>>>();
            int index = 0;

            for (DateTime i = start; i <= DateTime.Now.AddHours(1); i = i.AddDays(1))
            {
                var dateList = new List<AccountResponse>();
                while (index < rawList.Count && i <= rawList[index].CreateDateTime
                    && rawList[index].CreateDateTime < i.AddDays(1))
                {
                    dateList.Add(rawList[index]);
                    index++;
                }
                result.Add(Tuple.Create<DateTime,List<AccountResponse>>(i.Date, dateList));
            }

            response = Response<List<Tuple<DateTime,List<AccountResponse>>>>.Builder()
            .SetSuccess(true)
            .SetStatusCode((int)HttpStatusCode.OK)
            .SetMessage("Get successful")
            .SetData(result);
            return StatusCode((int)response.StatusCode, response);
        }
        catch (Exception ex)
        {
            if (ex is BadRequestException || ex is NotFoundException)
            {
                response = Response<List<Tuple<DateTime,List<AccountResponse>>>>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.BadRequest).SetMessage(ex.Message);
                return StatusCode((int)response.StatusCode, response);
            }
            response = Response<List<Tuple<DateTime,List<AccountResponse>>>>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.InternalServerError).SetMessage("Internal Server Error");
            return StatusCode((int)response.StatusCode, response);
        }
    }

    //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleAccount.Admin))]
    [HttpGet("match")]
    public async Task<ActionResult<Response<List<Tuple<DateTime,List<MatchResponseDTO>>>>>> GetMatch()
    {
        Response<List<Tuple<DateTime,List<MatchResponseDTO>>>> response = null;
        try
        {
            var convertDate = (string x) => DateTime.ParseExact(x, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);

            List<MatchResponseDTO> rawList = (await _matchService.GetMatches()).ToList();
            rawList = rawList.OrderBy(x => convertDate(x.CreateDatetime)).ToList();

            var start = convertDate(rawList.FirstOrDefault()?.CreateDatetime ?? "1/1/3000 12:07:10 PM").Date;
            var result = new List<Tuple<DateTime,List<MatchResponseDTO>>>();
            int index = 0;

            for (DateTime i = start; i <= DateTime.Now.AddHours(1); i = i.AddDays(1))
            {
                var dateList = new List<MatchResponseDTO>();
                while (index < rawList.Count && i <= convertDate(rawList[index].CreateDatetime)
                    && convertDate(rawList[index].CreateDatetime) < i.AddDays(1))
                {
                    dateList.Add(rawList[index]);
                    index++;
                }
                result.Add(Tuple.Create<DateTime,List<MatchResponseDTO>>(i.Date, dateList));
            }

            response = Response<List<Tuple<DateTime,List<MatchResponseDTO>>>>.Builder()
            .SetSuccess(true)
            .SetStatusCode((int)HttpStatusCode.OK)
            .SetMessage("Get successful")
            .SetData(result);
            return StatusCode((int)response.StatusCode, response);
        }
        catch (Exception ex)
        {
            if (ex is BadRequestException || ex is NotFoundException)
            {
                response = Response<List<Tuple<DateTime,List<MatchResponseDTO>>>>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.BadRequest).SetMessage(ex.Message);
                return StatusCode((int)response.StatusCode, response);
            }
            response = Response<List<Tuple<DateTime,List<MatchResponseDTO>>>>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.InternalServerError).SetMessage("Internal Server Error");
            return StatusCode((int)response.StatusCode, response);
        }
    }


    //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleAccount.Admin))]
    [HttpGet("payment")]
    public async Task<ActionResult<Response<List<Tuple<DateTime,List<PaymentEntity>>>>>> GetPayment()
    {
        Response<List<Tuple<DateTime,List<PaymentEntity>>>> response = null;
        try
        {
            List<PaymentEntity> rawList = (await _paymentService.GetPayments()).ToList();
            rawList = rawList.OrderBy(x => x.CreatedDate).ToList();

            var start = rawList.FirstOrDefault()?.CreatedDate.Date ?? DateTime.Now;
            var result = new List<Tuple<DateTime,List<PaymentEntity>>>();
            int index = 0;

            for (DateTime i = start; i <= DateTime.Now.AddHours(1); i = i.AddDays(1))
            {
                var dateList = new List<PaymentEntity>();
                while (index < rawList.Count && i <= rawList[index].CreatedDate
                    && rawList[index].CreatedDate < i.AddDays(1))
                {
                    dateList.Add(rawList[index]);
                    index++;
                }
                result.Add(Tuple.Create<DateTime,List<PaymentEntity>>(i.Date, dateList));
            }

            response = Response<List<Tuple<DateTime,List<PaymentEntity>>>>.Builder()
            .SetSuccess(true)
            .SetStatusCode((int)HttpStatusCode.OK)
            .SetMessage("Get successful")
            .SetData(result);
            return StatusCode((int)response.StatusCode, response);
        }
        catch (Exception ex)
        {
            if (ex is BadRequestException || ex is NotFoundException)
            {
                response = Response<List<Tuple<DateTime,List<PaymentEntity>>>>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.BadRequest).SetMessage(ex.Message);
                return StatusCode((int)response.StatusCode, response);
            }
            response = Response<List<Tuple<DateTime,List<PaymentEntity>>>>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.InternalServerError).SetMessage("Internal Server Error");
            return StatusCode((int)response.StatusCode, response);
        }
    }

}
