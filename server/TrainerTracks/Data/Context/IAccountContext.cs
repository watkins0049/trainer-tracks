using System;
using Microsoft.EntityFrameworkCore;
using TrainerTracks.Data.Model.Entity;

namespace TrainerTracks.Web.Data.Context
{
    public interface IAccountContext
    {
        DbSet<Trainer> Trainer { get; set; }
        DbSet<TrainerCredentials> TrainerCredentials { get; set; }
    }
}
