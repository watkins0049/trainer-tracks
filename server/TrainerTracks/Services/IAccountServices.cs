using TrainerTracks.Data.Model.DTO.Account;

namespace TrainerTracks.Web.Services
{
    public interface IAccountServices
    {
        UserClaimsDTO SetupUserClaims(UserDTO user);
    }
}
