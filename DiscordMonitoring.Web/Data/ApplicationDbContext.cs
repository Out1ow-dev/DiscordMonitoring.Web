using DiscordMonitoring.Web.Data.Config;
using DiscordMonitoring.Web.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DiscordMonitoring.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<GameEntity> Games { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<ServerEntity> Servers { get; set; }
        public DbSet<OnlineLog> onlineLogs { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ServerEntityConfiguration());
            base.OnModelCreating(modelBuilder); 
        }
    }
}
