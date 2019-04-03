using TrainerTracks.Web.Data.Context;
using TrainerTracks.Data.Model.Entity;

namespace TrainerTracks.Web.Test.Controller.IT
{
    public static class SeedData
    {
        public static void PopulateTestData(AccountContext dbContext)
        {
            dbContext.Trainer.Add(new Trainer
            {
                EmailAddress = "test@user.com",
                FirstName = "Test",
                LastName = "User"
            });
            dbContext.TrainerCredentials.Add(new TrainerCredentials
            {
                EmailAddress = "test@user.com",
                // the hash of the SHA512 hash of "password1234"
                Hash = "$2b$10$sCfS.t4SiS21G9rhNcqKuemSpI8sU/F6z59x.aZimKouY2qLFp69.",
                Salt = "$2b$10$sCfS.t4SiS21G9rhNcqKue"
            });
            dbContext.SaveChanges();
        }
    }
}
