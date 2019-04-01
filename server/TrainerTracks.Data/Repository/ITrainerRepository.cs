using System;
using System.Collections.Generic;
using TrainerTracks.Data.Model.Entity;

namespace TrainerTracks.Data.Repository
{
    public interface ITrainerRepository : IRepository<Trainer, string>
    {
        Trainer GetTrainerByEmail(string emailAddresss);
    }
}
