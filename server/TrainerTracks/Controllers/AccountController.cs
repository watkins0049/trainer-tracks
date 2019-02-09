using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TrainerTracks.Data.Model;
using TrainerTracks.Data.Model.DTO.Account;
using TrainerTracks.Services;

namespace TrainerTracks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IOptions<TrainerTracksConfig> config;
        private readonly IAccountServices accountServices;

        public AccountController(IOptions<TrainerTracksConfig> config, IAccountServices accountServices)
        {
            this.config = config;
            this.accountServices = accountServices;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public UserClaimsDTO Login(UserDTO user)
        {
            UserClaimsDTO userClaims = accountServices.SetupUserClaims(user);
            return userClaims;
        }
    }
}
