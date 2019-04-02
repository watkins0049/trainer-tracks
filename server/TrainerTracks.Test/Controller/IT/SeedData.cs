using TrainerTracks.Data.Context;
using TrainerTracks.Data.Model.Entity;

namespace TrainerTracks.Web.Test.Controller.IT
{
    public static class SeedData
    {
        public static void PopulateTestData(AccountContext dbContext)
        {
            dbContext.Trainer.Add(new Trainer
            {
                TrainerId = 1,
                EmailAddress = "nick.watkins49@gmail.com",
                FirstName = "Nick",
                LastName = "Watkins"
            });
            dbContext.TrainerCredentials.Add(new TrainerCredentials
            {
                TrainerId = 1,
                // the hash of the SHA512 hash of "password1234"
                Hash = "$2b$10$sCfS.t4SiS21G9rhNcqKuemSpI8sU/F6z59x.aZimKouY2qLFp69.",
                Salt = "$2b$10$sCfS.t4SiS21G9rhNcqKue"
            });
            dbContext.SaveChanges();
        }
    }
}
