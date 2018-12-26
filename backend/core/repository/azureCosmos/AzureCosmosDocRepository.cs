using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json.Linq;

namespace core.repository.azureCosmos
{
    public class AzureCosmosDocRepository : IRepository
    {
        DocumentClient client;
        static string databaseName = "Feed";
        public string collectionName { get; }
        
        public static async Task<IRepository> CreateAzureCosmosDocRepository(string collection, string token)
        {
            AzureCosmosDocRepository azureCosmosDocRepository = new AzureCosmosDocRepository(GetDocumentClient(token).Result, collection);
            await azureCosmosDocRepository.Initialize();
            return azureCosmosDocRepository;
        }

        private AzureCosmosDocRepository(DocumentClient client, string collection)
        {
            this.collectionName = collection;
            this.client = client;
        }

        private async Task Initialize()
        {
            ResourceResponse<Database> resourceResponse =  Task.Run(() => 
                client.CreateDatabaseIfNotExistsAsync(new Database { Id = databaseName })).Result;
            await Task.Run(() =>
                 client.CreateDocumentCollectionIfNotExistsAsync
                 (UriFactory.CreateDatabaseUri(databaseName), new DocumentCollection { Id = this.collectionName }));   
        }
        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public async Task<T> GetAsync<T>(string id, FeedOptions options)
        {
            try
            {
                var res = await this.client.ReadDocumentAsync(UriFactory.CreateDocumentUri(databaseName, this.collectionName, id));
                return (dynamic)res.Resource;
            }
            catch(Exception ex)
            {
                Console.WriteLine("AzureCosmosDocRepository:GetAsync Error: {0}", ex.Message);
                throw;
            } 
        }

        public async Task<T> CreateAsync<T>(T doc, FeedOptions options)
        {
            try
            {
                var res =  await this.client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), doc);
                return (dynamic)res.Resource;
            }
            catch(Exception ex)
            {
                Console.WriteLine("AzureCosmosDocRepository:CreateAsync Error: {0}", ex.Message);
                throw;
            }            
        }

        public async Task<T> UpdateAsync<T>(T doc, FeedOptions options) where T : DocumentBase
        {
            try
            {
                var existing = await this.client.ReadDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, doc.Id));   
                dynamic json = JObject.FromObject(doc);
                //json.id = doc.Id; //Didn't understand why earlier approach didn't work                
                var res =  await this.client.ReplaceDocumentAsync(existing.Resource.SelfLink, json);   
                return (dynamic)res.Resource;          
            }
            catch(Exception ex)
            {
                Console.WriteLine("AzureCosmosDocRepository:UpdateAsync Error: {0}", ex.Message);
                throw;
            }
        }

         public async Task<IList<T>> QueryAsync<T>(string filter = "", FeedOptions options = null)
        {
            var query = this.client.CreateDocumentQuery<T>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, this.collectionName),                    
                    $"SELECT * FROM {collectionName} {filter} ",
                    options)
                    .AsEnumerable().ToList();       


            return query;
        }

        public async Task DeleteAsync(string docId, FeedOptions options)
        {
            var existing = await this.client.ReadDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, docId));
            await this.client.DeleteDocumentAsync(existing.Resource.SelfLink);
        }

        public async Task<T> CreateIfNotExists<T>(T doc, FeedOptions options) where T : DocumentBase
        {
            try
            {
                var res = await this.client.ReadDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, doc.Id?.ToString()));      
                return (dynamic)res.Resource;          
            }
            catch (DocumentClientException de)
            {
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    var res =  await this.client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), doc);
                    return (dynamic)res.Resource;          
                }
                else
                {
                    throw;
                }
            }
        }

        static async Task<string> GetToken(string authority /*= "https://login.windows.net/alatkaryahoo.onmicrosoft.com"*/, 
            string resource /* ="https://funta.vault.azure.net"*/, string scope)
        {
             // Get Key from Keyvault
            var authContext = new AuthenticationContext(authority);
            ClientCredential clientCred = new ClientCredential("b4ea2dba-cf3d-4309-8d6c-d3fe29807232",
            "hpPrDcv7SO63qvbhp+J9QOWePrywHES4u75Xxq7yGU8=");
            AuthenticationResult result = await authContext.AcquireTokenAsync(resource, 
            clientCred);
            return result.AccessToken;
        }

         static async Task<DocumentClient> GetDocumentClient()
        {
            var kv = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(GetToken));
            var dbKey = await kv.GetSecretAsync("https://funta.vault.azure.net/secrets/funtadb-key/bd0f813ed8c341ccb3b0baf2eb82bc46");

            string EndpointUri = "https://funtadb.documents.azure.com:443/";
            var client = new DocumentClient(new Uri(EndpointUri), dbKey.Value);                

            return client;   
        }

        static async Task<DocumentClient> GetDocumentClient(string token)
        {
            string EndpointUri = "https://funtadb.documents.azure.com:443/";
            var client = new DocumentClient(new Uri(EndpointUri), token);     
          
            return client;   
        }
    }
}