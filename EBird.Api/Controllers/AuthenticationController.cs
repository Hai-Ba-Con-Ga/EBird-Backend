using AutoMapper;
using EBird.Api.UserFeatures.Requests;
using EBird.Application.Model.Auth;
using EBird.Application.Services.IServices;
using EBird.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Response;
using System.Net;
using System.Security.Claims;
using EBird.Application.Exceptions;

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
            Response<TokenModel> response = new Response<TokenModel>();
            try
            {
                var result = await _authenticationServices.Login(req.Username, req.Password);
                response = Response<TokenModel>.Builder().SetData(result).SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK);
            }
            catch (NotFoundException ex)
            {
                response = Response<TokenModel>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.NotFound).SetMessage(ex.Message);
            }
            catch (BadRequestException ex)
            {
                response = Response<TokenModel>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.BadRequest).SetMessage(ex.Message);
            }
            catch (Exception ex)
            {
                response = Response<TokenModel>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.InternalServerError).SetMessage(ex.Message);
            }
            return StatusCode((int)response.StatusCode, response);

        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete("logout")]
        public async Task<ActionResult<Response<string>>> Logout()
        {
            string rawId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (rawId == null)
            {
                return Response<string>.Builder().SetStatusCode((int)HttpStatusCode.Unauthorized).SetSuccess(false);
            }
            try
            {
                Guid id = Guid.Parse(rawId);
                await _authenticationServices.Logout(id);
                return Response<string>.Builder().SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpPost("signup")]
        public async Task<ActionResult<Response<string>>> Signup(SignupRequest req)
        {
            Response<string> response = new Response<string>();
            try
            {
                await _authenticationServices.Signup(_mapper.Map<AccountEntity>(req));
                response = Response<string>.Builder().SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK);
            }
            catch (BadRequestException ex)
            {
                response = Response<string>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.BadRequest).SetMessage(ex.Message);
            }
            catch (Exception ex)
            {
                response = Response<string>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.InternalServerError).SetMessage(ex.Message);
            }
            return StatusCode((int)response.StatusCode, response);


        }
        [HttpPost("renew-token")]
        public async Task<ActionResult<Response<TokenModel>>> RenewToken(TokenModel model)
        {
            Response<TokenModel> response = new Response<TokenModel>();
            try
            {
                var result = await _authenticationServices.RenewToken(model);
                response = Response<TokenModel>.Builder().SetData(result).SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK);

            }
            catch (BadRequestException ex)
            {
                response = Response<TokenModel>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.BadRequest).SetMessage(ex.Message);
            }
            catch (Exception ex)
            {
                response = Response<TokenModel>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.InternalServerError).SetMessage(ex.Message);
            }
            return StatusCode((int)response.StatusCode, response);


        }
        [HttpGet("me")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Response<AccountResponse>>> GetAccountInformation()
        {
            Response<AccountResponse> response = new Response<AccountResponse>();
            string rawId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (rawId == null)
            {
                response = Response<AccountResponse>.Builder().SetStatusCode((int)HttpStatusCode.Forbidden).SetSuccess(false);
            }
            try
            {
                Guid id = Guid.Parse(rawId);
                var result = await _authenticationServices.GetAccountById(id);
                response = Response<AccountResponse>.Builder().SetData(result).SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK);
            }
            catch (NotFoundException ex)
            {
                response = Response<AccountResponse>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.NotFound).SetMessage(ex.Message);
            }
            catch (Exception ex)
            {
                response = Response<AccountResponse>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.InternalServerError).SetMessage(ex.Message);
            }
            return StatusCode((int)response.StatusCode, response);
        }


        [HttpGet("login-with-google")]
        public async Task<ActionResult<Response<TokenModel>>> LoginWithGoogle([FromQuery] string idToken)
        {
            Response<TokenModel> response = new Response<TokenModel>();
            try
            {
                var result = await _authenticationServices.LoginWithGoogle(idToken);
                response = Response<TokenModel>.Builder().SetData(result).SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK);
            }
            catch(BadRequestException ex){
                response = Response<TokenModel>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.BadRequest).SetMessage(ex.Message);
            }
            catch (Exception ex)
            {
                response = Response<TokenModel>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.InternalServerError).SetMessage(ex.Message);
            }
            return StatusCode((int)response.StatusCode, response);
        }

    }
}
