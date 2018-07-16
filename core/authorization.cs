using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace core
{
    public class authorization
    {
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

         public static async Task<string> GetToken()
        {
            var kv = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(GetToken));
            var dbKey = await kv.GetSecretAsync("https://funta.vault.azure.net/secrets/funtadb-key/bd0f813ed8c341ccb3b0baf2eb82bc46");
            return dbKey.Value;
        }

/*        public async Task<DocumentClient> GetDocumentClient()
        {
            var kv = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(GetToken));
            var dbKey = await kv.GetSecretAsync("https://funta.vault.azure.net/secrets/funtadb-key/bd0f813ed8c341ccb3b0baf2eb82bc46");

            string EndpointUri = "https://funtadb.documents.azure.com:443/";
            var client = new DocumentClient(new Uri(EndpointUri), dbKey.Value);     

            //Running Aync methods in Ctor
            ResourceResponse<Database> resourceResponse =  Task.Run(() => 
                client.CreateDatabaseIfNotExistsAsync(new Database { Id = "Feed" })).Result;
            await Task.Run(() =>
                 client.CreateDocumentCollectionIfNotExistsAsync
                 (UriFactory.CreateDatabaseUri("Feed"), new DocumentCollection { Id = "FeedCollection" }));        

            return client;   
        }*/
    }
}