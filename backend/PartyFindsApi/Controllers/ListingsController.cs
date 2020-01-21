// <copyright company="PartyFinds LLC">
//   Copyright (c) PartyFinds LLC.  All rights reserved
// </copyright>

using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using PartyFindsApi.core;
using PartyFindsApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using JsonApiSerializer;
using Microsoft.AspNetCore.JsonPatch;

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
            // TODO: Parse query and pass to repo
            var query = this.Request.Query;
            var queryString = this.Request.QueryString;

            //return await _cosmosDbService.GetItemsAsync("SELECT * FROM c");
            try
            {
                var resp = await listingsRepo.QueryAsync<Listing>("", new FeedOptions { EnableCrossPartitionQuery = true });
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
            try
            {
                var resp = await listingsRepo.GetAsync<Listing>(id, new RequestOptions { PartitionKey = new PartitionKey(id) });
                if(resp == null )
                {
                    return NotFound();
                }

                return Ok(JsonConvert.SerializeObject(resp, new JsonApiSerializerSettings()));
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
                var result = await listingsRepo.CreateAsync(item, new RequestOptions());
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
        public async Task<IActionResult> PatchAsync(string id, [FromBody] JsonPatchDocument<Listing> patchDoc)
        {
            try
            {
                var resp = await listingsRepo.QueryAsync<Listing>($" where C.id = '{id}'", new FeedOptions { PartitionKey = new PartitionKey(id) });

                var listing =  resp.FirstOrDefault<Listing>();

                if(listing == null)
                {
                    return NotFound($"The Listing with id {id} is not found");
                }

                patchDoc.ApplyTo(listing, ModelState);
                
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await listingsRepo.UpdateAsync(listing, new RequestOptions { PartitionKey = new PartitionKey(id) });
                //Listing fd = (dynamic)result;  Do we need this?
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
