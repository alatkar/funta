// <copyright company="PartyFinds LLC">
//   Copyright (c) PartyFinds LLC.  All rights reserved
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JsonApiSerializer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PartyFindsApi.core;

namespace PartyFindsApi.Controllers
{
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogger logger;
        IRepository userRepo;

        public LoginController(ILogger<LoginController> logger)
        {
            this.logger = logger;
            //_cosmosDbService = cosmosDbService;
            this.userRepo = Container.Instance.userRepo;
        }

        //TODO: Token management
        [Route("api/login")]
        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromBody]Models.Account userInput)
        {
            logger.LogInformation($"Logging in user {userInput.Email}");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await QueryUser(userInput?.UserName, userInput.Email).ConfigureAwait(false);

            if (user == null)
            {
                return NotFound($"{userInput.Email} or {userInput.UserName} is not found");
            }

            if (!userInput.PasswordHash.Equals(user.PasswordHash, StringComparison.InvariantCultureIgnoreCase)) // TODO: Check culture invariant
            {
                return BadRequest($"Password provided for {user} does not match");
            }
            logger.LogInformation($"User {userInput.Email} logged in");

            return Ok(JsonConvert.SerializeObject(user, new JsonApiSerializerSettings()));
        }
        
        //TODO: Token management
        [Route("api/logout")]
        [HttpPost]
        public async Task<IActionResult> LogoutAsync([FromBody]Models.Account user)
        {
            var registeredUser = await QueryUser(user?.UserName, user.Email).ConfigureAwait(false);

            if (registeredUser == null)
            {
                return NotFound($"{user.UserName} is not found");
            }
            logger.LogInformation($"User {user.Email} logged out");

            return Ok(JsonConvert.SerializeObject(registeredUser, new JsonApiSerializerSettings()));
        }

        [Route("api/register")]
        [HttpPost]
        public async Task<IActionResult> RegisterAsync([FromBody]Models.Account user)
        {
            logger.LogInformation($"Registering user {user?.Email}");

            if (string.IsNullOrEmpty(user?.Email))
            {
                return BadRequest($"Email not provided");
            }

            if (string.IsNullOrEmpty(user.PasswordHash))
            {
                return BadRequest($"Password not provided");
            }

            IList<Models.User> users = null;
            var feedOptions = new FeedOptions { EnableCrossPartitionQuery = true };

            if (!string.IsNullOrEmpty(user.UserName))
            {
                users = await userRepo.QueryAsync<Models.User>($" where C.userName = '{user.UserName}'", feedOptions).ConfigureAwait(false);

                if (users != null && users.Count > 0)
                {
                    return BadRequest($"User {user.UserName} already exists");
                }
            }

            users = await userRepo.QueryAsync<Models.User>($" where C.email = '{user.Email}'", feedOptions).ConfigureAwait(false);

            if (users != null && users.Count > 0)
            {
                return BadRequest($"Email {user.Email} already registered");
            }

            try
            {
                Models.Account result = await userRepo.CreateAsync(user, null).ConfigureAwait(false);
                logger.LogInformation($"User {user?.Email} registered");

                return Ok(JsonConvert.SerializeObject(result, new JsonApiSerializerSettings()));
            }
            catch (Exception ex)
            {
                logger.LogError($"User {user?.Email} registration failed with exception {ex}");
                return StatusCode(500, ex);
            }
        }

        [Route("api/resetpassword")]
        [HttpPost]
        public IActionResult ResetPasswordAsync([FromBody]Models.User user)
        {
            return Ok(new string[] { "ResetPasswordAsync:username", "ResetPasswordAsync:password" });
        }

        [Route("api/confirmemail")]
        [HttpPost]
        public IActionResult ConfirmEmailAsync([FromBody]Models.User user)
        {
            return Ok(new string[] { "ResetPasswordAsync:username", "ResetPasswordAsync:password" });
        }

        // Query by Username or email
        async Task<Models.User> QueryUser(string userName, string email)
        {
            var feed = new FeedOptions();
            feed.EnableCrossPartitionQuery = true;

            string filter = "";
            IList<Models.User> users = null;
            if (!string.IsNullOrWhiteSpace(userName))
            {
                filter = $" c where c.userName = '{userName}'";
                users = await userRepo.QueryAsync<Models.User>(filter, feed).ConfigureAwait(false);
            }
            
            if (users == null || users.Count == 0)
            {
                filter = "";
                if (!string.IsNullOrWhiteSpace(email))
                {
                    filter = $" where C.email = '{email}'";
                    users = await userRepo.QueryAsync<Models.User>(filter, feed).ConfigureAwait(false);
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
