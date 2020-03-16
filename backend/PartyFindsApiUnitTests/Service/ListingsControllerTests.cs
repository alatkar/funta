// <copyright company="PartyFinds LLC">
//   Copyright (c) PartyFinds LLC.  All rights reserved
// </copyright>

using JsonApiSerializer;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Azure.Documents;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PartyFindsApi.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace PartyFindsApiUnitTests.Service
{
    [TestClass]
    public class ListingsControllerTests
    {
        [TestMethod]
        public async System.Threading.Tasks.Task QueryAllAsync()
        {
            var response = await AssemblyInit._client.GetAsync("api/listings");
            Assert.IsTrue(response.IsSuccessStatusCode);

            var stream = await response.Content.ReadAsStringAsync();
            var listingsResult = JsonConvert.DeserializeObject<List<Listing>>(stream, new JsonApiSerializerSettings());

            response = await AssemblyInit._client.GetAsync($"api/listings/{listingsResult[0].Id}");
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task QueryByIdAsync()
        {
            var response = await AssemblyInit._client.GetAsync("api/listings");
            Assert.IsTrue(response.IsSuccessStatusCode);

            var stream = await response.Content.ReadAsStringAsync();
            var listingsResult = JsonConvert.DeserializeObject<List<Listing>>(stream, new JsonApiSerializerSettings());
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task CreateDeleteListing()
        {
            var obj = new Listing
            {
                UserId = AssemblyInit.testUserId,
                Title = "Some Title",
                ListingType = "Some Type",
                ListingSubType = "Some Sub Type"
            };

            var response = await AssemblyInit._client.PostAsync(
                "api/listings", new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json"));
            var stream = await response.Content.ReadAsStringAsync();
            var listing = JsonConvert.DeserializeObject<Listing>(stream, new JsonApiSerializerSettings());

            response = await AssemblyInit._client.GetAsync($"api/listings/{listing.Id}");
            Assert.IsTrue(response.IsSuccessStatusCode);
            stream = await response.Content.ReadAsStringAsync();
            listing = JsonConvert.DeserializeObject<Listing>(stream, new JsonApiSerializerSettings());

            response = await AssemblyInit._client.DeleteAsync($"api/listings/{listing.Id}");
            Assert.IsTrue(response.IsSuccessStatusCode);

            //await Assert.ThrowsExceptionAsync<DocumentClientException>(() => AssemblyInit._client.DeleteAsync($"api/listings/{listing.Id}"));
        }

        [TestMethod]
        public async System.Threading.Tasks.Task PatchAsync()
        {
            var response = await AssemblyInit._client.GetAsync("api/listings");
            Assert.IsTrue(response.IsSuccessStatusCode);

            var stream = await response.Content.ReadAsStringAsync();
            var listingsResult = JsonConvert.DeserializeObject<List<Listing>>(stream, new JsonApiSerializerSettings());

            response = await AssemblyInit._client.GetAsync($"api/listings/{listingsResult[0].Id}");
            Assert.IsTrue(response.IsSuccessStatusCode);
            stream = await response.Content.ReadAsStringAsync();
            var listing = JsonConvert.DeserializeObject<Listing>(stream, new JsonApiSerializerSettings());
            Assert.AreEqual(0, listing.ImageUrls.Count);
            string oldTitle = listing.Title;

            string newTitle = "New Title";
            var jsonPatch = new JsonPatchDocument<Listing>().Replace(x => x.Title, newTitle).Add(x => x.ImageUrls, "Some Link");
            response = await AssemblyInit._client.PatchAsync($"api/listings/{listingsResult[0].Id}", 
                new StringContent(JsonConvert.SerializeObject(jsonPatch), Encoding.UTF8, "application/json"));
            Assert.IsTrue(response.IsSuccessStatusCode);
            stream = await response.Content.ReadAsStringAsync();
            listing = JsonConvert.DeserializeObject<Listing>(stream, new JsonApiSerializerSettings());
            Assert.AreEqual(1, listing.ImageUrls.Count);
            Assert.AreEqual(newTitle, listing.Title);
            Assert.AreNotEqual(oldTitle, listing.Title);
        }

        [ClassInitialize]
        public static async System.Threading.Tasks.Task ClassInitializeAsync(TestContext context)
        {
            Console.WriteLine("ClassInitialize");

            var obj = new Listing
            {
                UserId = AssemblyInit.testUserId,
                Title = "Some Title",
                ListingType = "Some Type",
                ListingSubType = "Some Sub Type"
            };

            var response = await AssemblyInit._client.PostAsync("api/listings", 
                new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json"));
            var stream = await response.Content.ReadAsStringAsync();
            var listingsResult = JsonConvert.DeserializeObject<Listing>(stream, new JsonApiSerializerSettings());
            response = await AssemblyInit._client.GetAsync($"api/listings/{listingsResult.Id}");
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [ClassCleanup]
        public static async System.Threading.Tasks.Task ClassCleanupAsync()
        {
            Console.WriteLine("ClassCleanup");

            var response = await AssemblyInit._client.GetAsync("api/listings");
            Assert.IsTrue(response.IsSuccessStatusCode);

            var stream = await response.Content.ReadAsStringAsync();
            var listingsResult = JsonConvert.DeserializeObject<List<Listing>>(stream, new JsonApiSerializerSettings());

            foreach(var listing in listingsResult)
            {
                response = await AssemblyInit._client.DeleteAsync($"api/listings/{listing.Id}");
                Assert.IsTrue(response.IsSuccessStatusCode);
            }
        }

        [TestInitialize]
        public void TestInitialize()
        {
            Console.WriteLine("TestInitialize");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Console.WriteLine("TestCleanup");
        }
    }
}
