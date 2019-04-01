using System;
using TrainerTracks.Data.Dao;
using TrainerTracks.Data.Model.DTO.Account;
using TrainerTracks.Data.Model.Entity;
using Xunit;

namespace TrainerTracks.Data.Test.Dao
{
    public class UserDaoShould
    {
        private readonly UserDao userDao;

        public UserDaoShould()
        {
            userDao = new UserDao();
        }

        /// <summary>
        /// GIVEN a UserDTO object containing a user's e-mail address and password
        /// WHEN a user is authenticated properly
        /// THEN a Trainer object is returned containing the user's e-mail address,
        /// first name, last name, and trainer ID
        /// </summary>
        //[Fact]
        //public void ReturnTrainerOnAuthenticatedUser()
        //{
        //    // GIVEN a UserDTO object containing a user's e-mail address and password
        //    UserDTO user = new UserDTO
        //    {
        //        EmailAddress = "test@user.com",
        //        Password = "password1234"
        //    };

        //    // WHEN a user is authenticated properly
        //    Trainer trainer = userDao.RetrieveUserInformation(user);

        //    // THEN a Trainer object is returned containing the user's e-mail address,
        //    // first name, last name, and trainer ID
        //    Assert.Equal(user.EmailAddress, trainer.EmailAddress);
        //    Assert.Equal("Test", trainer.FirstName);
        //    Assert.Equal("User", trainer.LastName);
        //    Assert.Equal(1, trainer.TrainerId);
        //}
    }
}
