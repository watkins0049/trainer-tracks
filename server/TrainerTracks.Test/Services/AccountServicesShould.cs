﻿using System.Collections.Generic;
using System.Security.Claims;
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
using TrainerTracks.Web.Data.Model.DTO.Account;
using TrainerTracks.Web.Exceptions;

namespace TrainerTracks.Test.Services
{
    public class AccountServicesShould
    {
        private const string PASSWORD1234_HASH = "$2b$10$sCfS.t4SiS21G9rhNcqKuemSpI8sU/F6z59x.aZimKouY2qLFp69.";
        private const string PASSWORD1234_SALT = "$2b$10$sCfS.t4SiS21G9rhNcqKue";
        private const string JWT_KEY = "fc5a6707-634b-4776-ba70-6f6cc45fbcfc";

        private readonly Mock<IAccountContext> accountContextMock = new Mock<IAccountContext>();
        private readonly Mock<IOptions<TrainerTracksConfig>> configMock = new Mock<IOptions<TrainerTracksConfig>>();
        private readonly IAccountServices accountServices;

        private TrainerCredentials trainerCredentialsCapture;
        private Trainer trainerCapture;

        public AccountServicesShould()
        {
            accountServices = new AccountServices(accountContextMock.Object,
                configMock.Object);

            accountContextMock.Setup(a => a.TrainerCredentials.Add(It.IsAny<TrainerCredentials>()))
                .Callback<TrainerCredentials>(r => trainerCredentialsCapture = r);
            accountContextMock.Setup(a => a.Trainer.Add(It.IsAny<Trainer>()))
                .Callback<Trainer>(t => trainerCapture = t);
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
            UserLoginDTO user = new UserLoginDTO
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
            UserLoginDTO user = new UserLoginDTO
            {
                EmailAddress = "test@user.com",
                Password = "Password1234"
            };

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
            Assert.Equal("Username or password is incorrect.", ex.Message);
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
            UserLoginDTO user = new UserLoginDTO
            {
                EmailAddress = "test@user.com",
                Password = "Password1234"
            };

            // WHEN a user's credentials are validated
            // AND the user is not found in the database
            accountContextMock.Setup(a => a.TrainerCredentials.Find(user.EmailAddress)).Returns((TrainerCredentials) null);

            // THEN ensure an UnauthorizedAccessException is thrown
            UnauthorizedAccessException ex = Assert.Throws<UnauthorizedAccessException>(() => accountServices.AuthorizeTrainer(user));
            // AND ensure the message reads "Username or password is incorrect."
            Assert.Equal("Username or password is incorrect.", ex.Message);
        }

        /// <summary>
        /// GIVEN a username and password
        /// WHEN a trainer signs up for an account
        /// THEN new Trainer with the inputted email address, first name, and last name is added
        /// AND a TrainerCredentials record is added
        /// AND SaveChanges is called
        /// AND the password can be verified by BCrypt
        /// </summary>
        [Fact]
        public void InsertTrainerAndTrainerCredentialsOnSignup()
        {
            // GIVEN a username and password
            UserSignupDTO user = new UserSignupDTO
            {
                EmailAddress = "test@user.com",
                ConfirmEmailAddress = "test@user.com",
                FirstName = "Test",
                LastName = "User",
                Password = "Password1234",
                ConfirmPassword = "Password1234"
            };

            // WHEN a trainer signs up for an account
            accountServices.SetupNewTrainer(user);

            // THEN new Trainer with the inputted email address, first name, and last name is added
            Assert.Equal(user.EmailAddress, trainerCapture.EmailAddress);
            Assert.Equal(user.FirstName, trainerCapture.FirstName);
            Assert.Equal(user.LastName, trainerCapture.LastName);
            accountContextMock.Verify(a => a.Trainer.Add(It.IsAny<Trainer>()), Times.Once);
            // AND a TrainerCredentials record is added
            accountContextMock.Verify(a => a.TrainerCredentials.Add(It.IsAny<TrainerCredentials>()), Times.Once);
            // AND SaveChanges is called
            accountContextMock.Verify(a => a.SaveChanges(), Times.Once);
            // AND the password can be verified by BCrypt
            string sha512Password1234 = "20B0747EEFCDC16FA4FB06BBF9284303645ECC3D2C43927878BD513F06853191C104AEBAE6D7FCA6291F1E296C6AF99EBF8A137CBD7A0D34F2E27B31CB4FECDB";
            Assert.True(BCrypt.Net.BCrypt.Verify(sha512Password1234, trainerCredentialsCapture.Hash));
        }

        /// <summary>
        /// GIVEN a UserSignupDTO without a valid email address
        /// WHEN a user is attempting to setup an account
        /// THEN throw an FormatException
        /// AND ensure the exception reads "The specified string is not in the form required for an e-mail address."
        /// </summary>
        [Fact]
        public void ThrowFormatExceptionWhenEmailAddressNotValid()
        {
            // GIVEN a UserSignupDTO without a valid email address
            UserSignupDTO user = new UserSignupDTO
            {
                EmailAddress = "not an email",
                ConfirmEmailAddress = "not an email",
                FirstName = "Test",
                LastName = "User",
                Password = "Password1234",
                ConfirmPassword = "Password1234"
            };

            // WHEN a user is attempting setup an account
            // THEN throw an ArgumentException
            FormatException ex = Assert.Throws<FormatException>(() => accountServices.SetupNewTrainer(user));
            // AND ensure the exception reads "Invalid email address."
            Assert.Equal("The specified string is not in the form required for an e-mail address.", ex.Message);
        }

