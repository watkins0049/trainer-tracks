using Microsoft.EntityFrameworkCore;
using TrainerTracks.Data.Model.Entity;

namespace TrainerTracks.Data.Context
{
    public class TrainerTracksContext: DbContext
    {
        public TrainerTracksContext(DbContextOptions<TrainerTracksContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configure default schema
            modelBuilder.HasDefaultSchema("tt");
        }

        public DbSet<Trainer> Trainer { get; set; }
    }
}
