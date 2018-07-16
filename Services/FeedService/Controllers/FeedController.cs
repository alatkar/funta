using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using core;
using core.repository;
using core.repository.azureCosmos;
using FeedService.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json.Linq;

namespace FeedService.Controllers
{
    [Route("api/[controller]")]
    public class FeedController : Controller
    {        
        DocumentClient client;
        string appid = "b4ea2dba-cf3d-4309-8d6c-d3fe29807232";

        IRepository repo;
        
        public FeedController()
        {            
            this.repo = Container.FromContainer().repo;
        }

        [HttpGet]
        public async Task<IEnumerable<IFeed>> GetAsync() 
        {
            try
            {
                IList<Feed> feeds = await repo.QueryAsync<Feed>("", null);
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
                var res = await repo.GetAsync<Feed>(id, null);
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
                var result = await repo.CreateFeedIfNotExists(doc, null);    
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
                var result = await repo.UpdateAsync(doc, null);    
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
    }
}