        /// <summary>
        /// GIVEN a UserSignupDTO without a valid password
        /// WHEN a user is attempting to setup an account
        /// THEN throw an ArgumentException
        /// AND ensure the exception reads "Password must be at least 8 characters, have 1 uppercase character, 1 lowercase character, and 1 number."
        /// </summary>
        [Theory]
        [InlineData(null)]
        [InlineData("pass")]
        [InlineData("password")]
        [InlineData("PASSWORD")]
        [InlineData("Password")]
        [InlineData("012345678")]
        public void ThrowArgumentExceptionWhenPasswordNotValid(string password)
        {
            // GIVEN a UserSignupDTO without a valid password
            UserSignupDTO user = new UserSignupDTO
            {
                EmailAddress = "test@user.com",
                ConfirmEmailAddress = "test@user.com",
                FirstName = "Test",
                LastName = "User",
                Password = password,
                ConfirmPassword = password
            };

            // WHEN a user is attempting to setup an account
            // THEN throw an ArgumentException
            ArgumentException ex = Assert.Throws<ArgumentException>(() => accountServices.SetupNewTrainer(user));
            // AND ensure the exception reads "Password must be at least 8 characters, have 1 uppercase character, 1 lowercase character, and 1 number."
            Assert.Equal("Password must be at least 8 characters, have 1 uppercase character, 1 lowercase character, and 1 number.",
                ex.Message);
        }

        /// <summary>
        /// GIVEN a UserSignupDTO without a first name or last name
        /// WHEN a user is attempting to setup an account
        /// THEN throw an ArgumentException
        /// AND ensure the exception reads "First name and last name are required."
        /// </summary>
        [Theory]
        [InlineData("first", "")]
        [InlineData("", "last")]
        [InlineData(" ", "last")]
        [InlineData("first", " ")]
        [InlineData(" ", " ")]
        [InlineData(null, null)]
        public void ThrowArgumentExceptionWhenNameIsBlankOrNull(string firstName, string lastName)
        {
            // GIVEN a UserSignupDTO without a first name or last name
            UserSignupDTO user = new UserSignupDTO
            {
                EmailAddress = "test@user.com",
                ConfirmEmailAddress = "test@user.com",
                FirstName = firstName,
                LastName = lastName,
                Password = "Password1234",
                ConfirmPassword = "Password1234"
            };

            // WHEN a user is attempting to setup an account
            // THEN throw an ArgumentException
            ArgumentException ex = Assert.Throws<ArgumentException>(() => accountServices.SetupNewTrainer(user));
            // AND ensure the exception reads "First name and last name are required."
            Assert.Equal("First name and last name are required.",
                ex.Message);
        }

        /// <summary>
        /// GIVEN a UserSignupDTO with valid input
        /// WHEN a user attempts to setup an account
        /// AND an account with the given email address already exists
        /// THEN throw a UserSignupException
        /// AND ensure the message reads "An account with the given email already exists."
        /// </summary>
        [Fact]
        public void ThrowUserSignupExceptionOnExistingUser()
        {
            // GIVEN a UserSignupDTO with valid input
            UserSignupDTO user = new UserSignupDTO
            {
                EmailAddress = "test@user.com",
                ConfirmEmailAddress = "test@user.com",
                FirstName = "First",
                LastName = "Last",
                Password = "Password1234",
                ConfirmPassword = "Password1234"
            };

            // WHEN a user attempts to setup an account
            // AND an account with the given e-mail address already exists
            accountContextMock.Setup(a => a.Trainer.Find(It.IsAny<string>())).Returns(new Trainer());

            // THEN throw a UserSignupException
            UserSignupException ex = Assert.Throws<UserSignupException>(() => accountServices.SetupNewTrainer(user));
            // AND ensure the message reads "An account with the given email already exists."
            Assert.Equal("An account with the given email already exists.", ex.Message);
        }

        /// <summary>
        /// GIVEN a UserSignupDTO with non-matching email addresses
        /// WHEN a user attempts to setup an account with non-matching email addresses
        /// THEN throw a UserSignupException
        /// AND ensure the message reads "Email addresses do not match."
        /// </summary>
        [Fact]
        public void ThrowUserSignupExceptionOnNonMatchingEmails()
        {
            // GIVEN a UserSignupDTO with non-matching email addresses
            UserSignupDTO user = new UserSignupDTO
            {
                EmailAddress = "test@user.com",
                ConfirmEmailAddress = "test1@user.com",
                FirstName = "Test",
                LastName = "User",
                Password = "Password1234",
                ConfirmPassword = "Password1234"
            };

            // WHEN a user attempts to setup an account with non-matching email addresses
            // THEN throw a UserSignupException
            UserSignupException ex = Assert.Throws<UserSignupException>(() => accountServices.SetupNewTrainer(user));
            // AND ensure the message reads "Email addresses do not match."
            Assert.Equal("Email addresses do not match.", ex.Message);
        }

        /// <summary>
        /// GIVEN a UserSignupDTO with non-matching passwords
        /// WHEN a user attempts to setup an account with non-matching passwords
        /// THEN throw a UserSignupException
        /// AND ensure the message reads "Passwords do not match."
        /// </summary>
        [Fact]
        public void ThrowUserSignupExceptionOnNonMatchingPasswords()
        {
            // GIVEN a UserSignupDTO with non-matching passwords
            UserSignupDTO user = new UserSignupDTO
            {
                EmailAddress = "test@user.com",
                ConfirmEmailAddress = "test@user.com",
                FirstName = "Test",
                LastName = "User",
                Password = "Password1234",
                ConfirmPassword = "Password12345"
            };

            // WHEN a user attempts to setup an account with non-matching passwords
            // THEN throw a UserSignupException
            UserSignupException ex = Assert.Throws<UserSignupException>(() => accountServices.SetupNewTrainer(user));
            // AND ensure the message reads "Passwords do not match."
            Assert.Equal("Passwords do not match.", ex.Message);
        }
    }
}