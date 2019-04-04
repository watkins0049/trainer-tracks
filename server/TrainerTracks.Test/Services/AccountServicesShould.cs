using System.Collections.Generic;
using System.Security.Claims;
using TrainerTracks.Data.Model.DTO.Account;
using TrainerTracks.Web.Services;
using Xunit;
using Moq;
using TrainerTracks.Data.Enums;
using System;
using System.IdentityModel.Tokens.Jwt;
using TrainerTracks.Data.Model;
using Microsoft.Extensions.Options;
using TrainerTracks.Web.Data.Context;
using TrainerTracks.Data.Model.Entity.DBEntities;

namespace TrainerTracks.Test.Services
{
    public class AccountServicesShould
    {
        private const string PASSWORD1234_HASH = "$2b$10$sCfS.t4SiS21G9rhNcqKuemSpI8sU/F6z59x.aZimKouY2qLFp69.";
        private const string PASSWORD1234_SALT = "$2b$10$sCfS.t4SiS21G9rhNcqKue";
        private const string JWT_KEY = "fc5a6707-634b-4776-ba70-6f6cc45fbcfc";

        private readonly Mock<IAccountContext> accountContextMock = new Mock<IAccountContext>();
        private readonly Mock<IOptions<TrainerTracksConfig>> configMock = new Mock<IOptions<TrainerTracksConfig>>();
        private readonly AccountServices accountServices;

        public AccountServicesShould()
        {
            accountServices = new AccountServices(accountContextMock.Object,
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
            TrainerCredentials mockTrainerCredentials = new TrainerCredentials
            {
                EmailAddress = "test@user.com",
                Hash = PASSWORD1234_HASH,
                Salt = PASSWORD1234_SALT
            };
            accountContextMock.Setup(a => a.TrainerCredentials.Find(user.EmailAddress)).Returns(mockTrainerCredentials);

            Trainer mockTrainer = new Trainer
            {
                EmailAddress = "test@user.com",
                FirstName = "Test",
                LastName = "User"
            };
            accountContextMock.Setup(a => a.Trainer.Find(mockTrainer.EmailAddress)).Returns(mockTrainer);

            configMock.Setup(c => c.Value.JwtKey).Returns(JWT_KEY);

            UserClaimsDTO userClaims = accountServices.AuthorizeTrainer(user);

            // THEN return a UserClaimsDTO containing an e-mail claim with the
            // user's e-mail, a name claim with the user's full name, a role claim
            // of trainer, and a TrainerId claim with the trainer's ID
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Email, "test@user.com"),
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Role, UserRole.TRAINER.ToString())
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
            TrainerCredentials mockTrainerCredentials = new TrainerCredentials
            {
                EmailAddress = "test@user.com",
                // NOTE: the hash should be "$2b$10$sCfS.t4SiS21G9rhNcqKue/PkEiitv/OfB0DojqdkMQneiUQw0l06"
                Hash = PASSWORD1234_HASH,
                Salt = PASSWORD1234_SALT
            };
            accountContextMock.Setup(a => a.TrainerCredentials.Find(user.EmailAddress)).Returns(mockTrainerCredentials);

            // THEN ensure an UnauthorizedAccessException is thrown
            UnauthorizedAccessException ex = Assert.Throws<UnauthorizedAccessException>(() => accountServices.AuthorizeTrainer(user));
            // AND ensure the message reads "Username or password is incorrect."
            Assert.Equal("Username or password is incorrect.", mockException.Message);
        }

        /// <summary>
        /// GIVEN a UserDTO with an e-mail address not found in the database
        /// WHEN a user's credentials are validated
        /// AND the user is not found in the database
        /// THEN an UnauthorizedAccessException is thrown
        /// AND the message reads "Username or password is incorrect."
        /// </summary>
        [Fact]
        public void ThrowAnUnauthorizedAccessExceptionWhenAUserIsNotFoundInDatabase()
        {
            // GIVEN a UserDTO with an e-mail address not found in the database
            UserDTO user = new UserDTO
            {
                EmailAddress = "test@user.com",
                Password = "Password1234"
            };

            // WHEN a user's credentials are validated
            // AND the user is not found in the database
            accountContextMock.Setup(a => a.TrainerCredentials.Find(user.EmailAddress)).Returns((TrainerCredentials)null);

            // THEN ensure an UnauthorizedAccessException is thrown
            UnauthorizedAccessException ex = Assert.Throws<UnauthorizedAccessException>(() => accountServices.AuthorizeTrainer(user));
            // AND ensure the message reads "Username or password is incorrect."
            UnauthorizedAccessException mockException = new UnauthorizedAccessException("Username or password is incorrect.");
            Assert.Equal("Username or password is incorrect.", mockException.Message);
        }
    }
}
