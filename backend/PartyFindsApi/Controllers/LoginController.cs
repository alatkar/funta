// <copyright company="PartyFinds LLC">
//   Copyright (c) PartyFinds LLC.  All rights reserved
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PartyFindsApi.core;

namespace PartyFindsApi.Controllers
{
    [ApiController]
    public class LoginController : ControllerBase
    {
        IRepository userRepo;

        public LoginController()
        {
            //_cosmosDbService = cosmosDbService;
            this.userRepo = Container.Instance.listingsRepo;
        }

        //TODO: Token management
        [Route("api/login")]
        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromBody]Models.Account userInput)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await QueryUser(userInput.UserName, userInput.Email);

            if (user == null)
            {
                return NotFound($"{userInput} is not found");
            }

            if (!userInput.PasswordHash.Equals(user.PasswordHash))
            {
                return BadRequest($"Password provided for {user} does not match");
            }

            return Ok(user);
        }
        
        //TODO: Token management
        [Route("api/logout")]
        [HttpPost]
        public async Task<IActionResult> LogoutAsync([FromBody]Models.Account user)
        {
            var registeredUser = await QueryUser(user.UserName, user.Email);

            if (registeredUser == null)
            {
                return NotFound($"{user.UserName} is not found");
            }

            return Ok(registeredUser);
        }

        [Route("api/register")]
        [HttpPost]
        public async Task<IActionResult> RegisterAsync([FromBody]Models.Account user)
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

            IList<Models.User> users = await userRepo.QueryAsync<Models.User>($" c where c.userName = '{user.UserName}'", null);

            if (users != null && users.Count > 0)
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
                Models.Account result = await userRepo.CreateAsync(user, null);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(503, ex);
            }            
        }

        [Route("api/resetpassword")]
        [HttpPost]
        public IActionResult ResetPasswordAsync([FromBody]Models.User user)
        {
            return Ok(new string[] { "ResetPasswordAsync:username", "ResetPasswordAsync:password" });
        }

        // QUery by Username or email
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
                throw new Exception($"More than one users found for Username {userName}");
            }
            else
            {
                throw new Exception($"Unknown exception while querying for Username {userName}");
            }
        }
    }
}
