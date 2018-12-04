using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Funta.Core.Domain.Abstarct.IServices;
using Funta.Core.Domain.Entity.Auth;
using Funta.Core.DTO.User;
using Funta.Core.Infrastructures.DataAccess;
using Funta.Core.Jwt.Layer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Funta.Core.Web.Api.Controllers
{
    public class AuthController : Controller
    {
        #region FIELDS
        private readonly IUnitOfWorks _uow;
        private readonly IRegisterServices _users;
        private readonly ITokenStoreService _tokenService;
        private readonly IOptionsSnapshot<ApiSettingsDto> _apiSettingsConfig;
        #endregion

        public AuthController(IUnitOfWorks uow)
        {
            _uow = uow;
        }

        #region Login
        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]LoginUserDto model)
        {
            Debugger.Break();
            if (model == null || !ModelState.IsValid)
                return BadRequest(model);

            try
            {
                (StatusUserEnum, ProfileDto) status = await _users.Verify(model);

                switch (status.Item1)
                {
                    case StatusUserEnum.LoggeIn:
                        var (accessToken, refreshToken) = await _tokenService.CreateJwtTokens(status.Item2, null).ConfigureAwait(false);
                        return Ok(new { access_token = accessToken, refresh_token = refreshToken });
                    case StatusUserEnum.NotActive:
                    case StatusUserEnum.UserNameNotExist:
                    case StatusUserEnum.PasswordIsWrong:
                    default:
                        return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Debug.WriteLine(ex.Message);
                throw ex;
            }
        }
        #endregion

        #region Refresh Token
        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> RefreshToken([FromBody]JToken jsonBody)
        {
            string refreshToken = jsonBody.Value<string>("refreshToken");
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return BadRequest("refreshToken is not set.");
            }

            UserToken token = await _tokenService.FindTokenAsync(refreshToken);
            if (token == null)
                return Unauthorized();
            ProfileDto profile = new ProfileDto
            {
                BirthDay = token.User.BirthDay,
                City = token.User.City,
                Family = token.User.Family,
                LastActivity = token.User.UpdateDate,
                Mobile = token.User.Mobile,
                Name = token.User.Name,
                SerialNumber = token.User.SerialNumber,
                UserId = token.User.Id
            };
            var (accessToken, newRefreshToken) = await _tokenService.CreateJwtTokens(profile, null).ConfigureAwait(false);
            return Ok(new { access_token = accessToken, refresh_token = newRefreshToken });
        }
        #endregion

        #region Logout
        [Authorize]
        [HttpGet("[action]"), HttpPost("[action]")]
        public async Task<bool> Logout()
        {
            ClaimsIdentity claimsIdentity = User.Identity as ClaimsIdentity;
            string userIdValue = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrWhiteSpace(userIdValue) && Guid.TryParse(userIdValue, out Guid userId))
                await _tokenService.InvalidateUserTokensAsync(userId).ConfigureAwait(false);

            await _tokenService.DeleteExpiredTokensAsync().ConfigureAwait(false);
            await _uow.SaveChangesAsync(true).ConfigureAwait(false);

            return true;
        }
        #endregion

        #region Get Api Setting 
        [AllowAnonymous]
        [HttpGet("[action]")]
        public IActionResult ApiSettings()
        {
            return Json(_apiSettingsConfig.Value, serializerSettings: new Newtonsoft.Json.JsonSerializerSettings
            {
                Formatting = Newtonsoft.Json.Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }
        #endregion
    }
}