using System;
using TrainerTracks.Data.Context;
using TrainerTracks.Data.Model.Entity;

namespace TrainerTracks.Data.Repository
{
    public class TrainerCredentialsRepository : ITrainerCredentialsRepository
    {
        private readonly AccountContext accountContext;

        public TrainerCredentialsRepository(AccountContext accountContext)
        {
            this.accountContext = accountContext;
        }

        public TrainerCredentials GetById(long id)
        {
            return accountContext.TrainerCredentials.Find(id);
        }
    }
}
