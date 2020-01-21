using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PartyFindsApi.Controllers;
using PartyFindsApi.Models;
using System;
using System.IO;

namespace PartyFindsApiUnitTests
{
    [TestClass]
    public class FilesControllerTest
    {
        [TestMethod]
        public async System.Threading.Tasks.Task UploadFileAsync()
        {
            var controller = new FilesController();
            var media = new MediaResource();
            /*
            string path = @"D:\Example.txt";
            var stream = new System.IO.MemoryStream();

            FileStream SourceStream = File.Open(path, FileMode.Open);

            media.file = new FormFile(SourceStream, 0, SourceStream.Length, path);
            */

            string localPath = "C:/MyDev/del";
            string fileName = "quickstart" + Guid.NewGuid().ToString() + ".txt";
            string localFilePath = Path.Combine(localPath, fileName);

            localFilePath = "C:\\Iphone_2018_Photos\\IMG_7203.jpg";
            // Write text to the file
            //await File.WriteAllTextAsync(localFilePath, "Hello, World!");

            using FileStream uploadFileStream = File.OpenRead(localFilePath);
            media.File = new FormFile(uploadFileStream, 0, uploadFileStream.Length, "some", string.Empty);
            IActionResult res = await controller.PostAsync(media);
            Assert.IsNotNull(res);
        }
    }
}
