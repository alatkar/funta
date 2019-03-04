using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using core;
using core.repository;
using Microsoft.AspNetCore.Mvc;

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

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            throw new NotSupportedException($"User with id {id} can not be deleted");
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

        //TODO: Register Action should be used rather than this controller. 
        // Set correct permissions so it can be only called by admin or something like that
        // POST api/<controller>
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody]Models.User user)
        {
            if (string.IsNullOrEmpty(user.UserName))
            {
                return BadRequest($"Username or Email not provided");
            }

            if (string.IsNullOrEmpty(user.Email))
            {
                return BadRequest($"Username or Email not provided");
            }

            if (string.IsNullOrEmpty(user.PasswordHash))
            {
                return BadRequest($"Password not provided");
            }

            if (string.IsNullOrEmpty(user.Address.ZipCode))
            {
                return BadRequest($"ZipCode not provided");
            }

            IList<Models.User> users = await userRepo.QueryAsync<Models.User>($" c where c.userName = '{user.UserName}'", null);

            if(users != null && users.Count > 0)
            {
                return BadRequest($"User {user.UserName} already exists");
            }

            users = await userRepo.QueryAsync<Models.User>($" c where c.email = '{user.Email}'", null);

            if (users != null && users.Count > 0)
            {
                return BadRequest($"Email {user.Email} already registered");
            }

            try
            {
                Models.User result = await userRepo.CreateAsync(user, null);
                return Ok(result);
            }
            catch(Exception ex)
            {
                int i = 0;
                i++;
                throw ex;
            }
        }

        //TODO: Support partial updated. Generate PATCH and check what can not be changed
        // POST api/<controller>
        [HttpPatch]
        public async Task<ActionResult> PatchAsync([FromBody]Models.User inputUser)
        {
            IList<Models.User> users = await userRepo.QueryAsync<Models.User>($" c where c.userName = '{inputUser.UserName}'", null);

            if (users == null && users.Count == 0)
            {
                return BadRequest($"User {inputUser.UserName} does not exists");
            }

            if (users.Count > 0)
            {
                return BadRequest($"More than one users exist for User {inputUser.UserName}");
            }

            if (users.Count > 1)
            {
                return BadRequest($"More than one users exist for User {inputUser.UserName}");
            }

            Models.User existingUser = users.FirstOrDefault();

            if (inputUser.Email != null && !inputUser.Email.Equals(existingUser.Email))
            {
                return BadRequest($"Email can not be updated. for User {inputUser.UserName}");
            }
            
            // Sanitize the request
            inputUser.Id = users[0].Id;
            inputUser.DogProfiles.ForEach(e => e.ProfileType = Models.Profile.ProfileType.DOG);
            inputUser.NonProfitProfiles.ForEach(e => e.ProfileType = Models.Profile.ProfileType.NONPROFIT);
            inputUser.ProductProfiles.ForEach(e => e.ProfileType = Models.Profile.ProfileType.PRODUCT);
            inputUser.ServiceProfiles.ForEach(e => e.ProfileType = Models.Profile.ProfileType.SERVICE);
            inputUser.ShelterProfiles.ForEach(e => e.ProfileType = Models.Profile.ProfileType.SHELTER);

            Models.User result = await userRepo.UpdateAsync(inputUser, null);
            return Ok(result);
        }       

        async Task<Models.User> QueryUser(string userName, string email)
        {
            string filter = "";
            IList<Models.User> users = null;
            if (!string.IsNullOrWhiteSpace(userName))
            {
                filter = $" c where c.userName = '{userName}'";
                users = await userRepo.QueryAsync<Models.User>(filter, null);
            }

            if (users == null || users.Count == 0)
            {
                filter = "";
                if (!string.IsNullOrWhiteSpace(email))
                {
                    filter = $" c where c.email = '{email}'";
                    users = await userRepo.QueryAsync<Models.User>(filter, null);
                }
            }

            if (users == null || users.Count == 0)
            {
                return null;
            }
            else if (users.Count == 1)
            {
                return users.FirstOrDefault();
            }
            else if (users.Count > 1)
            {
                throw new Exception($"More than one users found for Username {userName} or Emai {email}");
            }
            else
            {
                throw new Exception($"Unknown exception while querying for Username {userName} or Emai {email}");
            }
        }
    }
}
