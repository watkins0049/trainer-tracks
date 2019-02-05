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
            //var userCredentials = this.context.ExecuteProcedure<CredentialsDTO>("GetUserLoginCredentials", user.emailAddress);

            //if (userCredentials == null || userCredentials.Salt == null || userCredentials.Password == null)
            //{
            //    throw new Exception(this.INVALID_CREDENTIALS_ERROR_MESSAGE);
            //}

            //var pwTest = PasswordHashingHelpers.VerifyPassword(user.password, userCredentials.Password);

            //if (!pwTest)
            //{
            //    throw new Exception(this.INVALID_CREDENTIALS_ERROR_MESSAGE);
            //}

            //var trainer = this.context.Trainer.Where(t => t.EmailAddress.Equals(user.emailAddress)).First();

            //var claims = AccountServices.SetupClaims(trainer);
            //var token = AccountServices.GenerateSecurityToken(claims, this.config.Value.JwtKey);

            //UserClaimsDTO claimsDto = new UserClaimsDTO()
            //{
            //    Claims = claims,
            //    Token = token
            //};

            //return claimsDto;
        }
    }
}
