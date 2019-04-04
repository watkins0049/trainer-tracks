using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using TrainerTracks.Data.Enums;
using TrainerTracks.Web.Data.Model.Entity;

namespace TrainerTracks.Data.Model.Entity.DBEntities
{
    public class Trainer
    {
        [Key]
        public string EmailAddress { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? LastLoginDate { get; set; }

        public Claims GenerateClaims()
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, EmailAddress),
                new Claim(ClaimTypes.Name, FirstName + " " + LastName),
                new Claim(ClaimTypes.Role, UserRole.TRAINER.ToString())
            };

            return new Claims(claims);
        }
    }
}
