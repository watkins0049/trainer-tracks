using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TrainerTracks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        [HttpGet("HelloWorld")]
        public IEnumerable<string> HelloWorld()
        {
            return new string[] { "Hello World!" };
        }
    }
}
