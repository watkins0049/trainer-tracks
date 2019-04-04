using TrainerTracks.Data.Model.DTO.Account;

namespace TrainerTracks.Web.Services
{
    public interface IAccountServices
    {
        UserClaimsDTO AuthorizeTrainer(UserDTO user);
    }
}
