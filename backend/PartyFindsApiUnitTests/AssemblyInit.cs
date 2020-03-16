// <copyright company="PartyFinds LLC">
//   Copyright (c) PartyFinds LLC.  All rights reserved
// </copyright>

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JsonApiSerializer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace PartyFindsApiUnitTests
{
    [TestClass]
    public class AssemblyInit
    {
        public static readonly TestServer _server;
        public static HttpClient _client;
        public static Random random = new Random();
        public static string testUser = "test";
        public static string testUserEmail = "test@test.com";
        public static string testUserId = null;

        [AssemblyInitialize]
        public static async Task AssemblyInitializeAsync(TestContext testContext)
        {
            var projectDir = Directory.GetCurrentDirectory();
            var configPath = Path.Combine(projectDir, "appsettings.Development.json");
            var builder = new WebHostBuilder();
            builder.ConfigureAppConfiguration((context, conf) =>
            {
                conf.AddJsonFile(configPath);
            });

            // Delete Database (Remove comment when need to test)
            //await DeleteEmulatorDatabase().ConfigureAwait(false);

            var _server = new TestServer(builder
            .UseStartup<PartyFindsApi.Startup>());
            AssemblyInit._client = _server.CreateClient();

            // TODO: Set up Test Application wide Data
            await InitTestScenarios();
        }

        [AssemblyCleanup]
        public static async Task AssemblyCleanupAsync()
        {
            Console.WriteLine("AssemblyCleanup started");
            await CleanUp();
            Console.WriteLine("AssemblyCleanup finished");
            // TODO: Clean up Test Application wide Data
        }

        static async Task DeleteEmulatorDatabase()
        {
            // Delete Database
            DocumentClient client = new DocumentClient(
               new Uri("https://localhost:8081"),
               "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");

            Database database = client.CreateDatabaseQuery().Where(d => d.Id == "PartyFinds").AsEnumerable().FirstOrDefault();
            await client.DeleteDatabaseAsync(database.SelfLink).ConfigureAwait(false);
        }

        static async Task InitTestScenarios()
        {
            var response = await AssemblyInit._client.GetAsync("api/users");
            Assert.IsTrue(response.IsSuccessStatusCode);

            var stream = await response.Content.ReadAsStringAsync();
            var usersResult = JsonConvert.DeserializeObject<List<User>>(stream, new JsonApiSerializerSettings());

            foreach (var userToDelete in usersResult)
            {
                response = await AssemblyInit._client.DeleteAsync($"api/users/{userToDelete.Id}");
                Assert.IsTrue(response.IsSuccessStatusCode);
            }

            // Create Account
            var obj = new PartyFindsApi.Models.User
            {
                Email = "aa@aa.com",
                PasswordHash = "Password"
            };

            response = await AssemblyInit._client.PostAsync(
                "api/register", 
                new StringContent(JsonConvert.SerializeObject(obj), 
                Encoding.UTF8,
                "application/json"));
            Assert.IsTrue(response.IsSuccessStatusCode);
            stream = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<PartyFindsApi.Models.User>(stream, new JsonApiSerializerSettings());

            testUserId = user.Id;
        }

        static async Task CleanUp()
        {
            // Create Account
            await AssemblyInit._client.DeleteAsync($"api/users/{testUserId}");
        }
    }
}
