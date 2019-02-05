using System.Collections.Generic;
using System.Security.Claims;
using TrainerTracks.Data.Model.DTO.Account;
using TrainerTracks.Services;
using Xunit;
using Moq;
using TrainerTracks.Data.Dao;
using TrainerTracks.Data.Model.Entity;

namespace TrainerTracks.Test.Services
{
    public class AccountServicesShould
    {
        private readonly Mock<UserDao> userDaoMock = new Mock<UserDao>();
        private readonly AccountServices accountServices;

        public AccountServicesShould()
        {
            accountServices = new AccountServices(userDaoMock.Object);
        }

        /// <summary>
        /// GIVEN a UserDTO containing a user's e-mail and password
        /// WHEN the user is correctly authenticated
        /// AND the user's login information is returned from the database
        /// THEN return a UserClaimsDTO containing an e-mail claim with the
        /// user's e-mail, a name claim with the user's full name, a role claim
        /// of trainer, and a TrainerId claim with the trainer's ID
        /// AND a non-null Token
        /// </summary>
        [Fact]
        public void ReturnUserClaimsDTOForAuthenticatedUser()
        {
            // GIVEN a UserDTO containing a user's e-mail and password
            UserDTO user = new UserDTO()
            {
                emailAddress = "test@user.com",
                password = "password1234"
            };

            // WHEN the user is correctly authenticated
            userDaoMock.Setup(t => t.IsUserAuthenticated(user)).Returns(true);
            // AND the user's login information is returned from the database
            Trainer mockTrainer = new Trainer()
            {
                TrainerId = 1,
                EmailAddress = "test@user.com",
                FirstName = "Test",
                LastName = "User"
            };
            userDaoMock.Setup(t => t.RetrieveUserInformation(user)).Returns(mockTrainer);
            UserClaimsDTO userClaims = accountServices.SetupUserClaims(user);

            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Email, "test@user.com"),
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Role, "Trainer"),
                new Claim("TrainerId", "1")
            };

            // THEN return a UserClaimsDTO containing an e-mail claim with the
            // user's e-mail, a name claim with the user's full name, a role claim
            // of trainer, and a TrainerId claim with the trainer's ID
            for (int i=0; i<claims.Count; i++)
            {
                Assert.Equal(claims[i].GetType(), userClaims.Claims[i].GetType());
                Assert.Equal(claims[i].Value, userClaims.Claims[i].Value);
            }

            // AND a non-null Token
            Assert.NotNull(userClaims.Token);
        }
    }
}
