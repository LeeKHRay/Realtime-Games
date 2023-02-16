using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RealtimeGames.Shared.Models;
using RealtimeGames.Server.Areas.Identity.Models;

namespace RealtimeGames.Server.Data
{
    public class RealtimeGamesDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public RealtimeGamesDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        public DbSet<Score> Scores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // specify that the Score property is an Owned Entity of the ApplicationUser entity type
            modelBuilder.Entity<ApplicationUser>().OwnsOne(p => p.Score);
        }
    }
}