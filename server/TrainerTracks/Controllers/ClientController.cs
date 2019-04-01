using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using TrainerTracks.Data.Context;
using TrainerTracks.Data.Model;
using TrainerTracks.Data.Model.DTO.Forms;
using TrainerTracks.Data.Model.Entity;
using TrainerTracks.Security;

namespace TrainerTracks.Web.Controllers
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

            var results = context.TrainerClients
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
            var results = context.Client.Where(c => c.ClientId == clientId).FirstOrDefault();
            return results;
        }

        [HttpPost("saveClient")]
        public void SaveClient(Client client)
        {
            context.Client.Update(client);
            context.SaveChanges();
        }

        [HttpGet("clientForms")]
        public IEnumerable<FormsDTO> ClientForms(int clientId)
        {
            string directory = Path.Combine(config.Value.BaseClientFormsDirectory, clientId.ToString());
            IEnumerable<FormsDTO> fileEntries = Directory.GetFiles(directory)
                .Select(f => new FormsDTO() { FormName = Path.GetFileName(f) })
                .AsEnumerable();

            return fileEntries;
        }

        [HttpGet("downloadClientForm")]
        public IActionResult DownloadTrainerForm(int clientId, string formName)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var trainerId = identity.FindFirst("TrainerId").Value;

            formName = formName.SanitizeFileName();
            var fullFilePath = Path.Combine(config.Value.BaseClientFormsDirectory, clientId.ToString(), formName);

            var stream = new FileStream(fullFilePath, FileMode.Open);
            return File(stream, "application/pdf", formName);
        }
    }
}
