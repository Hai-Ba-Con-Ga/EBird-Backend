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
using Stripe.Checkout;
using Stripe;

namespace EBird.Api.Controllers
{
    [Route("/stripe")]
    [ApiController]
    public class StripeController : ControllerBase
    {

        private readonly IPaymentService _paymentService;


        public StripeController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
            Stripe.StripeConfiguration.ApiKey = "sk_test_51MjOMSDIlCFDWwtMJJyh0nNTdevlH2GcAV3E9T7m9b8UZeuq5q2zOIr38gs1OTENNChcjA9Nc7O2c7T1CUxrXrIh00H52ywIlA";
        }

        [HttpPost("payment_intent")]
        // [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Response<string>>> CreatePaymentIntent()
        {
            // CreatePayment request;
            var options = new PaymentIntentCreateOptions
            {
                Amount = 20000,
                Currency = "VND"
            };

            var service = new PaymentIntentService();
            var paymentIntent = service.Create(options);

            return Ok(paymentIntent);
        }
    }

}
