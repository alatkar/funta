// <copyright company="PartyFinds LLC">
//   Copyright (c) PartyFinds LLC.  All rights reserved
// </copyright>

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.KeyVault;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.OpenApi.Models;
using Microsoft.Azure.Documents.Client;
using Azure.Storage.Blobs;
using PartyFindsApi.core;
using Microsoft.Extensions.Logging;

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
            services.AddControllers().AddNewtonsoftJson();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });
        }

        public async Task<string> GetSecret(string secret)
        {
            if (!string.IsNullOrEmpty(secret) && secret.Contains("secrets", StringComparison.InvariantCultureIgnoreCase))
            {
                //TODO: Get from KeyVault
                return secret;
            }
            else
            {
                return secret;
            }
        }

        async Task<string> GetToken(string authority /*= "https://login.windows.net/alatkaryahoo.onmicrosoft.com"*/,
           string resource /* ="https://funta.vault.azure.net"*/, string scope)
        {
            // Get Key from Keyvault
            var authContext = new AuthenticationContext(authority);
            ClientCredential clientCred = new ClientCredential(
                Configuration.GetValue<string>("KeyVault:AzureADApplicationId"),//"b4ea2dba-cf3d-4309-8d6c-d3fe29807232",
                Configuration.GetValue<string>("KeyVault:AzureADApplicationSecret"));//"hpPrDcv7SO63qvbhp+J9QOWePrywHES4u75Xxq7yGU8=");
            AuthenticationResult result = await authContext.AcquireTokenAsync(resource, clientCred).ConfigureAwait(false);
            return result.AccessToken;
        }

        public async Task<string> GetToken()
        {
            var kv = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(GetToken));
            var dbKey = await kv.GetSecretAsync("https://funta.vault.azure.net/secrets/funtadb-key/");
            return dbKey.Value;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                //c.RoutePrefix = string.Empty;
            });

            logger.LogInformation($"Application: {env?.ApplicationName} Environment: {env.EnvironmentName}");
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

            // Create Azure resources
            
            // Connect to the Azure Cosmos
            DocumentClient client = new DocumentClient(
               new Uri(Configuration.GetSection("CosmosDb")["EndPoint"]),
               GetSecret(Configuration.GetSection("CosmosDb")["Key"]).Result);

            PartyFindsApi.core.Container.Instance.listingsRepo = 
                AzureCosmosDocRepository.CreateAzureCosmosDocRepository("Listings", client).Result;
            PartyFindsApi.core.Container.Instance.messageRepo = 
                AzureCosmosDocRepository.CreateAzureCosmosDocRepository("Messages", client).Result;
            PartyFindsApi.core.Container.Instance.notificationsRepo = 
                AzureCosmosDocRepository.CreateAzureCosmosDocRepository("Notifications", client).Result;
            PartyFindsApi.core.Container.Instance.userRepo = 
                AzureCosmosDocRepository.CreateAzureCosmosDocRepository("Users", client).Result;

            // TODO: Old code based on Token. Migrate to KeyVault or managed identity
            //var token = GetToken().Result;
            //core.Container.Instance.listingsRepo = AzureCosmosDocRepository.CreateAzureCosmosDocRepository("Listings", token).Result;

            // Blob Container
            string endpointSuffix = Configuration.GetValue<string>("Storage:EndpointSuffix");
            if (!string.IsNullOrEmpty(endpointSuffix))
            {
                endpointSuffix = $";EndpointSuffix={endpointSuffix}";
            }

            BlobServiceClient blobServiceClient = new BlobServiceClient
                ($"DefaultEndpointsProtocol=http;" +
                $"AccountName={Configuration.GetValue<string>("Storage:AccountName")};" +
                $"AccountKey={GetSecret(Configuration.GetValue<string>("Storage:Key")).Result};" +
                $"{endpointSuffix};BlobEndpoint={Configuration.GetValue<string>("Storage:BlobEndpoint")};");
            core.Container.Instance.uploadsContainer = blobServiceClient.GetBlobContainerClient("uploads");
            core.Container.Instance.uploadsContainer.CreateIfNotExists();
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
