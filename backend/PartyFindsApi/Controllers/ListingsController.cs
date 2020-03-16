// <copyright company="PartyFinds LLC">
//   Copyright (c) PartyFinds LLC.  All rights reserved
// </copyright>

using System;
using System.Linq;
using System.Threading.Tasks;
using PartyFindsApi.core;
using PartyFindsApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using JsonApiSerializer;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using System.Net;

namespace PartyFindsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListingsController : ControllerBase
    {
        private readonly ILogger logger;
        //private readonly ICosmosDbService _cosmosDbService;
        IRepository listingsRepo;
        IRepository userRepo;

        public ListingsController(ILogger<ListingsController> logger)
        {
            //_cosmosDbService = cosmosDbService;
            this.listingsRepo = Container.Instance.listingsRepo;
            this.logger = logger;
            this.userRepo = Container.Instance.userRepo;
        }


        /// <summary>
        /// Creates a TodoItem.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Todo
        ///     {
        ///        "id": 1,
        ///        "name": "Item1",
        ///        "isComplete": true
        ///     }
        ///
        /// </remarks>
        /// <param name="item"></param>
        /// <returns>A newly created TodoItem</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>   
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            // TODO: Parse query and pass to repo
            var query = this.Request.Query;
            var queryString = this.Request.QueryString;

            logger.LogInformation($"Getting listings with query {query} queryString {queryString}");

            try
            {
                var resp = await listingsRepo.QueryAsync<Listing>("", new FeedOptions { EnableCrossPartitionQuery = true }).ConfigureAwait(false);
                return Ok(JsonConvert.SerializeObject(resp, new JsonApiSerializerSettings()));
            }
            catch(Exception ex)
            {
                logger.LogError($"Received exception {ex} for querystring {queryString}");
                return StatusCode(500, ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(string id)
        {            
            try
            {
                var resp = await listingsRepo.GetAsync<Listing>(id, new RequestOptions { PartitionKey = new PartitionKey(id) }).ConfigureAwait(false);
                if(resp == null )
                {
                    return NotFound($"Listing with id {id} is not found");
                }

                return Ok(JsonConvert.SerializeObject(resp, new JsonApiSerializerSettings()));
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        /// <summary>
        /// Creates a TodoItem.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Todo
        ///     {
        ///        "id": 1,
        ///        "name": "Item1",
        ///        "isComplete": true
        ///     }
        ///
        /// </remarks>
        /// <param name="item"></param>
        /// <returns>A newly created TodoItem</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>   
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Listing item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (item == null || item.UserId == null)
            {
                logger.LogError($"Listing is null or missing UserId {item}");
                throw new ArgumentException($"Listing is null or missing UserId {item}", nameof(item));
            }

            try
            {
                logger.LogInformation($"Creating new listing {item}");

                var feed = new RequestOptions { PartitionKey = new PartitionKey(item.UserId) };
                var resp = await userRepo.GetAsync<Models.User>(item.UserId, feed).ConfigureAwait(false);
                if (resp == null)
                {
                    return NotFound($"User {item.UserId} not found");
                }

                var result = await listingsRepo.CreateAsync(item, new RequestOptions()).ConfigureAwait(false);
                Listing fd = (dynamic)result;

                logger.LogInformation($"Created new listing with id {fd.Id}");

                return Ok(JsonConvert.SerializeObject(fd, new JsonApiSerializerSettings()));
            }
            catch (DocumentClientException de)
            {
                string msg = $"{de.StatusCode} error occurred: {de.Message}, Message: {de.Message}";
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    return NotFound(msg);
                }

                return BadRequest(msg);
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                string msg = $"Error: {e.Message}, Message: {baseException.Message}";
                return BadRequest(msg);
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchAsync(string id, [FromBody] JsonPatchDocument<Listing> patchDoc)
        {
            try
            {
                logger.LogInformation($"Updating listing with id {id}");

                var resp = await listingsRepo.QueryAsync<Listing>($" where C.id = '{id}'", new FeedOptions { PartitionKey = new PartitionKey(id) }).ConfigureAwait(false);

                var listing =  resp.FirstOrDefault<Listing>();

                if(listing == null)
                {
                    return NotFound($"The Listing with id {id} is not found");
                }

                patchDoc.ApplyTo(listing, ModelState);
                
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState.Values);
                }

                var result = await listingsRepo.UpdateAsync(listing, new RequestOptions { PartitionKey = new PartitionKey(id) }).ConfigureAwait(false);

                logger.LogInformation($"Updated listing with id {id}");

                //Listing fd = (dynamic)result;  Do we need this?
                return Ok(JsonConvert.SerializeObject((dynamic)result, new JsonApiSerializerSettings()));
            }
            catch (DocumentClientException ex)
            {
                Exception baseException = ex.GetBaseException();
                string msg = $"{ex.StatusCode} error occurred: {ex.Message}, Message: {baseException.Message}";
                logger.LogError($"Received error {msg}");
                return BadRequest(msg);
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                string msg = $"Error: {e.Message}, Message: {baseException.Message}";
                logger.LogError($"Received error {msg}");
                return BadRequest(msg);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                logger.LogError($"Id not present {id}");
                throw new ArgumentException($"Please provide Listing Id  {id}", nameof(id));
            }

            try
            {
                // TODO: Check if user exists
                logger.LogInformation($"Deleting listing with id {id}");
                await listingsRepo.DeleteAsync(
                    id, 
                    new RequestOptions { PartitionKey = new PartitionKey(id) })
                    .ConfigureAwait(false);

                logger.LogInformation($"Deleted listing with id {id}");
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
