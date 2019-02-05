using System;
using TrainerTracks.Controllers;
using TrainerTracks.Services;
using Xunit;
using Moq;
using Microsoft.Extensions.Options;
using TrainerTracks.Data.Model;
using TrainerTracks.Data.Model.DTO.Account;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;

namespace TrainerTracks.Test.Controller
{
    public class AccountControllerTest: ControllerBase
    {
        private Mock<IOptions<TrainerTracksConfig>> configMock = new Mock<IOptions<TrainerTracksConfig>>();
        private Mock<IAccountServices> accountServicesMock = new Mock<IAccountServices>();
        private AccountController accountController;

        public AccountControllerTest()
        {
            accountController = new AccountController(configMock.Object, accountServicesMock.Object);
        }

        /// <summary>
        /// GIVEN a UserDTO with an email and a password
        /// WHEN a user requests access to Trainer Tracks
        /// THEN a UserClaimsDTO object is returned
        /// </summary>
        [Fact]
        public void LoginReturnsUserClaimsDTO()
        {
            UserDTO user = new UserDTO()
            {
                emailAddress = "user@test.com",
                password = "password1234"
            };
            accountServicesMock.Setup(a => a.SetupUserClaims(user)).Returns(new UserClaimsDTO());
            UserClaimsDTO userClaimsDto = accountController.Login(user);

            Assert.NotNull(userClaimsDto);
        }

        /// <summary>
        /// GIVEN a UserDTO with an incorrect email and/or password
        /// WHEN a user requests access with incorrect credentials
        /// THEN an InvalidCredentialException should be thrown
        /// </summary>
        [Fact]
        public void InvalidCredentialsReturnsError()
        {
            UserDTO user = new UserDTO()
            {
                emailAddress = "user@test.com",
                password = "password1234"
            };
            InvalidCredentialException mockException = new InvalidCredentialException("Email or password is incorrect.");

            accountServicesMock.Setup(a => a.SetupUserClaims(user)).Throws(mockException);

            InvalidCredentialException ex = Assert.Throws<InvalidCredentialException>(() => accountController.Login(user));
            Assert.Equal("Email or password is incorrect.", ex.Message);
        }

    }
}
