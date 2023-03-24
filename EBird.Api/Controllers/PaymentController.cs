using System.Net;
using EBird.Application.Model;
using EBird.Application.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Authorization;
using EBird.Application.Extensions;
using EBird.Application.AppConfig;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using EBird.Domain.Entities;

namespace EBird.Api.Controllers;


[Route("payment")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly VnpayConfig _config;
    
    public PaymentController(IPaymentService paymentService, IOptions<VnpayConfig> config)
    {
        _paymentService = paymentService;
        _config = config.Value;
    }

    [HttpPost("create-payment")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<ActionResult<Response<string>>> CreatePayment(CreatePayment request)
    {
        var response = new Response<string>();
        try
        {
            // var origin = Request.Headers["Origin"].ToString();
            // if (string.IsNullOrEmpty(origin))
            // {
            //     var scheme = HttpContext.Request.Scheme;
            //     var host = HttpContext.Request.Host;
            //     origin = $"{scheme}://{host}";
            // }
            var origin = "https://localhost:7137";
            string url = await _paymentService.CreatePayment(request, origin);
            response = Response<string>.Builder().SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK).SetMessage("Success").SetData(url);
        }
        catch (Exception ex)
        {
            response = Response<string>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.InternalServerError).SetMessage(ex.Message);
        }
        return StatusCode((int)response.StatusCode, response);
    }
    [HttpGet("callback-vnpay")]
    public async Task<IActionResult> CallbackVnPay()
    {
        var queryDictionary = QueryHelpers.ParseQuery(Request.QueryString.Value);
        await _paymentService.ProcessCallback(queryDictionary);
        string frontendUrlCallBack = _config.FrontendCallBack;

        string url = QueryHelpers.AddQueryString(frontendUrlCallBack, queryDictionary);
        return Redirect($"https://www.globird.tech/{url}");
    }
    [HttpGet("all")]
    public async Task<ActionResult<Response<List<PaymentEntity>>>> GetAllPayments()
    {
        var response = new Response<List<PaymentEntity>>();
        try
        {
            var payments = await _paymentService.GetPayments();
            response = Response<List<PaymentEntity>>.Builder().SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK).SetMessage("Success").SetData(payments);
        }
        catch (Exception ex)
        {
            response = Response<List<PaymentEntity>>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.InternalServerError).SetMessage(ex.Message);
        }
        return StatusCode((int)response.StatusCode, response);
    }
    [HttpGet("{paymentId}")]
    public async Task<ActionResult<Response<PaymentEntity>>> GetPaymentById(Guid paymentId)
    {
        var response = new Response<PaymentEntity>();
        try
        {
            var payment = await _paymentService.GetPaymentById(paymentId);
            response = Response<PaymentEntity>.Builder().SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK).SetMessage("Success").SetData(payment);
        }
        catch (Exception ex)
        {
            response = Response<PaymentEntity>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.InternalServerError).SetMessage(ex.Message);
        }
        return StatusCode((int)response.StatusCode, response);
    }

    [HttpDelete("{paymentId}")]
    public async Task<ActionResult<Response<string>>> DeletePaymentById(Guid paymentId)
    {
       var response = new Response<string>();
        try
        {
            await _paymentService.DeletePayment(paymentId);
            response = Response<string>.Builder().SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK).SetMessage("Success");
        }
        catch (Exception ex)
        {
            response = Response<string>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.InternalServerError).SetMessage(ex.Message);
        }
        return StatusCode((int)response.StatusCode, response);
    }


    

     

}
