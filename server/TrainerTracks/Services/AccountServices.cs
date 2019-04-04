using System;
using TrainerTracks.Data.Model.DTO.Account;
using TrainerTracks.Data.Model.Entity;
using TrainerTracks.Data.Model;
using Microsoft.Extensions.Options;
using TrainerTracks.Web.Data.Context;
using TrainerTracks.Web.Data.Model;

namespace TrainerTracks.Web.Services
{
    public class AccountServices : IAccountServices
    {
        private readonly IOptions<TrainerTracksConfig> config;
        private readonly IAccountContext accountContext;

        public AccountServices(IAccountContext accountContext,
            IOptions<TrainerTracksConfig> config)
        {
            this.accountContext = accountContext;
            this.config = config;
        }

        public UserClaimsDTO AuthorizeTrainer(UserDTO user)
        {
            Claims claims = GetTrainerClaims(user);
            return claims.GenerateUserClaimsDTO(config.Value.JwtKey);
        }

        private Claims GetTrainerClaims(UserDTO user)
        {
            TrainerCredentials trainerCredentials = accountContext.TrainerCredentials.Find(user.EmailAddress);

            if (trainerCredentials != null && trainerCredentials.IsTrainerAuthorized(user.Password))
            {
                return accountContext.Trainer.Find(user.EmailAddress).GenerateClaims();
            }
            throw new UnauthorizedAccessException("Username or password is incorrect.");
        }
    }
}
