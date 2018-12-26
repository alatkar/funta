using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using core;
using core.repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace FeedService.Controllers
{
    [Route("api/[controller]")]
    public class FeedController : Controller
    {        
        DocumentClient client;
        string appid = "b4ea2dba-cf3d-4309-8d6c-d3fe29807232";

        IRepository feedRepo;

        public FeedController()
        {
            this.feedRepo = Container.Instance.feedRepo;
        }

        [HttpGet]
        public async Task<IEnumerable<Feed>> GetAsync() 
        {
            try
            {
                IList<Feed> feeds = await feedRepo.QueryAsync<Feed>("", null);
                return feeds;
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

        [HttpGet("{id}")]
        public async Task<Feed> GetAsync(string id) 
        {
            try
            {
                var res = await feedRepo.GetAsync<Feed>(id, null);
                return res;
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

        [HttpPost]
        public async Task<Feed> PostAsync([FromBody]Feed doc) 
        {
            try
            {
                var result = await feedRepo.CreateAsync(doc, null);    
                Feed fd = (dynamic)result;
                return fd;                
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
        public async Task<Feed> PatchAsync([FromBody]Feed doc) 
        {
            try
            {
                var result = await feedRepo.UpdateAsync(doc, null);    
                Feed fd = (dynamic)result;
                return fd;                
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

        [HttpDelete("{id}")]
        public async Task DeleteAsync(string id) //TODO: Figure out how to send error codes instead of 500
        {            
            await feedRepo.DeleteAsync(id, null); 
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