// <copyright company="PartyFinds LLC">
//   Copyright (c) PartyFinds LLC.  All rights reserved
// </copyright>

using JsonApiSerializer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PartyFindsApi.Models;
using System;
using System.IO;
using System.Net.Http;
using System.Text;

namespace PartyFindsApiUnitTests.Scenario
{
    [TestClass]
    public class LoginScenariosTest
    {
        [TestMethod]
        public async System.Threading.Tasks.Task RegisterUserUpdateProfile()
        {
            Random random = new Random();
            // Create Account
            var obj = new User
            {
                Email = "LoginScenariosTest@aa.com",
                PasswordHash = "Password"
            };

            var response = await AssemblyInit._client.PostAsync("api/register", new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json"));
            Assert.IsTrue(response.IsSuccessStatusCode);

            var stream = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<User>(stream, new JsonApiSerializerSettings());

            response = await AssemblyInit._client.PostAsync("api/register", new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json"));
            Assert.IsFalse(response.IsSuccessStatusCode);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);

            response = await AssemblyInit._client.DeleteAsync($"api/users/{user.Id}");
            Assert.IsTrue(response.IsSuccessStatusCode);
        }
    }
}
