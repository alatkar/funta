// <copyright company="PartyFinds LLC">
//   Copyright (c) PartyFinds LLC.  All rights reserved
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using PartyFindsApi.core;
using PartyFindsApi.Models;

namespace PartyFindsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        IRepository messagesRepo;

        public MessagesController()
        {
            this.messagesRepo = Container.Instance.notificationsRepo;
        }

        // GET: api/Notifications
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //return await _cosmosDbService.GetItemsAsync("SELECT * FROM c");
            var feed = new FeedOptions();
            feed.EnableCrossPartitionQuery = true;

            try
            {
                var resp = await messagesRepo.QueryAsync<Message>("", feed);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                return StatusCode(503, ex);
            }
        }

        // GET: api/Notifications/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int userId)
        {
            try
            {
                var resp = await messagesRepo.QueryAsync<Message>($" where C.id = '{userId}'", null);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                return StatusCode(503, ex);
            }
        }

        [HttpPatch]
        public async Task<IActionResult> PatchAsync([FromBody]Message doc)
        {
            try
            {
                var result = await messagesRepo.UpdateAsync(doc, null);
                Message fd = (dynamic)result;
                return Ok(fd);
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

        // POST: api/Notifications
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string item)
        {
            try
            {
                var result = await messagesRepo.CreateAsync(item, null);
                Message fd = (dynamic)result;
                return Ok(fd);
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

        // PUT: api/Messages/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Message value)
        {
            return StatusCode(501);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return StatusCode(501);
        }
    }
}
