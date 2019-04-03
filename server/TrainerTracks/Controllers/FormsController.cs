using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using TrainerTracks.Web.Data.Context;
using TrainerTracks.Data.Model;
using TrainerTracks.Data.Model.DTO.Forms;
using TrainerTracks.Security;

namespace TrainerTracks.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Trainer")]
    public class FormsController : Controller
    {
        private readonly IOptions<TrainerTracksConfig> config;
        private readonly AccountContext context;

        public FormsController(IOptions<TrainerTracksConfig> config, AccountContext context)
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

        [HttpGet("downloadTrainerForm")]
        public IActionResult DownloadTrainerForm(string formName)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var trainerId = identity.FindFirst("TrainerId").Value;

            formName = formName.SanitizeFileName();
            var fullFilePath = Path.Combine(config.Value.BaseTrainerFormsDirectory, trainerId, formName);

            var stream = new FileStream(fullFilePath, FileMode.Open);
            return File(stream, "application/pdf", formName);
        }
    }
}