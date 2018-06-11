using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace FeedService.Controllers
{
    [Route("api/[controller]")]
    public class FeedController : Controller
    {        
        DocumentClient client;

        public FeedController()
        {
            string EndpointUri = "https://funtadb.documents.azure.com:443/";
            string PrimaryKey = "rLCebojdoh7su4Ld9zlINmt4NYfwjGSZT81nD2wRB3BQ9KNbNyqV3bqnxRFZzpIah1q8Yz7shJP1106NonUfdw==";
            this.client = new DocumentClient(new Uri(EndpointUri), PrimaryKey);            
        }

        [HttpGet]
        public async Task<IEnumerable<Feed>> GetAsync() 
        {
            try
            {
                ResourceResponse<Database> resourceResponse = await this.client.CreateDatabaseIfNotExistsAsync(new Database { Id = "Feed" });
                Database db = resourceResponse;
                await this.client.CreateDocumentCollectionIfNotExistsAsync
                    (UriFactory.CreateDatabaseUri("Feed"), new DocumentCollection { Id = "FeedCollection" });

                var values = await this.client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri("Feed", "FeedCollection"));

                FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };
                IQueryable<Feed> familyQueryInSql = this.client.CreateDocumentQuery<Feed>(
                UriFactory.CreateDocumentCollectionUri("Feed", "FeedCollection"),
                "SELECT * FROM FeedCollection ",
                queryOptions);

                /*
                var res = new List<Feed>
                {
                    new Feed {
                    Id = "Some ID",
                    Type = FeedTypes.POST,
                    Data = "I love my German Shepherd",
                    ImageUrl = "",
                    UserName = "admin",
                    DateCreated = DateTime.UtcNow.AddDays(-5),
                    DateLastUpdated = DateTime.UtcNow.AddDays(-1)
                    },
                    new Feed {
                    Id = "Some ID",
                    Type = FeedTypes.POST,
                    Data = "I love my German Shepherd",
                    ImageUrl = "",
                    UserName = "admin",
                    DateCreated = DateTime.UtcNow.AddDays(-5),
                    DateLastUpdated = DateTime.UtcNow.AddDays(-1)
                    }
                };*/
                return (dynamic)familyQueryInSql;
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
                ResourceResponse<Database> resourceResponse = await this.client.CreateDatabaseIfNotExistsAsync(new Database { Id = "Feed" });
                Database db = resourceResponse;
                await this.client.CreateDocumentCollectionIfNotExistsAsync
                    (UriFactory.CreateDatabaseUri("Feed"), new DocumentCollection { Id = "FeedCollection" });
                return new Feed {
                    Id = "Some ID",
                    Type = FeedTypes.POST,
                    Data = "I love my German Shepherd",
                    ImageUrl = "",
                    UserName = "admin",
                    DateCreated = DateTime.UtcNow.AddDays(-5),
                    DateLastUpdated = DateTime.UtcNow.AddDays(-1)
                };
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

        [HttpPost]
        public async Task<Feed> PostAsync([FromBody]Feed value) 
        {
            try
            {
                ResourceResponse<Database> resourceResponse = await this.client.CreateDatabaseIfNotExistsAsync(new Database { Id = "Feed" });
                Database db = resourceResponse;
                
                //await this.client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri("Feed") ,  new DocumentCollection { Id = "FeedCollection"});
            
                await this.client.CreateDocumentCollectionIfNotExistsAsync
                    (UriFactory.CreateDatabaseUri("Feed"), new DocumentCollection { Id = "FeedCollection" });

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

        private async Task<Document> CreateFeedIfNotExists(string databaseName, string collectionName, Feed feed)
        {
            try
            {
                return await this.client.ReadDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, feed.Id));                
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
    }
}