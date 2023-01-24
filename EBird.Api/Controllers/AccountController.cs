using AutoMapper;
using EBird.Api.UserFeatures.Requests;
using EBird.Application.Interfaces;
using EBird.Application.Services.IServices;
using EBird.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Response;

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
        public async Task<ActionResult<Response<AccountEntity>>> GetAccountById(Guid id)
        {
            var response = await _accountServices.GetAccountById(id);
            return StatusCode((int)response.StatusCode, response);
        }
        [HttpGet("find-all")]
        public async Task<ActionResult<Response<string>>> GetAllAccount()
        {
            var response = await _accountServices.GetAllAccount();
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPatch("update-account/{id}")]
        public async Task<ActionResult<Response<string>>> UpdateAccount(Guid id, [FromBody] UpdateAccountRequest updateAccount)
        {
            var acc = await _accountRepository.GetByIdAsync(id);
            var response = await _accountServices.UpdateAccount(_mapper.Map<UpdateAccountRequest, AccountEntity>(updateAccount, acc));
            return StatusCode((int)response.StatusCode, response);
        }
        [HttpDelete("delete-account/{id}")]
        public async Task<ActionResult<Response<string>>> DeleteAccount(Guid id)
        {
            var response = await _accountServices.DeleteAccount(id);
            return StatusCode((int)response.StatusCode, response);
        }

    }
}
