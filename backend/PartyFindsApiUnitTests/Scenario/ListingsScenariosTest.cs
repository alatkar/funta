// <copyright company="PartyFinds LLC">
//   Copyright (c) PartyFinds LLC.  All rights reserved
// </copyright>

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using JsonApiSerializer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PartyFindsApi.Models;

namespace PartyFindsApiUnitTests.Scenario
{
    [TestClass]
    public class ListingsScenariosTest
    {
        [TestMethod]
        public async System.Threading.Tasks.Task PostListingForUnknownUserAsync()
        {
            // Create Account
            var obj = new Listing
            {
                UserId = Guid.NewGuid().ToString(),
                Title = "Some Title",
                ListingType = "Some Type",
                ListingSubType = "Some Sub Type"
            };

            var response = await AssemblyInit._client.PostAsync("api/listings", new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json"));
            Assert.IsFalse(response.IsSuccessStatusCode);

            response = await AssemblyInit._client.GetAsync("api/listings");
            Assert.IsTrue(response.IsSuccessStatusCode);

            var stream = await response.Content.ReadAsStringAsync();
            var listingsResult = JsonConvert.DeserializeObject<List<Listing>>(stream, new JsonApiSerializerSettings());
            Assert.IsTrue(0 == listingsResult.Count);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task PostListingForUserAsync()
        {
            var listing = new Listing
            {
                UserId = AssemblyInit.testUserId,
                Title = "Some Title",
                ListingType = "Some Type",
                ListingSubType = "Some Sub Type"
            };

            var response = await AssemblyInit._client.PostAsync("api/listings", new StringContent(JsonConvert.SerializeObject(listing), Encoding.UTF8, "application/json"));
            Assert.IsTrue(response.IsSuccessStatusCode);

            response = await AssemblyInit._client.GetAsync("api/listings");
            Assert.IsTrue(response.IsSuccessStatusCode);

            var stream = await response.Content.ReadAsStringAsync();
            var listingsResult = JsonConvert.DeserializeObject<List<Listing>>(stream, new JsonApiSerializerSettings());
            Assert.IsTrue(1 == listingsResult.Count);

            response = await AssemblyInit._client.GetAsync($"api/listings/{listingsResult[0].Id}");

            // Upload File and path listing
            response = await AssemblyInit._client.PatchAsync("api/listings", 
                new StringContent(JsonConvert.SerializeObject(listing), Encoding.UTF8, "application/json"));


            response = await AssemblyInit._client.DeleteAsync($"api/listings/{listingsResult[0].Id}");
            Assert.IsTrue(response.IsSuccessStatusCode);

            response = await AssemblyInit._client.DeleteAsync($"api/listings/{listingsResult[0].Id}");
            Assert.IsFalse(response.IsSuccessStatusCode);
            Assert.IsTrue(HttpStatusCode.NotFound == response.StatusCode);
            //await Assert.ThrowsExceptionAsync<DocumentClientException>(() => AssemblyInit._client.DeleteAsync($"api/listings/{listingsResult[0].Id}"));
        }
    }
}
