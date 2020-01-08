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
    public class NotificationsController : ControllerBase
    {
        IRepository notificationsRepo;

        public NotificationsController()
        {
            //_cosmosDbService = cosmosDbService;
            this.notificationsRepo = Container.Instance.notificationsRepo;
        }

        // GET: api/Notifications
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var feed = new FeedOptions();
            feed.EnableCrossPartitionQuery = true;

            try
            {
                var resp = await notificationsRepo.QueryAsync<Listing>("", feed);
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
                var resp = await notificationsRepo.QueryAsync<Listing>($" where C.userId = '{userId}'", null);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                return StatusCode(503, ex);
            }
        }

        [HttpPatch]
        public async Task<IActionResult> PatchAsync([FromBody]Listing doc)
        {
            try
            {
                var result = await notificationsRepo.UpdateAsync(doc, null);
                Listing fd = (dynamic)result;
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
                var result = await notificationsRepo.CreateAsync(item, null);
                Listing fd = (dynamic)result;
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

        // PUT: api/Listings/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Listing value)
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
