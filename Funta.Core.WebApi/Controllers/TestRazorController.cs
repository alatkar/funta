using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Funta.Core.Web.Api.Controllers
{
    public class TestRazorController : Controller
    {
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        // GET: TestRazor
        public ActionResult Index()
        {
            var ff = HttpContext.User.Identity.Name;
            return View();
        }
    }
}