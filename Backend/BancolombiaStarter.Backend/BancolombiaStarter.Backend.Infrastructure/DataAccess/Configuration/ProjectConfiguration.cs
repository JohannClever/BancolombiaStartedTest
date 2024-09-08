using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BancolombiaStarter.Backend.Domain.Entities;

namespace BancolombiaStarter.Backend.Infrastructure.DataAccess.Configuration
{
    internal class ProjectConfiguration :IEntityTypeConfiguration<Projects>
    {
        public void Configure(EntityTypeBuilder<Projects> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.ToTable(nameof(Projects));

            builder.HasKey(p => new { p.Id });

            builder.Property(b => b.Id).IsRequired();
            builder.Property(b => b.Name).IsRequired();

        }
    }
}
