using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
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

        // taken from https://stackoverflow.com/questions/11367727/how-can-i-sha512-a-string-in-c
        private string HashStringSHA512(string input)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            using (SHA512 hash = SHA512.Create())
            {
                byte[] hashedInputBytes = hash.ComputeHash(bytes);
                return BitConverter.ToString(hashedInputBytes).Replace("-", "");
            }
        }

        public static TrainerCredentials BuildNewUser(UserSignupDTO user)
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
            string sha512 = HashStringSHA512(password);
            return BCrypt.Net.BCrypt.HashPassword(sha512, Salt);
        }
    }
}
