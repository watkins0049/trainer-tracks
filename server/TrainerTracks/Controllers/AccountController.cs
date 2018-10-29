using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq;
using TrainerTracks.Data.Context;
using TrainerTracks.Data.Model;
using TrainerTracks.Data.Model.DTO;
using TrainerTracks.Data.Model.Entity;
using TrainerTracks.Security;

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
            var parameters = new Npgsql.NpgsqlParameter[] { new Npgsql.NpgsqlParameter { Value = user.emailAddress } };
            var test = this.context.ExecuteProcedure<CredentialsDTO>("GetUserLoginCredentials", parameters);

            var pwTest = PasswordHashingHelpers.VerifyPassword(user.password, test.Password);

            return null;
        }
    }
}
