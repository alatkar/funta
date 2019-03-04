using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using core;
using core.repository;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MainService.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        IRepository userRepo;
        UserController userController;

        public AccountController(UserController userController)
        {
            this.userRepo = Container.Instance.userRepo;
            this.userController = userController;
        }

        [HttpPost]
        public async Task<ActionResult> LoginAsync([FromBody]Contracts.Login userInput)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }            

            var user = await QueryUser(userInput.UserName);            

            if (user == null)
            {
                return NotFound($"{userInput} is not found");
            }

            if (!userInput.PasswordHash.Equals(user.PasswordHash))
            {
                return BadRequest($"Password provided for {user} does not match");
            }

            return Ok(Json(user));            
        }

        [HttpPost]
        public async Task<ActionResult> LogoutAsync(string userName)
        {
            var user = await QueryUser(userName);

            if (user == null)
            {
                return NotFound($"{userName} is not found");
            }

            //Invalidate Token and Signoff user

            return Ok();
        }

        /// <summary>
        /// This API is short cut for UserController's POST Method.
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> RegisterAsync([FromBody]Contracts.Register userInput)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var res = await this.userController.PostAsync(ToUser(userInput));

            return Ok(Json(res));
        }

        static Models.User ToUser(Contracts.Register userInput)
        {
            return new Models.User
            {
                Address = new Models.Address
                {
                    ZipCode = userInput.ZipCode,
                },
                Email = userInput.Email,
                UserName = userInput.UserName,
                PasswordHash = userInput.PasswordHash,
                PhoneNumber = userInput.PhoneNumber,               
            };
        }

        async Task<Models.User> QueryUser(string userName /*string email*/)
        {
            string filter = "";
            IList<Models.User> users = null;
            if (!string.IsNullOrWhiteSpace(userName))
            {
                filter = $" c where c.userName = '{userName}'";
                users = await userRepo.QueryAsync<Models.User>(filter, null);
            }
            /*
            if (users == null || users.Count == 0)
            {
                filter = "";
                if (!string.IsNullOrWhiteSpace(email))
                {
                    filter = $" c where c.email = '{email}'";
                    users = await userRepo.QueryAsync<Models.User>(filter, null);
                }
            }*/

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