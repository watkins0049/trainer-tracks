using System;
using TrainerTracks.Web.Controllers;
using TrainerTracks.Web.Services;
using Xunit;
using Moq;
using Microsoft.Extensions.Options;
using TrainerTracks.Data.Model;
using TrainerTracks.Data.Model.DTO.Account;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;

namespace TrainerTracks.Test.Controller
{
    public class AccountControllerShould: ControllerBase
    {
        private Mock<IOptions<TrainerTracksConfig>> configMock = new Mock<IOptions<TrainerTracksConfig>>();
        private Mock<IAccountServices> accountServicesMock = new Mock<IAccountServices>();
        private AccountController accountController;

        public AccountControllerShould()
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
                EmailAddress = "user@test.com",
                Password = "password1234"
            };
            accountServicesMock.Setup(a => a.SetupUserClaims(user)).Returns(new UserClaimsDTO());
            UserClaimsDTO userClaimsDto = accountController.Login(user);

            Assert.NotNull(userClaimsDto);
        }

        /// <summary>
        /// GIVEN a UserDTO with an incorrect email and/or password
        /// WHEN a user requests access with incorrect credentials
        /// THEN an UnauthorizedAccessException is thrown
        /// AND the message reads "Username or password is incorrect."
        /// </summary>
        [Fact]
        public void InvalidCredentialsReturnsError()
        {
            UserDTO user = new UserDTO()
            {
                EmailAddress = "user@test.com",
                Password = "password1234"
            };
            UnauthorizedAccessException mockException = new UnauthorizedAccessException("Username or password is incorrect.");

            accountServicesMock.Setup(a => a.SetupUserClaims(user)).Throws(mockException);

            // THEN ensure an UnauthorizedAccessException is thrown
            UnauthorizedAccessException ex = Assert.Throws<UnauthorizedAccessException>(() => accountController.Login(user));
            // AND ensure the message reads "Username or password is incorrect."
            Assert.Equal("Username or password is incorrect.", mockException.Message);
        }

    }
}
