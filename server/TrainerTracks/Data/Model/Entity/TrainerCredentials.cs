using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace TrainerTracks.Data.Model.Entity
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
    }
}
