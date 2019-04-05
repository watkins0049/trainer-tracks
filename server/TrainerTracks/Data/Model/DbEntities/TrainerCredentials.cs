using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using TrainerTracks.Web.Data.Model.DTO.Account;

namespace TrainerTracks.Data.Model.Entity.DBEntities
{
    public class TrainerCredentials
    {
        [Key]
        public string EmailAddress { get; set; }

        public string Hash { get; set; }
        public string Salt { get; set; }

        public bool IsTrainerAuthorized(string password)
        {
            string hashedPassword = HashStringSHA512(password);
            return BCrypt.Net.BCrypt.Verify(hashedPassword, Hash);
        }

        public static TrainerCredentials BuildNewTrainerCredentials(UserSignupDTO user)
        {
            TrainerCredentials trainerCredentials =  new TrainerCredentials
            {
                EmailAddress = user.EmailAddress
            };
            trainerCredentials.Salt = BCrypt.Net.BCrypt.GenerateSalt();
            trainerCredentials.Hash = trainerCredentials.HashPassword(user.Password);

            return trainerCredentials;
        }

        private string HashPassword(string password)
        {
            if (IsPasswordValid(password))
            {
                string sha512 = HashStringSHA512(password);
                return BCrypt.Net.BCrypt.HashPassword(sha512, Salt);
            }
            throw new ArgumentException("Password must be at least 8 characters, have 1 uppercase character, 1 lowercase character, and 1 number.");
        }

        // taken from https://stackoverflow.com/questions/11367727/how-can-i-sha512-a-string-in-c
        private string HashStringSHA512(string input)
        {
            using (SHA512 hash = SHA512.Create())
            {
                byte[] hashedInputBytes = hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(hashedInputBytes).Replace("-", "");
            }
        }

        private bool IsPasswordValid(string password)
        {
            if (password == null)
            {
                return false;
            }

            bool is8CharactersLong = password.Length >= 8;

            Regex atLeastOneUppercase = new Regex("(?=.*[A-Z])");
            bool passwordContainsOneUppercase = atLeastOneUppercase.IsMatch(password);

            Regex atLeastOneLowercase = new Regex("(?=.*[a-z])");
            bool passwordContainsOneLowercase = atLeastOneLowercase.IsMatch(password);

            Regex atLeastOneNumber = new Regex("(?=.*[0-9])");
            bool passwordContainsOneNumber = atLeastOneNumber.IsMatch(password);

            return is8CharactersLong && passwordContainsOneUppercase && passwordContainsOneLowercase && passwordContainsOneNumber;

        }
    }
}
