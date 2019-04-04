using TrainerTracks.Data.Model.Entity;
using Xunit;

namespace TrainerTracks.Web.Test.Data.Model.Entity
{
    public class TrainerCredentialsShould
    {
        private const string PASSWORD1234_SALT = "$2b$10$sCfS.t4SiS21G9rhNcqKue";
        private const string PASSWORD1234_HASH = "$2b$10$sCfS.t4SiS21G9rhNcqKue/PkEiitv/OfB0DojqdkMQneiUQw0l06";

        /// <summary>
        /// GIVEN a TrainderCredentials object
        /// AND a password presented by the user
        /// WHEN a user is attempting to authenticate
        /// THEN return true indicating the user is authorized
        /// </summary>
        [Fact]
        public void ReturnTrueForAuthorizedUser()
        {
            // GIVEN a TrainderCredentials object
            TrainerCredentials trainerCredentials = new TrainerCredentials
            {
                EmailAddress = "test@user.com",
                Hash = PASSWORD1234_HASH,
                Salt = PASSWORD1234_SALT
            };
            // AND a password presented by the user
            string password = "Password1234";

            // WHEN a user is attempting to authenticate
            bool isUserAuthorized = trainerCredentials.IsTrainerAuthorized(password);

            // THEN return true indicating the user is authorized
            Assert.True(isUserAuthorized);
        }

        /// <summary>
        /// GIVEN a TrainderCredentials object
        /// AND a password presented by the user
        /// WHEN a user is attempting to authenticate
        /// THEN return false indicating the user is not authorized
        /// </summary>
        [Fact]
        public void ReturnFalseForAuthorizedUser()
        {
            // GIVEN a TrainderCredentials object
            TrainerCredentials trainerCredentials = new TrainerCredentials
            {
                EmailAddress = "test@user.com",
                Hash = PASSWORD1234_HASH,
                Salt = PASSWORD1234_SALT
            };
            // AND a password presented by the user
            string password = "password1234";

            // WHEN a user is attempting to authenticate
            bool isUserAuthorized = trainerCredentials.IsTrainerAuthorized(password);

            // THEN return true indicating the user is authorized
            Assert.False(isUserAuthorized);
        }
    }
}
