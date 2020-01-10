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

namespace PartyFindsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListingsController : ControllerBase
    {
        //private readonly ICosmosDbService _cosmosDbService;
        IRepository listingsRepo;

        public ListingsController()
        {
            //_cosmosDbService = cosmosDbService;
            this.listingsRepo = Container.Instance.listingsRepo;
        }


        // GET: api/Listings
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            //return await _cosmosDbService.GetItemsAsync("SELECT * FROM c");
            var feed = new FeedOptions();
            feed.EnableCrossPartitionQuery = true;

            try
            {
                var resp = await listingsRepo.QueryAsync<Listing>("", feed);
                return Ok(JsonConvert.SerializeObject(resp, new JsonApiSerializerSettings()));
            }
            catch(Exception ex)
            {
                return StatusCode(503, ex);
            }
        }

        // GET: api/Listings/94e70e4-1f9c-48c3-bfc9-272550fe3581
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(string id)
        {
            
            var feed = new FeedOptions();
            feed.EnableCrossPartitionQuery = true;

            try
            {
                //TODO: Need to figure out partitioning strategy
                var resp = await listingsRepo.QueryAsync<Listing>($" where C.id = '{id}'", feed);
                //var resp = await listingsRepo.GetAsync<Listings>(id, feed);
                if(resp == null || resp.Count == 0)
                {
                    return NotFound();
                }

                return Ok(JsonConvert.SerializeObject(resp.First(), new JsonApiSerializerSettings()));
            }
            catch(Exception ex)
            {
                return StatusCode(503, ex);
            }
        }

        // POST: api/Listings
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Listing item)
        {
            try
            {
                var result = await listingsRepo.CreateAsync(item, null);
                Listing fd = (dynamic)result;
                return Ok(JsonConvert.SerializeObject(fd, new JsonApiSerializerSettings()));
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

        [HttpPatch]
        public async Task<IActionResult> PatchAsync([FromBody]Listing doc)
        {
            try
            {
                var result = await listingsRepo.UpdateAsync(doc, null);
                Listing fd = (dynamic)result;
                return Ok(JsonConvert.SerializeObject(fd, new JsonApiSerializerSettings()));
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
