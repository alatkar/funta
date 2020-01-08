﻿// <copyright company="PartyFinds LLC">
//   Copyright (c) PartyFinds LLC.  All rights reserved
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;
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
            this.userRepo = Container.Instance.userRepo;
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
                return NotFound($"{userInput.Email} or {userInput.UserName} is not found");
            }

            if (!userInput.Password.Equals(user.Password))
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
            if (string.IsNullOrEmpty(user.Email))
            {
                return BadRequest($"Email not provided");
            }

            if (string.IsNullOrEmpty(user.Password))
            {
                return BadRequest($"Password not provided");
            }

            IList<Models.User> users = null;

            var feed = new FeedOptions();
            feed.EnableCrossPartitionQuery = true;

            if (!string.IsNullOrEmpty(user.UserName))
            {
                users = await userRepo.QueryAsync<Models.User>($" where C.userName = '{user.UserName}'", feed);

                if (users != null && users.Count > 0)
                {
                    return BadRequest($"User {user.UserName} already exists");
                }
            }            

            users = await userRepo.QueryAsync<Models.User>($" where C.email = '{user.Email}'", feed);

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
                users = await userRepo.QueryAsync<Models.User>(filter, feed);
            }
            
            if (users == null || users.Count == 0)
            {
                filter = "";
                if (!string.IsNullOrWhiteSpace(email))
                {
                    filter = $" where C.email = '{email}'";
                    users = await userRepo.QueryAsync<Models.User>(filter, feed);
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
