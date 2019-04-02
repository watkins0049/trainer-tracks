using System;
using System.Linq;
using TrainerTracks.Data.Context;
using TrainerTracks.Data.Model.Entity;

namespace TrainerTracks.Data.Repository
{
    public class TrainerRepository : ITrainerRepository
    {
        private readonly AccountContext accountContext;

        public TrainerRepository(AccountContext accountContext)
        {
            this.accountContext = accountContext;
        }

        public Trainer GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Trainer GetTrainerByEmail(string emailAddresss)
        {
            return accountContext.Trainer.Where(t => emailAddresss.Equals(t.EmailAddress))?.FirstOrDefault();
        }
    }
}
