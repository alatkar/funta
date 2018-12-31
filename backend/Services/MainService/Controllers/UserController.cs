using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using core;
using core.repository;
using MainService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MainService.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        IRepository userRepo;

        public UserController()
        {
            this.userRepo = Container.Instance.userRepo;
        }

        // GET: api/<controller>
        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            string filter = "";

            // Filter Processing
            string userName = this.Request.Query["userName"];
            if(!string.IsNullOrWhiteSpace(userName))
            {
                filter = $" c where c.userName = '{userName}'";
            }

            IList <Models.User> users = await userRepo.QueryAsync<Models.User>(filter, null);
            if (users == null)
            {
                return NotFound();
            }
            return Ok(Json(users));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetAsync(string id)
        {
            var user = await userRepo.GetAsync<Models.User>(id, null);
            return Ok(user);
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody]Models.User user)
        {
            
            IList<Models.User> users = await userRepo.QueryAsync<Models.User>($" c where c.userName = '{user.UserName}'", null);

            if(users != null && users.Count > 0)
            {
                return BadRequest($"User {user.UserName} already exists");
            }

            Models.User result = await userRepo.CreateAsync(user, null);
            return Ok(result);            
        }

        // POST api/<controller>
        [HttpPatch]
        public async Task<ActionResult> PatchAsync([FromBody]Models.User user)
        {

            IList<Models.User> users = await userRepo.QueryAsync<Models.User>($" c where c.userName = '{user.UserName}'", null);

            if (users == null && users.Count == 0)
            {
                return BadRequest($"User {user.UserName} does not exists");
            }

            // Sanitize the request
            user.Id = users[0].Id;
            user.DogProfiles.ForEach(e => e.ProfileType = Models.Profile.ProfileType.DOG);
            user.NonProfitProfiles.ForEach(e => e.ProfileType = Models.Profile.ProfileType.NONPROFIT);
            user.ProductProfiles.ForEach(e => e.ProfileType = Models.Profile.ProfileType.PRODUCT);
            user.ServiceProfiles.ForEach(e => e.ProfileType = Models.Profile.ProfileType.SERVICE);
            user.ShelterProfiles.ForEach(e => e.ProfileType = Models.Profile.ProfileType.SHELTER);

            Models.User result = await userRepo.UpdateAsync(user, null);
            return Ok(result);
        }


        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
