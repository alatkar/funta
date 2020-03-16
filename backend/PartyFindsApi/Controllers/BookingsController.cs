using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace PartyFindsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly ILogger logger;

        // GET: api/Bookings
        [HttpGet]
        public IActionResult Get()
        {
            return StatusCode(501);
        }

        // GET: api/Bookings/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return StatusCode(501);
        }

        // POST: api/Bookings
        [HttpPost]
        public IActionResult Post([FromBody] string value)
        {
            return StatusCode(501);
        }

        // PUT: api/Bookings/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] string value)
        {
            return StatusCode(501);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return StatusCode(501);
        }
    }
}
