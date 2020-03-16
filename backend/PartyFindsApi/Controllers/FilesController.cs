// <copyright company="PartyFinds LLC">
//   Copyright (c) PartyFinds LLC.  All rights reserved
// </copyright>

using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Logging;
using PartyFindsApi.core;
using PartyFindsApi.Models;

namespace PartyFindsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly ILogger logger;
        BlobContainerClient uploadsContainer;
        IRepository userRepo;

        public FilesController(ILogger<FilesController> logger)
        {
            this.logger = logger;
            this.uploadsContainer = Container.Instance.uploadsContainer;
            this.userRepo = Container.Instance.userRepo;
        }

        // GET: api/Files
        [HttpGet("{UserName}/{fileId}")]
        public async Task<IActionResult> GetAsync(string userName, string fileId)
        {
            try
            {
                BlobClient blobClient = this.uploadsContainer.GetBlobClient($"{userName}/{fileId}");
                BlobProperties props = await blobClient.GetPropertiesAsync().ConfigureAwait(false);
                MemoryStream blobStream = new MemoryStream();
                
                var file = await blobClient.DownloadToAsync(blobStream).ConfigureAwait(false);
                blobStream.Position = 0;

                return File(blobStream, props.ContentType);
            }
            catch (Azure.RequestFailedException ex)
            {
                logger.LogError($"Received Exception {ex}");
                if (ex.Status == (int)HttpStatusCode.NotFound)
                {
                    return NotFound(ex);
                }
                return StatusCode(501);
            }
            catch (Exception ex)
            {
                logger.LogError($"File not present for the resource {ex}");
                return StatusCode(500, ex);
            }
        }

        // POST: api/Files
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromForm] MediaResource resource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Decide policy to store files without user
            if (string.IsNullOrEmpty(resource?.UserId))
            {
                logger.LogError($"Userid not present for the resource {resource.UserId}");
                throw new ArgumentException($"Please provide filename parameter. Resource provided is {resource}", nameof(resource));
            }

            if (resource?.File == null)
            {
                logger.LogError($"File not present for the resource {resource.File}");
                throw new ArgumentException($"Please provide file. Resource provided is {resource}", nameof(resource));
            }

            // Check if user exists
            var feed = new RequestOptions { PartitionKey = new PartitionKey(resource.UserId) };
            var resp = await userRepo.GetAsync<Models.User>(resource.UserId, feed).ConfigureAwait(false);
            if (resp == null)
            {
                return NotFound($"User {resource.UserId} not found");
            }

            string path = string.IsNullOrEmpty(resource?.UserId) ? "garbage" : $"{resource.UserId}/";
            //path = $"{path}{resource.File.FileName}";
            
            string fileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(resource.File.FileName)}";
            path = $"{path}{fileName}";

            logger.LogInformation($"Saving file {resource.UserId} for user {resource.UserId}");

            try
            {
                // Get a reference to a blob
                BlobClient blobClient = this.uploadsContainer.GetBlobClient($"{path}");
                //TODO: Delete policy if the blob exists
                await blobClient.UploadAsync(Request.Form.Files[0].OpenReadStream()).ConfigureAwait(false);
                //return Ok($"{blobClient.Uri.AbsolutePath}");
                return Ok($"{fileName}");
            }
            catch (Exception ex)
            {
                logger.LogError($"PostAsync failed with exception {ex}");
                return StatusCode(500, ex);
            }
        }

        [HttpDelete("{UserId}/{fileId}")]
        public async Task<IActionResult> DeleteFileByIdAsync(string userId, string fileId)
        {
            if (string.IsNullOrEmpty(fileId))
            {
                logger.LogError($"FileName not present {fileId} UserName {fileId}");
                throw new ArgumentException($"Please provide filename to delete {fileId}", nameof(fileId));
            }

            if (string.IsNullOrEmpty(userId))
            {
                logger.LogError($"UserName not present {userId} UserName {userId}");
                throw new ArgumentException($"Please provide userName to delete {userId}", nameof(userId));
            }

            try
            {
                string fileToDelete = $"{userId}/{fileId}";
                logger.LogInformation($"Deleting file {fileToDelete} for user {fileId}");
                BlobClient blobClient = this.uploadsContainer.GetBlobClient(fileToDelete);
                BlobProperties props = await blobClient.GetPropertiesAsync().ConfigureAwait(false);
                var str = blobClient.Uri;
                blobClient.DeleteIfExists();
                logger.LogInformation($"Deleted file  file {blobClient.Uri}");
                return Ok($"{blobClient.Uri}");
            }
            catch (Azure.RequestFailedException ex)
            {
                logger.LogError($"Received Exception {ex}");
                if (ex.Status == (int)HttpStatusCode.NotFound)
                {
                    return NotFound(ex);
                }
                return StatusCode(501);
            }
        }
    }
}
