using AutoMapper;
using EBird.Api.UserFeatures.Requests;
using EBird.Application.Interfaces;
using EBird.Application.Model;
using EBird.Application.Services.IServices;
using EBird.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;
using Response;
using System.Net;
using System.Security.Claims;

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
            var response = await _accountServices.GetAccountById(id);
            
            return StatusCode((int)response.StatusCode, response);
        }
        [HttpGet]
        public async Task<ActionResult<Response<string>>> GetAllAccount()
        {
            var response = await _accountServices.GetAllAccount();
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Response<string>>> UpdateAccount(Guid id, [FromBody] UpdateAccountRequest updateAccount)
        {
            var acc = await _accountRepository.GetByIdActiveAsync(id);
            
            var response = await _accountServices.UpdateAccount(_mapper.Map<UpdateAccountRequest, AccountEntity>(updateAccount, acc));
            return StatusCode((int)response.StatusCode, response);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<string>>> DeleteAccount(Guid id)
        {
            var response = await _accountServices.DeleteAccount(id);
            return StatusCode((int)response.StatusCode, response);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut("change-password")]
        public async Task<ActionResult<Response<string>>> ChangePassword([FromBody] ChangePasswordModel req)
        {
            string rawId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (rawId == null)
            {
                return Response<string>.Builder().SetStatusCode(401).SetSuccess(false);
            }
            try
            {
                Guid id = Guid.Parse(rawId);
                return await _accountServices.ChangePassword(id, req);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpPut("forgot-password")]
        public async Task<ActionResult<Response<string>>> ForgotPassword([FromBody] ForgotPasswordRequest req)
        {
            try
            {
                return await _accountServices.ForgotPassword(req.Username);
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpPut("reset-password")]
        public async Task<ActionResult<Response<string>>> ResetPassword([FromBody] ResetPasswordModel req)
        {
            try
            {
                return await _accountServices.ResetPassword(req);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpPost("email")]
        public async Task<ActionResult<Response<string>>> CheckEmail([FromBody] CheckEmailRequest req)
        {
            return await _accountServices.CheckEmail(req.Email);
        }
    }
}
