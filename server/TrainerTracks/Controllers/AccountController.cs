﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TrainerTracks.Data.Model;
using TrainerTracks.Data.Model.DTO.Account;
using TrainerTracks.Web.Services;

namespace TrainerTracks.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountServices accountServices;

        public AccountController(IAccountServices accountServices)
        {
            this.accountServices = accountServices;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public UserClaimsDTO Login([FromBody] UserDTO user)
        {
            return accountServices.AuthorizeTrainer(user);
        }
    }
}
