using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FeedService.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json.Linq;

namespace FeedService.Controllers
{
    [Route("api/[controller]")]
    public class FeedController : Controller
    {        
        DocumentClient client;

        public FeedController()
        {
            string EndpointUri = "https://funtadb.documents.azure.com:443/";
            string PrimaryKey = "XXXXX";
            this.client = new DocumentClient(new Uri(EndpointUri), PrimaryKey);     

            //Running Aync methods in Ctor
            ResourceResponse<Database> resourceResponse =  Task.Run(() => 
                this.client.CreateDatabaseIfNotExistsAsync(new Database { Id = "Feed" })).Result;
            Task.Run(() => 
                this.client.CreateDocumentCollectionIfNotExistsAsync
                (UriFactory.CreateDatabaseUri("Feed"), new DocumentCollection { Id = "FeedCollection" }));                  
        }

        [HttpGet]
        public async Task<IEnumerable<IFeed>> GetAsync() 
        {
            try
            {  
                FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };
                var familyQueryInSql = this.client.CreateDocumentQuery<Feed>(
                UriFactory.CreateDocumentCollectionUri("Feed", "FeedCollection"),
                "SELECT * FROM FeedCollection ",
                queryOptions)
                .AsEnumerable().ToList();            

                IList<IFeed> feeds = new List<IFeed>();
                feeds.Add(new EventFeed("Seattle"));
                feeds.Add(new TipFeed("Dog Food"));
                
                var test = familyQueryInSql;

                return familyQueryInSql;
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
                var res = await this.client.ReadDocumentAsync(UriFactory.CreateDocumentUri("Feed", "FeedCollection", id));
                return (dynamic)res.Resource;
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
        public async Task<Feed> PostAsync([FromBody]Feed value) 
        {
            try
            {
                var result = await CreateFeedIfNotExists("Feed", "FeedCollection", value);    
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
        public async Task<Feed> PatchAsync([FromBody]Feed value) 
        {
            try
            {
                var result = await UpdateFeed("Feed", "FeedCollection", value);    
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

        private async Task<Document> CreateFeedIfNotExists(string databaseName, string collectionName, Feed feed)
        {
            try
            {
                return await this.client.ReadDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, feed.Id.ToString()));                
            }
            catch (DocumentClientException de)
            {
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    return await this.client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), feed);
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task<Document> UpdateFeed(string databaseName, string collectionName, Feed feed)
        {            
            try{
            var doc = await this.client.ReadDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, feed.Id.ToString()));   
            dynamic json = JObject.FromObject(doc.Resource);
            json.id = feed.Id; //Didn't understand why earlier approach didn't work
            return await this.client.ReplaceDocumentAsync(doc.Resource.SelfLink, json);             
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                throw;
            }
        }
    }
}