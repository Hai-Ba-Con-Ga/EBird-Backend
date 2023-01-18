using Duende.IdentityServer.Models;
using EBird.Application.Exceptions;
using EBird.Application.Interfaces;
using EBird.Application.Model;
using EBird.Application.Services.IServices;
using EBird.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace EBird.Application.Services
{
    public class AuthenticationServices : IAuthenticationServices
    {

        private readonly IGenericRepository<RefreshTokenEntity> _refreshTokenRepository;
        private readonly IGenericRepository<AccountEntity> _accountRepository;
        private readonly IConfiguration _configuration;

        public AuthenticationServices(IGenericRepository<RefreshTokenEntity> refreshTokenRepository, IConfiguration configuration)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _configuration = configuration;
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
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
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
                    ExpiredAt = DateTime.UtcNow.AddDays(1)
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
         
        
    }
}
