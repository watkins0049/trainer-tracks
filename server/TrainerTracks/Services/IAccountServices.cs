using TrainerTracks.Web.Data.Model.DTO.Account;

namespace TrainerTracks.Web.Services
{
    public interface IAccountServices
    {
        UserClaimsDTO AuthorizeTrainer(UserLoginDTO user);
        void SetupNewTrainer(UserSignupDTO user);
    }
}
