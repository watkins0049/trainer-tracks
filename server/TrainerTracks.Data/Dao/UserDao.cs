using System;
using Microsoft.EntityFrameworkCore;
using TrainerTracks.Data.Model.DTO.Account;
using TrainerTracks.Data.Model.Entity;

namespace TrainerTracks.Data.Dao
{
    public class UserDao : DbContext
    {
        public UserDao()
        {
        }

        public virtual bool IsUserAuthenticated(UserDTO user)
        {
            return true;
        }

        public virtual Trainer RetrieveUserInformation(UserDTO user)
        {
            return new Trainer();
        }
    }
}
