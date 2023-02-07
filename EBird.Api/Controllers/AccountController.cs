using AutoMapper;
using EBird.Api.UserFeatures.Requests;
using EBird.Application.Exceptions;
using EBird.Application.Interfaces;
using EBird.Application.Interfaces.IRepository;
using EBird.Application.Model.Auth;
using EBird.Application.Services.IServices;
using EBird.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;
using Response;
using System.Net;
using System.Security.Claims;
using EBird.Domain.Enums;

namespace EBird.Api.Controllers
{
    [Route("account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountServices _accountServices;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<AccountEntity> _accountRepository;

        public AccountController(IAccountServices accountServices, IMapper mapper, IGenericRepository<AccountEntity> accountRepository)
        {
            _accountServices = accountServices;
            _mapper = mapper;
            _accountRepository = accountRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response<AccountResponse>>> GetAccountById(Guid id)
        {
            var response = new Response<AccountResponse>();
            try
            {
                var account = await _accountServices.GetAccountById(id);
                response = Response<AccountResponse>.Builder().SetData(account).SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK);
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
        [Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(Role.Admin))]
        [HttpGet]
        public async Task<ActionResult<Response<List<AccountResponse>>>> GetAllAccount()
        {
            var response = new Response<List<AccountResponse>>();
            try
            {
                var account = await _accountServices.GetAllAccount();
                response = Response<List<AccountResponse>>.Builder().SetData(account).SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK);
            }
            catch (NotFoundException ex)
            {
                response = Response<List<AccountResponse>>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.NotFound).SetMessage(ex.Message);
            }
            catch (Exception ex)
            {
                response = Response<List<AccountResponse>>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.InternalServerError).SetMessage(ex.Message);
            }
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Response<string>>> UpdateAccount(Guid id, [FromBody] UpdateAccountRequest updateAccount)
        {
            var acc = await _accountRepository.GetByIdActiveAsync(id);
            var response = new Response<string>();

            if (acc == null)
            {
                response = Response<string>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.NotFound).SetMessage("Account not found");
            }
            try
            {
                await _accountRepository.UpdateAsync(_mapper.Map<UpdateAccountRequest, AccountEntity>(updateAccount, acc));
                response = Response<string>.Builder().SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK).SetMessage("Update account successfully");
            }
            catch (NotFoundException ex)
            {
                response = Response<string>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.NotFound).SetMessage(ex.Message);
            }
            catch (Exception ex)
            {
                response = Response<string>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.InternalServerError).SetMessage(ex.Message);
            }
            return StatusCode((int)response.StatusCode, response);

        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<string>>> DeleteAccount(Guid id)
        {
            var response = new Response<string>();
            try
            {
                await _accountRepository.DeleteAsync(id);
                response = Response<string>.Builder().SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK).SetMessage("Delete account successfully");
            }
            catch (NotFoundException ex)
            {
                response = Response<string>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.NotFound).SetMessage(ex.Message);
            }
            catch (Exception ex)
            {
                response = Response<string>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.InternalServerError).SetMessage(ex.Message);

            }
            return StatusCode((int)response.StatusCode, response);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut("change-password")]
        public async Task<ActionResult<Response<string>>> ChangePassword([FromBody] ChangePasswordModel req)
        {
            string rawId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = new Response<string>();
            if (rawId == null)
            {
                response = Response<string>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.NotFound).SetMessage("Account not found");
            }
            try
            {
                Guid id = Guid.Parse(rawId);
                await _accountServices.ChangePassword(id, req);
                response = Response<string>.Builder().SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK).SetMessage("Change password successfully");
            }
            catch (NotFoundException ex)
            {
                response = Response<string>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.NotFound).SetMessage(ex.Message);
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
        [HttpPut("forgot-password")]
        public async Task<ActionResult<Response<string>>> ForgotPassword([FromBody] ForgotPasswordRequest req)
        {
            var response = new Response<string>();
            try
            {
                await _accountServices.ForgotPassword(req.Username);
                response = Response<string>.Builder().SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK).SetMessage("Send email successfully");
            }
            catch (NotFoundException ex)
            {
                response = Response<string>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.NotFound).SetMessage(ex.Message);
            }
            catch (Exception ex)
            {
                response = Response<string>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.InternalServerError).SetMessage(ex.Message);
            }
            return StatusCode((int)response.StatusCode, response);
        }
        [HttpPut("reset-password")]
        public async Task<ActionResult<Response<string>>> ResetPassword([FromBody] ResetPasswordModel req)
        {
            var response = new Response<string>();
            try
            {
                await _accountServices.ResetPassword(req);
                response = Response<string>.Builder().SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK).SetMessage("Reset password successfully");
            }
            catch (NotFoundException ex)
            {
                response = Response<string>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.NotFound).SetMessage(ex.Message);
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
        [HttpPost("email")]
        public async Task<ActionResult<Response<string>>> CheckEmail([FromBody] CheckEmailRequest req)
        {
            var response = new Response<string>();
            try
            {
                await _accountServices.CheckEmail(req.Email);
                response = Response<string>.Builder().SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK).SetMessage("Check email successfully");
            }
            catch (NotFoundException ex)
            {
                response = Response<string>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.NotFound).SetMessage(ex.Message);
            }
            catch (Exception ex)
            {
                response = Response<string>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.InternalServerError).SetMessage(ex.Message);
            }
            return StatusCode((int)response.StatusCode, response);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(Role.Admin))]
        [HttpPut("change-role-admin/{id}")]
        public async Task<ActionResult<Response<string>>> ChangeRoleAdmin(Guid id)
        {
            var response = new Response<string>();
            try
            {
                await _accountServices.ChangeRoleAdmin(id);
                response = Response<string>.Builder().SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK).SetMessage("Change role successfully");
            }
            catch (Exception ex)
            {
                response = Response<string>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.InternalServerError).SetMessage(ex.Message);
            }
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
