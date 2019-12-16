// <copyright company="PartyFinds LLC">
//   Copyright (c) PartyFinds LLC.  All rights reserved
// </copyright>

using System.Threading.Tasks;
using PartyFindsApi.core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.KeyVault;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Clients.ActiveDirectory;


namespace PartyFindsApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddSingleton<ICosmosDbService>(InitializeCosmosClientInstanceAsync(Configuration.GetSection("CosmosDb")).GetAwaiter().GetResult());
            services.AddControllers();
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

        public static async Task<string> GetToken()
        {
            var kv = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(GetToken));
            var dbKey = await kv.GetSecretAsync("https://funta.vault.azure.net/secrets/funtadb-key/bd0f813ed8c341ccb3b0baf2eb82bc46");
            return dbKey.Value;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var token = Startup.GetToken().Result;
            core.Container.Instance.listingsRepo = AzureCosmosDocRepository.CreateAzureCosmosDocRepository("Listings", token).Result;
            core.Container.Instance.userRepo = AzureCosmosDocRepository.CreateAzureCosmosDocRepository("Users", token).Result;
        }

        /// <summary>
        /// Creates a Cosmos DB database and a container with the specified partition key. 
        /// </summary>
        /// <returns></returns>
        private static async Task<CosmosDbService> InitializeCosmosClientInstanceAsync(IConfigurationSection configurationSection)
        {
            string databaseName = configurationSection.GetSection("DatabaseName").Value;
            string containerName = configurationSection.GetSection("ContainerName").Value;
            string account = configurationSection.GetSection("Account").Value;
            string key = configurationSection.GetSection("Key").Value;
            CosmosClientBuilder clientBuilder = new CosmosClientBuilder(account, key);
            CosmosClient client = clientBuilder
                                .WithConnectionModeDirect()
                                .Build();
            CosmosDbService cosmosDbService = new CosmosDbService(client, databaseName, containerName);
            DatabaseResponse database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
            await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");

            return cosmosDbService;
        }        
    }
}
