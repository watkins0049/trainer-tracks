﻿using System;
using TrainerTracks.Data.Model;
using Microsoft.Extensions.Options;
using TrainerTracks.Web.Data.Context;
using TrainerTracks.Web.Data.Model.Entity;
using TrainerTracks.Data.Model.Entity.DBEntities;
using TrainerTracks.Web.Data.Model.DTO.Account;
using TrainerTracks.Web.Exceptions;

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
            Trainer trainer = accountContext.Trainer.Find(user.EmailAddress); 
            if (trainer != null)
            {
                throw new UserSignupException("An account with the given email already exists.");
            }

            trainer = Trainer.BuildTrainerFromUserSignup(user);
            accountContext.Trainer.Add(trainer);

            TrainerCredentials trainerCredentials = TrainerCredentials.BuildTrainerCredentialsFromUserSignup(user);
            accountContext.TrainerCredentials.Add(trainerCredentials);

            accountContext.SaveChanges();
        }

        #endregion New Trainer Setup
    }
}
