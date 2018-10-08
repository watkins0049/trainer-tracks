using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using TrainerTracks.Data.Context;
using TrainerTracks.Data.Model;

namespace TrainerTracks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IOptions<TrainerTracksConfig> config;
        private readonly TrainerTracksContext context;

        public ValuesController(IOptions<TrainerTracksConfig> config, TrainerTracksContext context)
        {
            this.config = config;
            this.context = context;
        }

        [HttpGet("HelloWorld")]
        public IEnumerable<string> HelloWorld()
        {
            return new string[] { "Hello World!" };
        }

        [HttpGet("Test")]
        public Trainer Test()
        {
            Trainer conn = context.Trainer.Find("email_address_here");
            return conn;
        }
    }
}
