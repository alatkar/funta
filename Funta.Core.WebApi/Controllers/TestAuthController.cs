using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Funta.Core.Web.Api.Controllers
{
    [Authorize(Policy = "Member")]
    [Produces("application/json")]
    [ApiVersionNeutral]
    [Route("api/v{version:apiVersion}/[controller]")]
    [EnableCors("main-cors")]
    [ApiController]
    public class TestAuthController : Controller
    {
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpGet("[action]")]
        public IActionResult TestLogin()
        {
            var dict = new Dictionary<string, string>();
            HttpContext.User.Claims.ToList().ForEach(item => dict.Add(item.Type, item.Value));
            return Ok(dict);
        }
    }
}
