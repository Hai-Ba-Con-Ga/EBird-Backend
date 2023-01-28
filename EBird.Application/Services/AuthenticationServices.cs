using EBird.Application.Interfaces;
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

namespace EBird.Application.Services
{
    public class AuthenticationServices : IAuthenticationServices
    {

        private readonly IGenericRepository<RefreshTokenEntity> _refreshTokenRepository;
        private readonly IGenericRepository<AccountEntity> _accountRepository;
        private readonly IConfiguration _configuration;

        public AuthenticationServices(IGenericRepository<RefreshTokenEntity> refreshTokenRepository, IGenericRepository<AccountEntity> accountRepository, IConfiguration configuration)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _accountRepository = accountRepository;
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
        public string HashPassword(string password)
        {
            var sha = SHA256.Create();
            var asByteArrray = Encoding.Default.GetBytes(password);
            var hasedPassword = sha.ComputeHash(asByteArrray);
            return Convert.ToBase64String(hasedPassword);
        }
        public async Task<ActionResult<Response<TokenModel>>> Login(string username, string password)
        {
            var user = await _accountRepository.FindWithCondition(p => p.Username.Equals(username));

            
            if (user == null)
            {
                return Response<TokenModel>.Builder().SetStatusCode((int)HttpStatusCode.BadRequest).SetMessage("Username is not exist").SetSuccess(false);
            }
            if (user.IsDeleted)
            {
                return Response<TokenModel>.Builder().SetStatusCode((int)HttpStatusCode.Forbidden).SetMessage("The account has been deleted!").SetSuccess(false);

            }
            if (!user.Password.Equals(HashPassword(password)))
            {
                return Response<TokenModel>.Builder().SetStatusCode((int)HttpStatusCode.BadRequest).SetMessage("Password wrong!").SetSuccess(false);
            }
            var token = await CreateToken(user);
            return Response<TokenModel>.Builder().SetStatusCode((int)HttpStatusCode.OK)
                .SetMessage("Login successfully!").SetSuccess(true).SetData(token);

        }
        public async Task<ActionResult<Response<string>>> Logout(Guid id)
        {
            var refreshToken = await _refreshTokenRepository.FindWithCondition(p=>p.AccountId.Equals(id));  
            await _refreshTokenRepository.DeleteAsync(refreshToken.Id);
            return Response<string>.Builder().SetStatusCode((int)HttpStatusCode.OK).SetSuccess(true);
        }
        public async Task<ActionResult<Response<string>>> Signup(AccountEntity req)
        {
            var user = await _accountRepository.FindWithCondition(p => p.Username.Equals(req.Username));
            if (user != null)
            {
                return Response<string>.Builder().SetStatusCode((int)HttpStatusCode.BadRequest).SetSuccess(false).SetMessage("Username is exist!");
            }
            req.Password = HashPassword(req.Password);
            await _accountRepository.CreateAsync(req);

            return Response<string>.Builder().SetStatusCode((int)HttpStatusCode.OK).SetSuccess(true);
        }
        public async Task<ActionResult<Response<TokenModel>>> RenewToken(TokenModel model)
        {
            var refreshToken = await _refreshTokenRepository.FindWithCondition(p => p.Token.Equals(model.RefreshToken));
            if (refreshToken == null)
            {
                return Response<TokenModel>.Builder().SetStatusCode((int)HttpStatusCode.BadRequest).SetSuccess(false).SetMessage("Invalid Token");

            }
            if (refreshToken.ExpiredAt < DateTime.Now)
            {
                return Response<TokenModel>.Builder().SetStatusCode((int)HttpStatusCode.BadRequest).SetSuccess(false).SetMessage("Token expried");
            }
            if (refreshToken.IsUsed || refreshToken.IsRevoked)
            {
                return Response<TokenModel>.Builder().SetStatusCode((int)HttpStatusCode.BadRequest).SetSuccess(false).SetMessage("Invalid Token");

            }

            var user = await _accountRepository.GetByIdAsync(refreshToken.AccountId);
            refreshToken.IsRevoked = true;
            refreshToken.IsUsed = true;
            await _refreshTokenRepository.UpdateAsync(refreshToken);
            var token = await CreateToken(user);
            return Response<TokenModel>.Builder().SetStatusCode((int)HttpStatusCode.OK).SetSuccess(true).SetData(token);
        }


    }
}
