using AutoMapper;
using EBird.Api.UserFeatures.Requests;
using EBird.Application.Interfaces;
using EBird.Application.Model;
using EBird.Application.Services.IServices;
using EBird.Domain.Entities;
using EBird.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Response;
using System.Net;
using System.Security.Claims;

namespace EBird.Api.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationServices _authenticationServices;
        private readonly IGenericRepository<AccountEntity> _accountRepository;
        private readonly IGenericRepository<RefreshTokenEntity> _refreshTokenRepository;
        private readonly IMapper _mapper;

        public AuthenticationController(IAuthenticationServices authenticationServices, IGenericRepository<AccountEntity> accountRepository, IGenericRepository<RefreshTokenEntity> refreshTokenRepository, IMapper mapper)
        {
            _authenticationServices = authenticationServices;
            _accountRepository = accountRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _mapper = mapper;
        }

        [HttpPost("login")]

        public async Task<ActionResult<Response<TokenModel>>> Login(LoginRequest req)
        {
            var user = await _accountRepository.FindWithCondition(p => p.Username.Equals(req.Username));
            
            if (user == null)
            {
                return Response<TokenModel>.Builder().SetStatusCode(400).SetMessage("Username is not exist").SetSuccess(false);
            }
            if (!user.Password.Equals(req.Password))
            {
                return Response<TokenModel>.Builder().SetStatusCode(400).SetMessage("Password wrong!").SetSuccess(false);
            }
            var token = await _authenticationServices.CreateToken(user);
            return Response<TokenModel>.Builder().SetStatusCode(200)
                .SetMessage("Login successfully!").SetSuccess(true).SetData(token);

        }
        [HttpGet("logout")]
        public async Task<ActionResult<Response<string>>> Logout()
        {
            string rawId = this.User.FindFirstValue((ClaimTypes.NameIdentifier));
            Guid id = Guid.Parse(rawId);
            
            var refresh = await _refreshTokenRepository.FindWithCondition(p => p.AccountId.Equals(id));
            if (refresh == null)
            {
                return Response<string>.Builder().SetStatusCode(401).SetSuccess(false);
            }
            await _refreshTokenRepository.DeleteAsync(refresh.Id);
            return Response<string>.Builder().SetStatusCode(200).SetSuccess(true);
        }
        [HttpPost("signup")]
        public async Task<ActionResult<Response<string>>> Signup(SignupRequest req)
        {
            var user = _accountRepository.FindWithCondition(p=> p.Username.Equals(req.Username));
            if (user != null) {
                return Response<string>.Builder().SetStatusCode(400).SetSuccess(false).SetMessage("Username is exist!");
            }
            var newUser = new AccountEntity{
                Username = req.Username,
                Password= req.Password,
                CreateDateTime= DateTime.UtcNow,
                Description= req.Description,
                Email= req.Email,
                FirstName= req.FirstName,
                LastName= req.LastName,
                Role = Role.User
            };
            await _accountRepository.CreateAsync(newUser);
            return Response<string>.Builder().SetStatusCode(200).SetSuccess(true);
        }

    }
}
