using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Security.Claims;
using TrainerTracks.Data.Enums;
using TrainerTracks.Web.Data.Model.DTO.Account;
using TrainerTracks.Web.Data.Model.Entity;
using TrainerTracks.Web.Exceptions;

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

        public static Trainer BuildTrainerFromUserSignup(UserSignupDTO user)
        {
            // No need for a regex; this automagically validates the user's
            // email address and throws a FormatException if it's not valid
            MailAddress m = new MailAddress(user.EmailAddress);

            if (string.IsNullOrWhiteSpace(user.LastName) || string.IsNullOrWhiteSpace(user.FirstName))
            {
                throw new ArgumentException("First name and last name are required.");
            }

            if (!user.EmailAddress.Equals(user.ConfirmEmailAddress))
            {
                throw new UserSignupException("Email addresses do not match.");
            }

            return new Trainer
            {
                EmailAddress = user.EmailAddress,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }
    }
}
