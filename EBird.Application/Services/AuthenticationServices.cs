﻿using AutoMapper;
using EBird.Application.Interfaces;
using EBird.Application.Interfaces.IRepository;
using EBird.Application.Model.Auth;
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
using Google.Apis.Auth;
using Newtonsoft.Json;
using System.Text.Json;
using EBird.Application.Model;

namespace EBird.Application.Services
{
    public class AuthenticationServices : IAuthenticationServices
    {

        private readonly IGenericRepository<RefreshTokenEntity> _refreshTokenRepository;
        private readonly IGenericRepository<AccountEntity> _accountRepository;
        private readonly IGenericRepository<VipRegistrationEntity> _vipRegistrationRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthenticationServices(IGenericRepository<RefreshTokenEntity> refreshTokenRepository,
            IGenericRepository<AccountEntity> accountRepository,
            IGenericRepository<VipRegistrationEntity> vipRegistrationRepository,
            IConfiguration configuration,
            IMapper mapper)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _accountRepository = accountRepository;
            _vipRegistrationRepository = vipRegistrationRepository;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<TokenModel> CreateToken(AccountEntity account)
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
            var refreshToken = await _refreshTokenRepository.FindWithCondition(p => p.AccountId.Equals(id));
            await _refreshTokenRepository.DeleteAsync(refreshToken.Id);

        }
        public async Task Signup(AccountEntity req)
        {
            var user = await _accountRepository.FindWithCondition(p => p.Username.Equals(req.Username) || p.Email.Equals(req.Email));
            if (user != null)
            {
                if (req.Username.Equals(user.Username))
                {
                    throw new BadRequestException(String.Format("Username {0} is already taken", req.Username));
                }
                if (req.Email.Equals(user.Email))
                {
                    throw new BadRequestException(String.Format("Email {0} is already taken", req.Email));
                }
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
            var vipList = await _vipRegistrationRepository.FindAllWithCondition(p => p.AccountId.Equals(id));
            if (vipList.Count() > 0)
            {
                foreach (var item in vipList)
                {
                    if (item.ExpiredDate > DateTime.Now) //check expired date of vip
                    {
                        accountResponse.Vip = _mapper.Map<VipRegistrationEntity, VipResponse>(item);
                        break;
                    }
                }
            }
            accountResponse = _mapper.Map<AccountEntity, AccountResponse>(account, accountResponse);
            // accountResponse.Vip = _mapper.Map<VipRegistrationEntity, VipResponse>(vip);
            return accountResponse;
        }

        public async Task<TokenModel> LoginWithGoogle(string googleToken)
        {
            var client = new GoogleJsonWebSignature.Payload();


            client = GoogleJsonWebSignature.ValidateAsync(googleToken, new GoogleJsonWebSignature.ValidationSettings()).Result;
            // string json = System.Text.Json.JsonSerializer.Serialize(client);
            Console.WriteLine(client.Email);
            var account = await _accountRepository.FindWithCondition(p => p.Email.Equals(client.Email));

            var token = new TokenModel();
            if (account == null)
            {
                var newAccount = new AccountEntity()
                {
                    Email = client.Email,
                    FirstName = client.GivenName,
                    LastName = client.FamilyName
                };
                await _accountRepository.CreateAsync(newAccount);
                token = await CreateToken(newAccount);
                return token;
            }
            if (account.Username == null)
            {
                throw new BadRequestException("The account invalid!!");
            }
            token = await CreateToken(account);
            return token;
        }


    }
}
