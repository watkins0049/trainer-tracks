using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using TrainerTracks.Data.Enums;

namespace TrainerTracks.Data.Model.Entity
{
    public class Trainer
    {
        [Key]
        public long TrainerId { get; set; }

        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? LastLoginDate { get; set; }

        public List<Claim> GenerateClaims()
        {
            List<Claim> result = new List<Claim>
            {
                new Claim(ClaimTypes.Email, EmailAddress),
                new Claim(ClaimTypes.Name, FirstName + " " + LastName),
                new Claim(ClaimTypes.Role, UserRole.TRAINER.ToString()),
                new Claim("TrainerId", TrainerId.ToString())
            };

            return result;
        }
    }
}
