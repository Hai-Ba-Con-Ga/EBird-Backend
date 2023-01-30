using AutoMapper;
using EBird.Api.UserFeatures.Requests;
using EBird.Application.Interfaces;
using EBird.Application.Model;
using EBird.Application.Services.IServices;
using EBird.Domain.Entities;
using EBird.Domain.Enums;
using EBird.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Response;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace EBird.Api.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationServices _authenticationServices;
        private readonly IMapper _mapper;

        public AuthenticationController(IAuthenticationServices authenticationServices, IMapper mapper)
        {
            _authenticationServices = authenticationServices;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<ActionResult<Response<TokenModel>>> Login(LoginRequest req)
        {
            return await _authenticationServices.Login(req.Username, req.Password);

        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete("logout")]
        public async Task<ActionResult<Response<string>>> Logout()
        {
            string rawId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(rawId == null)
            {
                return Response<string>.Builder().SetStatusCode((int)HttpStatusCode.Unauthorized).SetSuccess(false);
            }
            try
            {
                Guid id = Guid.Parse(rawId);
                return await _authenticationServices.Logout(id);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpPost("signup")]
        public async Task<ActionResult<Response<string>>> Signup(SignupRequest req)
        {
            return await _authenticationServices.Signup(_mapper.Map <AccountEntity> (req));
        }
        [HttpPost("renew-token")]
        public async Task<ActionResult<Response<TokenModel>>> RenewToken(TokenModel model)
        {
            return await _authenticationServices.RenewToken(model);
        }
        [HttpGet("me")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Response<AccountResponse>>> GetAccountInformation()
        {
            string rawId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (rawId == null)
            {
                return Response<AccountResponse>.Builder().SetStatusCode((int)HttpStatusCode.Forbidden).SetSuccess(false);
            }
            try
            {
                Guid id = Guid.Parse(rawId);
                return await _authenticationServices.GetAccountById(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
