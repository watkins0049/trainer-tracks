using System;
using TrainerTracks.Data.Model.DTO.Account;
using TrainerTracks.Data.Model.Entity;

namespace TrainerTracks.Data.Dao
{
    public class UserDao
    {
        public UserDao()
        {
        }

        public virtual bool IsUserAuthenticated(UserDTO user)
        {
            return false;
        }

        public virtual Trainer RetrieveUserInformation(UserDTO user)
        {
            return new Trainer();
        }
    }
}
