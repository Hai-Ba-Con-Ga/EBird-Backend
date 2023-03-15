using AutoMapper;
using EBird.Api.Controllers.Exentions;
using EBird.Application.Exceptions;
using EBird.Application.Model.Group;
using EBird.Application.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Response;
using System.Net;
using Imagekit.Sdk;
using Imagekit.Models;
namespace EBird.Api.Controllers
{
    [Route("/imagekit")]
    [ApiController]
    public class ImagekitController : ControllerBase
    {

        // GET all
        [HttpGet("sign")]
        public async Task<ActionResult<AuthParamResponse>> Get()
        {

            ImagekitClient imagekit = new ImagekitClient("public_S6vyU9FG56dNofgzx0hbbBAZGDs=", "private_1LD3K7nVG8n6LkP08+Lk21zCZ3M=", "https://ik.imagekit.io/flamefoxeswyvernp/");
            var sign = imagekit.GetAuthenticationParameters();
            return sign;

        }
    }

}
