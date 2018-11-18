using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using TrainerTracks.Data.Context;
using TrainerTracks.Data.Model;
using TrainerTracks.Data.Model.Entity;

namespace TrainerTracks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Trainer")]
    public class ClientController : ControllerBase
    {
        private readonly IOptions<TrainerTracksConfig> config;
        private readonly TrainerTracksContext context;

        public ClientController(IOptions<TrainerTracksConfig> config, TrainerTracksContext context)
        {
            this.config = config;
            this.context = context;
        }

        [HttpGet("searchClients")]
        public IEnumerable<TrainerClients> SearchClients(string firstName, string lastName)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var trainerId = identity.FindFirst("TrainerId").Value;

            var results = this.context.TrainerClients
                .Include(tc => tc.Client)
                .AsEnumerable()
                .Where(c => (firstName == null || c.Client.FirstName.ToUpper().Contains(firstName.ToUpper())) &&
                    (lastName == null || c.Client.LastName.ToUpper().Contains(lastName.ToUpper())) &&
                    c.TrainerId.Equals(Convert.ToInt64(trainerId)));

            return results;
        }

        [HttpGet("clientDetails")]
        public Client ClientDetails(int clientId)
        {
            var results = this.context.Client.Where(c => c.ClientId == clientId).FirstOrDefault();
            return results;
        }
    }
}
