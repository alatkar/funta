// <copyright company="PartyFinds LLC">
//   Copyright (c) PartyFinds LLC.  All rights reserved
// </copyright>

using JsonApiSerializer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;

namespace PartyFindsApiUnitTests.Service
{
    [TestClass]
    public class FilesControllerTest
    {
        [TestMethod]
        public async System.Threading.Tasks.Task UploadDownloadImageFileAsync()
        {
            string path = Directory.GetCurrentDirectory();


            var pathItems = path.Split(Path.DirectorySeparatorChar);
            var pos = pathItems.Reverse().ToList().FindIndex(x => string.Equals("bin", x));
            string projectPath = String.Join(Path.DirectorySeparatorChar.ToString(), pathItems.Take(pathItems.Length - pos - 1));
            path =  Path.Combine(projectPath, "TestData", "all-ages-flowers.jpg");

            using (var form = new MultipartFormDataContent())
            using (var stream = File.OpenRead(path))
            {
                stream.Position = 0;
                using (var streamContent = new StreamContent(stream))
                {
                    form.Add(streamContent, "file", "all-ages-flowers.jpg"); // File name is needed. Don't know why!!
                    form.Add(new StringContent(AssemblyInit.testUserId), "userId");
                    var response = await AssemblyInit._client.PostAsync("api/files", form);
                    Assert.IsTrue(response.IsSuccessStatusCode);
                    var respStream = await response.Content.ReadAsStringAsync();

                    response = await AssemblyInit._client.GetAsync($"api/files/{AssemblyInit.testUserId}/{respStream}");
                    var downLoad = await response.Content.ReadAsStreamAsync();

                    string fileSave = $"{Path.Combine(projectPath, "TestData")}\\{respStream}";
                    using (var saveStrm = File.Create(fileSave))
                    {
                        downLoad.Seek(0, SeekOrigin.Begin);
                        downLoad.CopyTo(saveStrm);
                    }

                    File.Delete(fileSave);

                    response = await AssemblyInit._client.DeleteAsync($"api/files/{AssemblyInit.testUserId}/{respStream}");
                    Assert.IsTrue(response.IsSuccessStatusCode);
                }
            }
        }

        [TestMethod]
        public async System.Threading.Tasks.Task UploadInMemoryTextFileAsync()
        {
            string fileContents = "This is a sample string";

            byte[] data = System.Text.Encoding.ASCII.GetBytes(fileContents);
            using (var form = new MultipartFormDataContent())
            using (var stream = new System.IO.MemoryStream())
            {
                stream.Write(data, 0, data.Length);
                stream.Position = 0;
                using (var streamContent = new StreamContent(stream))
                {
                    form.Add(streamContent, "file", "testfile.ext"); // File name is needed. Don't know why!!
                    form.Add(new StringContent(AssemblyInit.testUserId), "userId");
                    var response = await AssemblyInit._client.PostAsync("api/files", form);

                    Assert.IsTrue(response.IsSuccessStatusCode);

                    var respFileUri = await response.Content.ReadAsStringAsync();
                    response = await AssemblyInit._client.GetAsync($"api/files/{AssemblyInit.testUserId}/{respFileUri}");
                    var downLoad = await response.Content.ReadAsStringAsync();

                    Assert.AreEqual(fileContents, downLoad);

                    response = await AssemblyInit._client.DeleteAsync($"api/files/{AssemblyInit.testUserId}/{respFileUri}");
                    Assert.IsTrue(response.IsSuccessStatusCode);

                    response = await AssemblyInit._client.DeleteAsync($"api/files/{AssemblyInit.testUserId}/{respFileUri}");
                    Assert.IsFalse(response.IsSuccessStatusCode);
                    Assert.IsTrue(HttpStatusCode.NotFound == response.StatusCode);

                    response = await AssemblyInit._client.DeleteAsync($"api/files?fileName=testfile.ext&userName={AssemblyInit.testUserId}");
                }
            }
        }
    }
}
