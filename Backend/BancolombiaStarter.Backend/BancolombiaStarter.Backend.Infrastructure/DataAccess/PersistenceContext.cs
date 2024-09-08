using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BancolombiaStarter.Backend.Infrastructure.Authorization.Entities;

namespace BancolombiaStarter.Backend.Infrastructure.DataAccess
{
    /// <summary>
    /// Persistence Context
    /// </summary>
    public class PersistenceContext : IdentityDbContext<ApplicationUser>
    {
        /// <summary>        
        /// </summary>
        /// <param name="options"></param>
        public PersistenceContext(DbContextOptions options) : base(options) { }

        /// <summary>
        /// Save changes to be confirmed
        /// </summary>
        /// <returns></returns>
        public async Task CommitAsync()
        {
            await SaveChangesAsync().ConfigureAwait(false);
        }

        /// <summary>        
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(PersistenceContext).Assembly);
        }
    }
}
