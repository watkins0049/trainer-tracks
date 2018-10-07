using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols;
using System.Configuration;
using Microsoft.Extensions.Options;

namespace TrainerTracks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        private readonly IOptions<TrainerTracksConfig> config;

        public ValuesController(IOptions<TrainerTracksConfig> config)
        {
            this.config = config;
        }

        // GET: /<controller>/
        //public IActionResult Index() => View(config.Value);

        //private IActionResult View(MyConfig value)
        //{
        //    throw new NotImplementedException();
        //}

        [HttpGet("HelloWorld")]
        public IEnumerable<string> HelloWorld()
        {
            return new string[] { "Hello World!" };
        }

        [HttpGet("DbConnection")]
        public string DbConnection()
        {

            var conn = config.Value.ConnectionString;
            return conn;
        }
    }
}
