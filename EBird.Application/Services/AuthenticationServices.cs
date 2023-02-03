using AutoMapper;
using EBird.Application.Interfaces;
using EBird.Application.Interfaces.IRepository;
using EBird.Application.Model;
using EBird.Application.Services.IServices;
using EBird.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Response;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using EBird.Application.Exceptions;

namespace EBird.Application.Services
{
    public class AuthenticationServices : IAuthenticationServices
    {

        private readonly IGenericRepository<RefreshTokenEntity> _refreshTokenRepository;
        private readonly IGenericRepository<AccountEntity> _accountRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthenticationServices(IGenericRepository<RefreshTokenEntity> refreshTokenRepository, IGenericRepository<AccountEntity> accountRepository, IConfiguration configuration, IMapper mapper)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _accountRepository = accountRepository;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task< TokenModel> CreateToken(AccountEntity account)
        {
            var claims = new List<Claim>
             {
                 new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                 new Claim("id", account.Id.ToString()),
                 new Claim(ClaimTypes.Name, account.FirstName),
                 new Claim(ClaimTypes.Role, account.RoleString),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
             };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("JwtSetting:IssuerSigningKey").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = System.DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(token);
            var refreshToken = GenerateRefreshToken();
            await _refreshTokenRepository.CreateAsync(
                new RefreshTokenEntity
                {
                    JwtId = token.Id,
                    AccountId = account.Id,
                    Token = refreshToken,
                    IsUsed = false,
                    IsRevoked = false,
                    IssuedAt = DateTime.UtcNow,
                    ExpiredAt = DateTime.UtcNow.AddDays(7)
                }
            );
            return new TokenModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };

        }
        private string GenerateRefreshToken()
        {
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }
        public string HashPassword(string password)
        {
            var sha = SHA256.Create();
            var asByteArrray = Encoding.Default.GetBytes(password);
            var hasedPassword = sha.ComputeHash(asByteArrray);
            return Convert.ToBase64String(hasedPassword);
        }
        public async Task<TokenModel> Login(string username, string password)
        {
            var user = await _accountRepository.FindWithCondition(p => p.Username.Equals(username));

            
            if (user == null)
            {
               throw new NotFoundException("User not found!");
            }
            if (user.IsDeleted)
            {
                throw new NotFoundException("User is deleted!");
            }
            if (!user.Password.Equals(HashPassword(password)))
            {
               throw new BadRequestException("Password is incorrect!");
            }
            var token = await CreateToken(user);
            return token;
        }
        public async Task Logout(Guid id)
        {
            var refreshToken = await _refreshTokenRepository.FindWithCondition(p=>p.AccountId.Equals(id));  
            await _refreshTokenRepository.DeleteAsync(refreshToken.Id);
            
        }
        public async Task Signup(AccountEntity req)
        {
            var user = await _accountRepository.FindWithCondition(p => p.Username.Equals(req.Username));
            if (user != null)
            {
                throw new BadRequestException(String.Format("Username {0} is already taken", req.Username));
            }
            req.Password = HashPassword(req.Password);
            await _accountRepository.CreateAsync(req);
        }
        public async Task<TokenModel> RenewToken(TokenModel model)
        {
            var refreshToken = await _refreshTokenRepository.FindWithCondition(p => p.Token.Equals(model.RefreshToken));
            if (refreshToken == null)
            {
                throw new BadRequestException("Invalid Token");

            }
            if (refreshToken.ExpiredAt < DateTime.Now)
            {
                throw new BadRequestException("Token has expired");
            }
            if (refreshToken.IsUsed || refreshToken.IsRevoked)
            {
               throw new BadRequestException("Invalid Token");
            }

            var user = await _accountRepository.GetByIdAsync(refreshToken.AccountId);
            refreshToken.IsRevoked = true;
            refreshToken.IsUsed = true;
            await _refreshTokenRepository.UpdateAsync(refreshToken);
            var token = await CreateToken(user);
            return token;
        }
        public async Task<AccountResponse> GetAccountById(Guid id)
        {
            var account = await _accountRepository.GetByIdActiveAsync(id);

            if (account == null)
            {
                throw new NotFoundException("Account not found");
            }
            var accountResponse = new AccountResponse();
            _mapper.Map<AccountEntity, AccountResponse>(account, accountResponse);

            return accountResponse;
        }


    }
}
