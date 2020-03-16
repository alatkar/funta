// <copyright company="PartyFinds LLC">
//   Copyright (c) PartyFinds LLC.  All rights reserved
// </copyright>

using System;
using System.Threading.Tasks;
using JsonApiSerializer;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PartyFindsApi.core;
using User = PartyFindsApi.Models.User;

namespace PartyFindsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger logger;
        IRepository userRepo;

        public UsersController(ILogger<UsersController> logger)
        {
            this.logger = logger;
            //_cosmosDbService = cosmosDbService;
            this.userRepo = Container.Instance.userRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var feed = new FeedOptions();
            feed.EnableCrossPartitionQuery = true;

            try
            {
                logger.LogInformation($"Getting Users with query {this.Request.Query} queryString {this.Request.QueryString}");
                var resp = await userRepo.QueryAsync<User>("", feed).ConfigureAwait(false);
                return Ok(JsonConvert.SerializeObject(resp, new JsonApiSerializerSettings()));
            }
            catch (Exception ex)
            {
                logger.LogError($"Received exception {ex}");
                return StatusCode(500, ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var feed = new RequestOptions { PartitionKey = new PartitionKey(id) };
            //feed.EnableCrossPartitionQuery = true;

            try
            {
                logger.LogInformation($"Getting Users with id {id}");

                var resp = await userRepo.GetAsync<User>(id, feed).ConfigureAwait(false);
                if (resp == null)
                {
                    return NotFound();
                }

                return Ok(JsonConvert.SerializeObject(resp, new JsonApiSerializerSettings()));
            }
            catch (Exception ex)
            {
                logger.LogError($"Received exception {ex}");
                return StatusCode(500, ex);
            }
        }

        [HttpPatch]
        public async Task<IActionResult> PatchAsync(string id, [FromBody]JsonPatchDocument<Models.User> patchDoc)
        {
            try
            {
                var user = await userRepo.GetAsync<Models.User>(
                    id, 
                    new RequestOptions { PartitionKey = new PartitionKey(id) })
                    .ConfigureAwait(false);

                if (user == null)
                {
                    return NotFound($"The user with id {id} is not found");
                }

                patchDoc.ApplyTo(user, ModelState);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await userRepo.UpdateAsync(
                    user, new RequestOptions{ PartitionKey = new PartitionKey(user.Id) }).ConfigureAwait(false);
                //Models.User respUser = (dynamic)result;
                return Ok(JsonConvert.SerializeObject((dynamic)result, new JsonApiSerializerSettings()));
            }
            catch (DocumentClientException ex)
            {
                Exception baseException = ex.GetBaseException();
                string msg = $"{ex.StatusCode} error occurred: {ex.Message}, Message: {baseException.Message}";
                logger.LogError($"Received exception {ex}");
                return StatusCode(503, msg);
            }
            catch (Exception ex)
            {
                logger.LogError($"Received exception {ex}");
                Exception baseException = ex.GetBaseException();
                string msg = $"Error: {ex.Message}, Message: {baseException.Message}";
                return StatusCode(500, msg);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            try
            {
                // TODO: Check if user exists
                logger.LogInformation($"Deleting user with id {id}");
                await userRepo.DeleteAsync(
                    id,
                    new RequestOptions { PartitionKey = new PartitionKey(id) })
                    .ConfigureAwait(false);

                logger.LogInformation($"Deleted user with id {id}");
                return Ok();
            }
            catch (DocumentClientException ex)
            {
                Exception baseException = ex.GetBaseException();
                string msg = $"Error: {ex.Message}, Message: {baseException.Message}";
                logger.LogError($"Received error {msg}");
                return NotFound(msg);
            }
            catch (Exception ex)
            {
                Exception baseException = ex.GetBaseException();
                string msg = $"Error: {ex.Message}, Message: {baseException.Message}";
                logger.LogError($"Received error {msg}");
                return StatusCode(500, msg);
            }
        }
    }
}
