namespace TrainerTracks.Security
{
    public class PasswordHashingHelpers
    {

        public string GenerateSalt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt();
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

    }
}
