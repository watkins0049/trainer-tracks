using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;
using TrainerTracks.Data.Context;
using TrainerTracks.Data.Model;
using TrainerTracks.Data.Model.DTO;
using TrainerTracks.Data.Model.Entity;

namespace TrainerTracks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IOptions<TrainerTracksConfig> config;
        private readonly TrainerTracksContext context;

        public AccountController(IOptions<TrainerTracksConfig> config, TrainerTracksContext context)
        {
            this.config = config;
            this.context = context;
        }


        [HttpPost("login")]
        public Trainer Login(UserDTO user)
        {
            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            var hash = BCrypt.Net.BCrypt.HashPassword(user.password, salt);
            
            Trainer conn = context.Trainer.Where(t => t.EmailAddress.Equals(user.emailAddress)).FirstOrDefault();
            return conn;
        }
    }
}
