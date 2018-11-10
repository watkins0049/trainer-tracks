using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using TrainerTracks.Data.Context;
using TrainerTracks.Data.Model;
using TrainerTracks.Data.Model.DTO;
using TrainerTracks.Security;
using TrainerTracks.Services;

namespace TrainerTracks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private readonly IOptions<TrainerTracksConfig> config;
        private readonly TrainerTracksContext context;
        private readonly string INVALID_CREDENTIALS_ERROR_MESSAGE = "Email address or password is incorrect.";

        public AccountController(IOptions<TrainerTracksConfig> config, TrainerTracksContext context)
        {
            this.config = config;
            this.context = context;
        }


        [HttpPost("login")]
        public SecurityToken Login(UserDTO user)
        {
            var userCredentials = this.context.ExecuteProcedure<CredentialsDTO>("GetUserLoginCredentials", user.emailAddress);

            if (userCredentials == null || userCredentials.Salt == null || userCredentials.Password == null)
            {
                throw new Exception(this.INVALID_CREDENTIALS_ERROR_MESSAGE);
            }

            var pwTest = PasswordHashingHelpers.VerifyPassword(user.password, userCredentials.Password);

            if (!pwTest)
            {
                throw new Exception(this.INVALID_CREDENTIALS_ERROR_MESSAGE);
            }

            var trainer = this.context.Trainer.Where(t => t.EmailAddress.Equals(user.emailAddress)).First();
            var claims = AccountServices.SetupClaimsAsync(trainer);

            var token = AccountServices.GenerateSecurityToken(claims);

            return token;
        }
    }
}
