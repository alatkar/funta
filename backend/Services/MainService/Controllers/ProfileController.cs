using core;
using core.repository;
using MainService.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MainService.Controllers
{
    [Route("api/user/{userName}/[controller]")]
    public class ProfileController : Controller
    {
        IRepository userRepo;

        public ProfileController()
        {
            this.userRepo = Container.Instance.userRepo;
        }

        [HttpGet]
        [HttpGet("{id}")] // This can be profile name or ID
        public async Task<ActionResult> Get(string userName, string id)
        {
            // TODO: Need to search the name Case Insensitive
            IList<Models.User> users = await userRepo.QueryAsync<Models.User>($" c where c.userName = '{userName}'", null);

            if (users == null && users.Count == 0)
            {
                return BadRequest($"User {userName} does not exists");
            }

            if (users != null && users.Count > 1)
            {
                return BadRequest($"More than 1 {userName} exists. Count is {users.Count}");
            }

            // Check Profile Name and Profile Id
            var profile = users[0].DogProfiles.Find(e => e.ProfileName.Equals(id, StringComparison.OrdinalIgnoreCase));
            if (profile == null)
            {
                profile = users[0].DogProfiles.Find(e => e.ProfileId.Equals(id, StringComparison.OrdinalIgnoreCase));
            }

            if (profile == null)
            {
                return BadRequest($"Profile {id} for User {userName} does not exists");
            }

            return Ok(profile);
        }
    }
}
