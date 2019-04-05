using System;
using TrainerTracks.Data.Model;
using Microsoft.Extensions.Options;
using TrainerTracks.Web.Data.Context;
using TrainerTracks.Web.Data.Model.Entity;
using TrainerTracks.Data.Model.Entity.DBEntities;
using TrainerTracks.Web.Data.Model.DTO.Account;
using System.Net.Mail;

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

        #region Authorization

        public UserClaimsDTO AuthorizeTrainer(UserLoginDTO user)
        {
            Claims claims = GetTrainerClaims(user);
            return claims.GenerateUserClaimsDTO(config.Value.JwtKey);
        }

        private Claims GetTrainerClaims(UserLoginDTO user)
        {
            TrainerCredentials trainerCredentials = accountContext.TrainerCredentials.Find(user.EmailAddress);

            if (trainerCredentials != null && trainerCredentials.IsTrainerAuthorized(user.Password))
            {
                return accountContext.Trainer.Find(user.EmailAddress).GenerateClaims();
            }
            throw new UnauthorizedAccessException("Username or password is incorrect.");
        }

        #endregion Authorization

        #region New Trainer Setup

        public void SetupNewTrainer(UserSignupDTO user)
        {
            // No need for a regex; this automagically validates the user's
            // email address and throws a FormatException if it's not valid
            MailAddress m = new MailAddress(user.EmailAddress);
            Trainer trainer = new Trainer
            {
                EmailAddress = user.EmailAddress,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            TrainerCredentials trainerCredentials = TrainerCredentials.BuildNewUser(user);

            accountContext.Trainer.Add(trainer);
            accountContext.TrainerCredentials.Add(trainerCredentials);
            accountContext.SaveChanges();
        }

        #endregion New Trainer Setup
    }
}
