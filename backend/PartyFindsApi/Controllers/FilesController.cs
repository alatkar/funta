using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using PartyFindsApi.core;
using PartyFindsApi.Models;

namespace PartyFindsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        BlobContainerClient uploadsContainer;

        public FilesController()
        {
            this.uploadsContainer = Container.Instance.uploadsContainer;
        }

        // GET: api/Files
        [HttpGet]
        public IActionResult Get()
        {
            return StatusCode(501);
        }

        // GET: api/Files/5
        [HttpGet("{id}")]
        public IActionResult Get(int uri)
        {
            return StatusCode(501);
        }

        // POST: api/Files
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromForm] MediaResource resource)
        {
            // TODO: Decide policy to store files without user
            string path = string.IsNullOrEmpty(resource.UserId) ? "garbage" : $"{resource.UserId}/";
            path = $"{path}{resource.File.FileName}";

            try
            {
                // Get a reference to a blob
                BlobClient blobClient = this.uploadsContainer.GetBlobClient($"{path}");
                //TODO: Delete policy if the blob exists
                await blobClient.UploadAsync(resource.File.OpenReadStream());
                return Ok($"{blobClient.Uri}");
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        // PUT: api/Files/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
