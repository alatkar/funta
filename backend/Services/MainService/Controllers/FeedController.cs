using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using core;
using core.repository;
using MainService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MainService.Controllers
{
    [Route("api/[controller]")]
    public class FeedController : Controller
    {
        IRepository feedRepo;

        public FeedController()
        {
            this.feedRepo = Container.Instance.feedRepo;
        }

        // GET: api/<controller>
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            try
            {
                IList<Feed> feeds = await feedRepo.QueryAsync<Feed>("", null);
                if (feeds == null)
                {
                    return NotFound();
                }

                return Ok(Json(feeds));
            }
            catch (DocumentClientException de)
            {
                /*throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StreamContent(this.Serializer.Serialize($"{de.StatusCode} error occurred: {de.Message}, Message: {de.Message}"))
                    });*/
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
            }
            return null;
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(string id)
        {
            try
            {
                var feed = await feedRepo.GetAsync<Feed>(id, null);
                return Ok(feed);
            }
            catch (DocumentClientException de)
            {
                Exception baseException = de.GetBaseException();
                Console.WriteLine("{0} error occurred: {1}, Message: {2}", de.StatusCode, de.Message, baseException.Message);
                throw;
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
                throw;
            }
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<ActionResult<Feed>> PostAsync([FromBody]Feed doc)
        {
            try
            {
                Feed result = await feedRepo.CreateAsync(doc, null);
                Ok(result);
            }
            catch (DocumentClientException de)
            {
                Exception baseException = de.GetBaseException();
                Console.WriteLine("{0} error occurred: {1}, Message: {2}", de.StatusCode, de.Message, baseException.Message);
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
            }
            return null;
        }

        [HttpPatch]
        public async Task<ActionResult<Feed>> PatchAsync([FromBody]Feed doc)
        {
            try
            {
                var result = await feedRepo.UpdateAsync(doc, null);
                Feed fd = (dynamic)result;
                Ok(fd);
            }
            catch (DocumentClientException de)
            {
                Exception baseException = de.GetBaseException();
                Console.WriteLine("{0} error occurred: {1}, Message: {2}", de.StatusCode, de.Message, baseException.Message);
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
            }
            return null;
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            await feedRepo.DeleteAsync(id, null);
            return Ok();
            /*
            try
            {
                await repo.DeleteAsync(id, null); 
                //return HttpStatusCode.OK;
            }
            catch (DocumentClientException de)
            {
                    Exception baseException = de.GetBaseException();
                    Console.WriteLine("{0} error occurred: {1}, Message: {2}", de.StatusCode, de.Message, baseException.Message);                    
                    //return HttpStatusCode.NotFound;
            }
            catch (Exception e)
            {
                    Exception baseException = e.GetBaseException();
                    Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
                    throw;
            } */
        }
    }
}
