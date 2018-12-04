using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Funta.Core.Domain.Abstarct.IServices;
using Funta.Core.DTO.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Funta.Core.Web.Api.Controllers
{
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[Controller]")]
    [EnableCors("main-cors")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        #region FIELDS
        private readonly IRegisterServices _users;
        #endregion

        #region CTOR
        public UsersController(IRegisterServices register)
        {
            _users = register;
        }
        #endregion

        #region Register
        [AllowAnonymous]
        [ResponseCache(CacheProfileName = "none")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody]RegisterUserDto model)
        {
            if (model == null)
                return BadRequest(ModelState);
            if (ModelState.IsValid)
            {
                var status = await _users.InsertAsync(model);
                return Created("", model);
            }
            return BadRequest(ModelState);
        }
        #endregion


        #region Get User Info
        [Authorize]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetUserInfo()
        {
            ClaimsIdentity claimsIdentity = this.User.Identity as ClaimsIdentity;
            string userIdValue = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (claimsIdentity == null || userIdValue == null || !Guid.TryParse(userIdValue, out Guid id))
                return Unauthorized();

            try
            {
                RegisterUserDto model = await _users.FindAsync(id);
                if (model == null)
                    return StatusCode(204);
                model.Id = default(Guid);
                model.Password = string.Empty;
                model.SaltForHashing = string.Empty;
                return Ok(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Debug.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }

        }
        #endregion


    }
}