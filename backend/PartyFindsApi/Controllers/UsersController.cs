// <copyright company="PartyFinds LLC">
//   Copyright (c) PartyFinds LLC.  All rights reserved
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JsonApiSerializer;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using PartyFindsApi.core;
using PartyFindsApi.Models;
using User = PartyFindsApi.Models.User;

namespace PartyFindsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        IRepository userRepo;

        public UsersController()
        {
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
                var resp = await userRepo.QueryAsync<User>("", feed);
                return Ok(JsonConvert.SerializeObject(resp, new JsonApiSerializerSettings()));
            }
            catch (Exception ex)
            {
                return StatusCode(503, ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var feed = new FeedOptions();
            feed.EnableCrossPartitionQuery = true;

            try
            {
                var resp = await userRepo.QueryAsync<User>($" where C.id = '{id}'", feed);
                if (resp == null || resp.Count == 0)
                {
                    return NotFound();
                }

                return Ok(JsonConvert.SerializeObject(resp.First(), new JsonApiSerializerSettings()));
            }
            catch (Exception ex)
            {
                return StatusCode(503, ex);
            }
        }

        [HttpPatch]
        public async Task<IActionResult> PatchAsync(string id, [FromBody]JsonPatchDocument<Models.User> patchDoc)
        {
            try
            {
                var resp = await userRepo.QueryAsync<Models.User>($" where C.id = '{id}'", new FeedOptions { PartitionKey = new PartitionKey(id) });

                var user = resp.FirstOrDefault<Models.User>();

                if (user == null)
                {
                    return NotFound($"The user with id {id} is not found");
                }

                patchDoc.ApplyTo(user, ModelState);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await userRepo.UpdateAsync(user, new RequestOptions{ PartitionKey = new PartitionKey(user.Id) });
                //Models.User respUser = (dynamic)result;
                return Ok(JsonConvert.SerializeObject((dynamic)result, new JsonApiSerializerSettings()));
            }
            catch (DocumentClientException de)
            {
                Exception baseException = de.GetBaseException();
                string msg = $"{de.StatusCode} error occurred: {de.Message}, Message: {baseException.Message}";
                return StatusCode(503, msg);
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                string msg = $"Error: {e.Message}, Message: {baseException.Message}";
                return StatusCode(503, msg);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] string value)
        {
            return StatusCode(501);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] string value)
        {
            return StatusCode(501);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return StatusCode(501);
        }
    }
}
