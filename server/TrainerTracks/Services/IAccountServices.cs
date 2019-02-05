using System;
using TrainerTracks.Data.Model.DTO.Account;

namespace TrainerTracks.Services
{
    public interface IAccountServices
    {

        UserClaimsDTO SetupUserClaims(UserDTO user);

    }
}
