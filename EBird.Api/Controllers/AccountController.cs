using AutoMapper;
using EBird.Application.Model.Auth;
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
using EBird.Application.Model.PagingModel;

namespace EBird.Api.Controllers
{
    [Route("account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountServices _accountServices;
        private readonly IAuthenticationServices _authenticationServices;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<AccountEntity> _accountRepository;

        public AccountController(IAccountServices accountServices, IMapper mapper, 
        IGenericRepository<AccountEntity> accountRepository, 
        IAuthenticationServices authenticationServices)
        {
            _accountServices = accountServices;
            _mapper = mapper;
            _accountRepository = accountRepository;
            _authenticationServices = authenticationServices;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response<AccountResponse>>> GetAccountById(Guid id)
        {
            var response = new Response<AccountResponse>();
            try
            {
                // var account = await _accountServices.GetAccountById(id);
                var account = await _authenticationServices.GetAccountById(id);
                
                response = Response<AccountResponse>.Builder().SetData(account).SetSuccess(true).SetStatusCode((int) HttpStatusCode.OK);
            }
            catch(NotFoundException ex)
            {
                response = Response<AccountResponse>.Builder().SetSuccess(false).SetStatusCode((int) HttpStatusCode.NotFound).SetMessage(ex.Message);
            }
            catch(Exception ex)
            {
                response = Response<AccountResponse>.Builder().SetSuccess(false).SetStatusCode((int) HttpStatusCode.InternalServerError).SetMessage(ex.Message);
            }

            return StatusCode((int) response.StatusCode, response);
        }
        [Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleAccount.Admin))]
        [HttpGet]
        public async Task<ActionResult<Response<List<AccountResponse>>>> GetAllAccount()
        {
            var response = new Response<List<AccountResponse>>();
            try
            {
                var account = await _accountServices.GetAllAccount();
                response = Response<List<AccountResponse>>.Builder().SetData(account).SetSuccess(true).SetStatusCode((int) HttpStatusCode.OK);
            }
            catch(NotFoundException ex)
            {
                response = Response<List<AccountResponse>>.Builder().SetSuccess(false).SetStatusCode((int) HttpStatusCode.NotFound).SetMessage(ex.Message);
            }
            catch(Exception ex)
            {
                response = Response<List<AccountResponse>>.Builder().SetSuccess(false).SetStatusCode((int) HttpStatusCode.InternalServerError).SetMessage(ex.Message);
            }
            return StatusCode((int) response.StatusCode, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Response<string>>> UpdateAccount(Guid id, [FromBody] UpdateAccountRequest updateAccount)
        {
            var acc = await _accountRepository.GetByIdActiveAsync(id);
            var response = new Response<string>();

            if(acc == null)
            {
                response = Response<string>.Builder().SetSuccess(false).SetStatusCode((int) HttpStatusCode.NotFound).SetMessage("Account not found");
            }
            try
            {
                await _accountRepository.UpdateAsync(_mapper.Map<UpdateAccountRequest, AccountEntity>(updateAccount, acc));
                response = Response<string>.Builder().SetSuccess(true).SetStatusCode((int) HttpStatusCode.OK).SetMessage("Update account successfully");
            }
            catch(NotFoundException ex)
            {
                response = Response<string>.Builder().SetSuccess(false).SetStatusCode((int) HttpStatusCode.NotFound).SetMessage(ex.Message);
            }
            catch(Exception ex)
            {
                response = Response<string>.Builder().SetSuccess(false).SetStatusCode((int) HttpStatusCode.InternalServerError).SetMessage(ex.Message);
            }
            return StatusCode((int) response.StatusCode, response);

        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<string>>> DeleteAccount(Guid id)
        {
            var response = new Response<string>();
            try
            {
                await _accountServices.DeleteAccount(id);

                response = Response<string>.Builder()
                .SetSuccess(true)
                .SetStatusCode((int) HttpStatusCode.OK)
                .SetMessage("Delete account successfully");
            }
            catch(Exception ex)
            {
                if(ex is BadRequestException || ex is NotFoundException)
                {
                    response = Response<string>.Builder()
                    .SetSuccess(false)
                    .SetStatusCode((int) HttpStatusCode.BadRequest)
                    .SetMessage(ex.Message);
                }
                else
                {
                    response = Response<string>.Builder()
                    .SetSuccess(false)
                    .SetStatusCode((int) HttpStatusCode.InternalServerError)
                    .SetMessage(ex.Message);
                }
            }
            return StatusCode((int) response.StatusCode, response);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut("change-password")]
        public async Task<ActionResult<Response<string>>> ChangePassword([FromBody] ChangePasswordModel req)
        {
            string rawId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = new Response<string>();
            if(rawId == null)
            {
                response = Response<string>.Builder().SetSuccess(false).SetStatusCode((int) HttpStatusCode.NotFound).SetMessage("Account not found");
            }
            try
            {
                Guid id = Guid.Parse(rawId);
                await _accountServices.ChangePassword(id, req);
                response = Response<string>.Builder().SetSuccess(true).SetStatusCode((int) HttpStatusCode.OK).SetMessage("Change password successfully");
            }
            catch(NotFoundException ex)
            {
                response = Response<string>.Builder().SetSuccess(false).SetStatusCode((int) HttpStatusCode.NotFound).SetMessage(ex.Message);
            }
            catch(BadRequestException ex)
            {
                response = Response<string>.Builder().SetSuccess(false).SetStatusCode((int) HttpStatusCode.BadRequest).SetMessage(ex.Message);
            }
            catch(Exception ex)
            {
                response = Response<string>.Builder().SetSuccess(false).SetStatusCode((int) HttpStatusCode.InternalServerError).SetMessage(ex.Message);
            }
            return StatusCode((int) response.StatusCode, response);
        }

        [HttpPut("forgot-password")]
        public async Task<ActionResult<Response<string>>> ForgotPassword([FromBody] ForgotPasswordRequest req)
        {
            var response = new Response<string>();
            try
            {
                await _accountServices.ForgotPassword(req.Username);
                response = Response<string>.Builder().SetSuccess(true).SetStatusCode((int) HttpStatusCode.OK).SetMessage("Send email successfully");
            }
            catch(NotFoundException ex)
            {
                response = Response<string>.Builder().SetSuccess(false).SetStatusCode((int) HttpStatusCode.NotFound).SetMessage(ex.Message);
            }
            catch(Exception ex)
            {
                response = Response<string>.Builder().SetSuccess(false).SetStatusCode((int) HttpStatusCode.InternalServerError).SetMessage(ex.Message);
            }
            return StatusCode((int) response.StatusCode, response);
        }

        [HttpPut("reset-password")]
        public async Task<ActionResult<Response<string>>> ResetPassword([FromBody] ResetPasswordModel req)
        {
            var response = new Response<string>();
            try
            {
                await _accountServices.ResetPassword(req);
                response = Response<string>.Builder().SetSuccess(true).SetStatusCode((int) HttpStatusCode.OK).SetMessage("Reset password successfully");
            }
            catch(NotFoundException ex)
            {
                response = Response<string>.Builder().SetSuccess(false).SetStatusCode((int) HttpStatusCode.NotFound).SetMessage(ex.Message);
            }
            catch(BadRequestException ex)
            {
                response = Response<string>.Builder().SetSuccess(false).SetStatusCode((int) HttpStatusCode.BadRequest).SetMessage(ex.Message);
            }
            catch(Exception ex)
            {
                response = Response<string>.Builder().SetSuccess(false).SetStatusCode((int) HttpStatusCode.InternalServerError).SetMessage(ex.Message);
            }
            return StatusCode((int) response.StatusCode, response);
        }

        [HttpPost("email")]
        public async Task<ActionResult<Response<string>>> CheckEmail([FromBody] CheckEmailRequest req)
        {
            var response = new Response<string>();
            try
            {
                await _accountServices.CheckEmail(req.Email);
                response = Response<string>.Builder().SetSuccess(true).SetStatusCode((int) HttpStatusCode.OK).SetMessage("Check email successfully");
            }
            catch(NotFoundException ex)
            {
                response = Response<string>.Builder().SetSuccess(false).SetStatusCode((int) HttpStatusCode.NotFound).SetMessage(ex.Message);
            }
            catch(Exception ex)
            {
                response = Response<string>.Builder().SetSuccess(false).SetStatusCode((int) HttpStatusCode.InternalServerError).SetMessage(ex.Message);
            }
            return StatusCode((int) response.StatusCode, response);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleAccount.Admin))]
        [HttpPut("change-role-admin/{id}")]
        public async Task<ActionResult<Response<string>>> ChangeRoleAdmin(Guid id)
        {
            var response = new Response<string>();
            try
            {
                await _accountServices.ChangeRoleAdmin(id);
                response = Response<string>.Builder().SetSuccess(true).SetStatusCode((int) HttpStatusCode.OK).SetMessage("Change role successfully");
            }
            catch(Exception ex)
            {
                response = Response<string>.Builder().SetSuccess(false).SetStatusCode((int) HttpStatusCode.InternalServerError).SetMessage(ex.Message);
            }
            return StatusCode((int) response.StatusCode, response);
        }


        [HttpGet("search")]
        public async Task<ActionResult<Response<AccountResponse>>> SearchAccountByUsername([FromQuery] string? username)
        {
            var response = new Response<AccountResponse>();
            try
            {
                var account = await _accountServices.GetAccountByUsername(username);

                response = Response<AccountResponse>.Builder()
                .SetData(account)
                .SetSuccess(true)
                .SetStatusCode((int) HttpStatusCode.OK)
                .SetMessage("Search successfully");
            }
            catch(Exception ex)
            {

                if(ex is NotFoundException || ex is BadRequestException)
                {
                    response = Response<AccountResponse>.Builder()
                    .SetSuccess(false)
                    .SetStatusCode((int) HttpStatusCode.BadRequest)
                    .SetMessage(ex.Message);
                }

                response = Response<AccountResponse>.Builder()
                    .SetSuccess(false)
                    .SetStatusCode((int) HttpStatusCode.InternalServerError)
                    .SetMessage(ex.Message);
            }
            return StatusCode((int) response.StatusCode, response);
        }

        [HttpGet("all")]
        public async Task<ActionResult<Response<IList<AccountResponse>>>> GetAllWithPagination([FromQuery] AccountParameters parameters)
        {
            Response<IList<AccountResponse>> response = null;
            try
            {
                IList<AccountResponse> accounts = await _accountServices.GetAllAccountWithPagination(parameters);

                PagingData metaData = new PagingData
                {
                    CurrentPage = ((PagedList<AccountResponse>) accounts).CurrentPage,
                    PageSize = ((PagedList<AccountResponse>) accounts).PageSize,
                    TotalCount = ((PagedList<AccountResponse>) accounts).TotalCount,
                    TotalPages = ((PagedList<AccountResponse>) accounts).TotalPages,
                    HasNext = ((PagedList<AccountResponse>) accounts).HasNext,
                    HasPrevious = ((PagedList<AccountResponse>) accounts).HasPrevious
                };

                response = ResponseWithPaging<IList<AccountResponse>>.Builder()
                .SetData(accounts)
                .SetPagingData(metaData)
                .SetSuccess(true)
                .SetStatusCode((int) HttpStatusCode.OK);

                return StatusCode((int) response.StatusCode, response);
            }
            catch(Exception ex)
            {

                if(ex is BadRequestException)
                {
                    response = Response<IList<AccountResponse>>.Builder()
                    .SetSuccess(false)
                    .SetStatusCode((int) HttpStatusCode.BadRequest)
                    .SetMessage(ex.Message);

                    return StatusCode((int) response.StatusCode, response);
                }

                response = Response<IList<AccountResponse>>.Builder()
                .SetSuccess(false)
                .SetStatusCode((int) HttpStatusCode.InternalServerError)
                .SetMessage(ex.Message);

                return StatusCode((int) response.StatusCode, response);
            }
        }
    }
}
