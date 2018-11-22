using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using TrainerTracks.Data.Context;
using TrainerTracks.Data.Model;
using TrainerTracks.Data.Model.DTO.Forms;

namespace TrainerTracks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Trainer")]
    public class FormsController : Controller
    {
        private readonly IOptions<TrainerTracksConfig> config;
        private readonly TrainerTracksContext context;

        public FormsController(IOptions<TrainerTracksConfig> config, TrainerTracksContext context)
        {
            this.config = config;
            this.context = context;
        }

        [HttpGet("trainerForms")]
        public IEnumerable<FormsDTO> TrainerForms()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var trainerId = identity.FindFirst("TrainerId").Value;

            string directory = config.Value.BaseTrainerFormsDirectory + trainerId;
            IEnumerable<FormsDTO> fileEntries = Directory.GetFiles(directory)
                .Select(f => new FormsDTO() { FormName = Path.GetFileName(f) })
                .AsEnumerable();

            return fileEntries;
        }
    }
}