using Microsoft.EntityFrameworkCore;
using TrainerTracks.Data.Model.Entity;

namespace TrainerTracks.Web.Data.Context
{
    public class AccountContext : DbContext, IAccountContext
    {
        public AccountContext(DbContextOptions<AccountContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("tt");
        }
        
        public DbSet<Trainer> Trainer { get; set; }
        public DbSet<TrainerCredentials> TrainerCredentials { get; set; }

        // TODO: remove these...
        public DbSet<Client> Client { get; set; }
        public DbSet<TrainerClients> TrainerClients { get; set; }
    }
}
