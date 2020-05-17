using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClientService.Data.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ClientService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientQueries _clientQueries;
        private readonly ILogger<ClientController> _logger;

        public ClientController(IClientQueries clientQueries, ILogger<ClientController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _clientQueries = clientQueries ?? throw new ArgumentNullException(nameof(clientQueries));
        }
        // GET: api/Client
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Client/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<ActionResult<IEnumerable<Facility>>> GetFacilityById(string id)
        {
            Int64 clientId = Convert.ToInt64(id);
            var result = await _clientQueries.GetFacilityByClientId(clientId);
            return new JsonResult(result);
        }

        // POST: api/Client
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Client/5
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
