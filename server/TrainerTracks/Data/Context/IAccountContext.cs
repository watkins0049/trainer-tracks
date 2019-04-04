using Microsoft.EntityFrameworkCore;
using TrainerTracks.Data.Model.Entity.DBEntities;

namespace TrainerTracks.Web.Data.Context
{
    public interface IAccountContext
    {
        DbSet<Trainer> Trainer { get; set; }
        DbSet<TrainerCredentials> TrainerCredentials { get; set; }
    }
}
