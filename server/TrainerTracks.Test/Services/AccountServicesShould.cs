using System.Collections.Generic;
using System.Security.Claims;
using TrainerTracks.Data.Model.DTO.Account;
using TrainerTracks.Web.Services;
using Xunit;
using Moq;
using TrainerTracks.Data.Model.Entity;
using TrainerTracks.Data.Enums;
using System;
using System.IdentityModel.Tokens.Jwt;
using TrainerTracks.Data.Repository;
using TrainerTracks.Data.Model;

namespace TrainerTracks.Test.Services
{
    public class AccountServicesShould
    {
        private readonly Mock<ITrainerRepository> trainerRepositoryMock = new Mock<ITrainerRepository>();
        private readonly Mock<ITrainerCredentialsRepository> trainerCredentialsRepositoryMock = new Mock<ITrainerCredentialsRepository>();
        private readonly Mock<TrainerTracksConfig> configMock = new Mock<TrainerTracksConfig>();
        private readonly AccountServices accountServices;

        public AccountServicesShould()
        {
            accountServices = new AccountServices(trainerRepositoryMock.Object,
                trainerCredentialsRepositoryMock.Object,
                configMock.Object);
        }

        /// <summary>
        /// GIVEN a UserDTO containing a user's e-mail and password
        /// WHEN the user is correctly authenticated
        /// AND the user's login information is returned from the database
        /// THEN return a UserClaimsDTO containing an e-mail claim with the
        /// user's e-mail, a name claim with the user's full name, a role claim
        /// of trainer, and a TrainerId claim with the trainer's ID
        /// AND an encrypted Token
        /// </summary>
        [Fact]
        public void ReturnUserClaimsDTOForAuthenticatedUser()
        {
            // GIVEN a UserDTO containing a user's e-mail and password
            UserDTO user = new UserDTO
            {
                EmailAddress = "test@user.com",
                Password = "password1234"
            };

            // WHEN the user is correctly authenticated
            // AND the user's login information is returned from the database
            Trainer mockTrainer = new Trainer
            {
                TrainerId = 1,
                EmailAddress = "test@user.com",
                FirstName = "Test",
                LastName = "User"
            };
            trainerRepositoryMock.Setup(t => t.GetTrainerByEmail(mockTrainer.EmailAddress)).Returns(mockTrainer);

            TrainerCredentials mockTrainerCredentials = new TrainerCredentials
            {
                TrainerId = 1,
                // the hash of the SHA512 hash of "password1234"
                Hash = "$2b$10$sCfS.t4SiS21G9rhNcqKuemSpI8sU/F6z59x.aZimKouY2qLFp69.",
                Salt = "$2b$10$sCfS.t4SiS21G9rhNcqKue"
            };
            trainerCredentialsRepositoryMock.Setup(c => c.GetById(mockTrainer.TrainerId)).Returns(mockTrainerCredentials);

            configMock.Setup(c => c.JwtKey).Returns("fc5a6707-634b-4776-ba70-6f6cc45fbcfc");

            UserClaimsDTO userClaims = accountServices.SetupUserClaims(user);

            // THEN return a UserClaimsDTO containing an e-mail claim with the
            // user's e-mail, a name claim with the user's full name, a role claim
            // of trainer, and a TrainerId claim with the trainer's ID
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Email, "test@user.com"),
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Role, UserRole.TRAINER.ToString()),
                new Claim("TrainerId", "1")
            };

            for (int i = 0; i < claims.Count; i++)
            {
                Assert.Equal(claims[i].GetType(), userClaims.Claims[i].GetType());
                Assert.Equal(claims[i].Value, userClaims.Claims[i].Value);
            }

            // AND an encrypted Token
            var handler = new JwtSecurityTokenHandler();
            var decodedClaims = handler.ReadToken(userClaims.Token) as JwtSecurityToken;
            Assert.NotNull(decodedClaims);
        }

        /// <summary>
        /// GIVEN a UserDTO containing a user's e-mail and password
        /// WHEN a user is not correctly authenticated
        /// THEN an UnauthorizedAccessException is thrown
        /// AND the message reads "Username or password is incorrect."
        /// </summary>
        [Fact]
        public void ThrowAnUnauthorizedAccessExceptionWhenAUserIsNotAuthenticated()
        {
            // GIVEN a UserDTO containing a user's e-mail and password
            UserDTO user = new UserDTO
            {
                EmailAddress = "test@user.com",
                Password = "Password1234"
            };
            UnauthorizedAccessException mockException = new UnauthorizedAccessException("Username or password is incorrect.");

            // WHEN a user is not correctly authenticated
            Trainer mockTrainer = new Trainer
            {
                TrainerId = 1,
                EmailAddress = "test@user.com",
                FirstName = "Test",
                LastName = "User"
            };
            trainerRepositoryMock.Setup(t => t.GetTrainerByEmail(mockTrainer.EmailAddress)).Returns(mockTrainer);

            TrainerCredentials mockTrainerCredentials = new TrainerCredentials
            {
                TrainerId = 1,
                // NOTE: the hash should be "$2b$10$sCfS.t4SiS21G9rhNcqKue/PkEiitv/OfB0DojqdkMQneiUQw0l06"
                // the hash of the SHA512 hash of "password1234"
                Hash = "$2b$10$sCfS.t4SiS21G9rhNcqKuemSpI8sU/F6z59x.aZimKouY2qLFp69.",
                Salt = "$2b$10$sCfS.t4SiS21G9rhNcqKue"
            };
            trainerCredentialsRepositoryMock.Setup(c => c.GetById(mockTrainer.TrainerId)).Returns(mockTrainerCredentials);

            // THEN ensure an UnauthorizedAccessException is thrown
            UnauthorizedAccessException ex = Assert.Throws<UnauthorizedAccessException>(() => accountServices.SetupUserClaims(user));
            // AND ensure the message reads "Username or password is incorrect."
            Assert.Equal("Username or password is incorrect.", mockException.Message);
        }
    }
}
