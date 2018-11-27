using Funta.Core.Domain.Abstarct.IServices;
using Funta.Core.Domain.Entity.Auth;
using Funta.Core.Domain.Entity.Enums;
using Funta.Core.DTO.User;
using Funta.Core.Infrastructures.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Funta.Core.Jwt.Layer
{
    public interface ITokenStoreService
    {
        Task AddUserTokenAsync(UserToken userToken);
        Task AddUserTokenAsync(ProfileDto user, string refreshToken, string accessToken, string refreshTokenSource);
        Task<bool> IsValidTokenAsync(string accessToken, Guid userId);
        Task DeleteExpiredTokensAsync();
        Task<UserToken> FindTokenAsync(string refreshToken);
        Task DeleteTokenAsync(string refreshToken);
        Task DeleteTokensWithSameRefreshTokenSourceAsync(string refreshTokenIdHashSource);
        Task InvalidateUserTokensAsync(Guid userId);
        Task<(string accessToken, string refreshToken)> CreateJwtTokens(ProfileDto user, string refreshTokenSource);
        Task RevokeUserBearerTokensAsync(string userIdValue, string refreshToken);
    }
    public class TokenStoreService : ITokenStoreService
    {
        #region FIELD
        private readonly ISecurityService _securityService;
        private readonly IRoleServices _roleServices;
        private readonly IUnitOfWorks _uow;
        private readonly DbSet<UserToken> _tokens;
        private readonly IOptionsSnapshot<BearerTokensOptions> _configuration;
        #endregion

        #region CTOR
        public TokenStoreService(
            IRoleServices roleServices,
            IUnitOfWorks uow,
            ISecurityService security,
            IOptionsSnapshot<BearerTokensOptions> configuration)
        {
            _securityService = security;
            _uow = uow;
            _configuration = configuration;
            _tokens = _uow.Set<UserToken>();
            _roleServices = roleServices;
        }
        #endregion

        public async Task AddUserTokenAsync(UserToken userToken)
        {
            if (!_configuration.Value.AllowMultipleLoginsFromTheSameUser)
            {
                await InvalidateUserTokensAsync(userToken.UserKey);
            }
            await DeleteTokensWithSameRefreshTokenSourceAsync(userToken.RefreshTokenIdHash);
            _tokens.Add(userToken);
        }
        public async Task AddUserTokenAsync(ProfileDto user, string refreshToken, string accessToken, string refreshTokenSource)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            UserToken token = new UserToken
            {
                UserKey = user.UserId,
                RefreshTokenIdHash = _securityService.GetSha256Hash(refreshToken),
                RefreshTokenIdHashSource = string.IsNullOrWhiteSpace(refreshTokenSource) ?
                                           null : _securityService.GetSha256Hash(refreshTokenSource),
                AccessTokenHash = _securityService.GetSha256Hash(accessToken),
                RefreshTokenExpiresDateTime = now.AddMinutes(_configuration.Value.RefreshTokenExpirationMinutes),
                AccessTokenExpiresDateTime = now.AddMinutes(_configuration.Value.AccessTokenExpirationMinutes)
            };
            await AddUserTokenAsync(token);
        }
        public async Task DeleteExpiredTokensAsync()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            await _tokens.Where(x => x.RefreshTokenExpiresDateTime < now)
                         .ForEachAsync(userToken =>
                         {
                             _tokens.Remove(userToken);
                         });
        }
        public async Task DeleteTokenAsync(string refreshToken)
        {
            UserToken token = await FindTokenAsync(refreshToken);
            if (token != null)
            {
                _tokens.Remove(token);
            }
        }
        public async Task DeleteTokensWithSameRefreshTokenSourceAsync(string refreshTokenIdHashSource)
        {
            if (string.IsNullOrWhiteSpace(refreshTokenIdHashSource))
            {
                return;
            }
            await _tokens.Where(t => t.RefreshTokenIdHashSource == refreshTokenIdHashSource)
                         .ForEachAsync(userToken =>
                         {
                             _tokens.Remove(userToken);
                         });
        }
        public async Task RevokeUserBearerTokensAsync(string userIdValue, string refreshToken)
        {
            if (!string.IsNullOrWhiteSpace(userIdValue) && Guid.TryParse(userIdValue, out Guid userId))
            {
                if (_configuration.Value.AllowSignoutAllUserActiveClients)
                {
                    await InvalidateUserTokensAsync(userId);
                }
            }

            if (!string.IsNullOrWhiteSpace(refreshToken))
            {
                var refreshTokenIdHashSource = _securityService.GetSha256Hash(refreshToken);
                await DeleteTokensWithSameRefreshTokenSourceAsync(refreshTokenIdHashSource);
            }

            await DeleteExpiredTokensAsync();
        }
        public async Task<UserToken> FindTokenAsync(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                return null;

            string refreshTokenIdHash = _securityService.GetSha256Hash(refreshToken);
            return await _tokens.Include(x => x.User).FirstOrDefaultAsync(x => x.RefreshTokenIdHash == refreshTokenIdHash);
        }
        public async Task InvalidateUserTokensAsync(Guid userId)
        {
            await _tokens.Where(x => x.UserKey == userId)
                          .ForEachAsync(userToken =>
                          {
                              _tokens.Remove(userToken);
                          });
        }
        public async Task<bool> IsValidTokenAsync(string accessToken, Guid userId)
        {
            string accessTokenHash = _securityService.GetSha256Hash(accessToken);
            UserToken userToken = await _tokens.FirstOrDefaultAsync(
                x => x.AccessTokenHash == accessTokenHash && x.UserKey == userId);
            return userToken?.AccessTokenExpiresDateTime >= DateTimeOffset.UtcNow;
        }
        public async Task<(string accessToken, string refreshToken)> CreateJwtTokens(ProfileDto user, string refreshTokenSource)
        {
            string accessToken = await createAccessTokenAsync(user);
            string refreshToken = Guid.NewGuid().ToString().Replace("-", "");
            await AddUserTokenAsync(user, refreshToken, accessToken, refreshTokenSource);
            await _uow.SaveChangesAsync(true);
            return (accessToken, refreshToken);
        }
        private async Task<string> createAccessTokenAsync(ProfileDto user)
        {
            IList<Claim> claims = new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iss, _configuration.Value.Issuer));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()));
            claims.Add(new Claim("UserName", user.Mobile));
            claims.Add(new Claim(ClaimTypes.DateOfBirth, user.BirthDay.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.Name));
            claims.Add(new Claim("Family", user.Family));
            claims.Add(new Claim("City", user.City));
            claims.Add(new Claim("LastActivity", user.LastActivity.ToString()));
            claims.Add(new Claim(ClaimTypes.SerialNumber, user.SerialNumber));

            IList<Roles> roles = await _roleServices.FindUserRolesAsync(user.UserId);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            foreach (Roles role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role.Name));

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Value.Key));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            DateTime now = DateTime.UtcNow;
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration.Value.Issuer,
                audience: _configuration.Value.Audience,
                claims: claims,
                notBefore: now,
                expires: now.AddMinutes(_configuration.Value.AccessTokenExpirationMinutes),
                signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
