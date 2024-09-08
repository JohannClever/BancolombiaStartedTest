using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BancolombiaStarter.Backend.Domain.Entities;

namespace BancolombiaStarter.Backend.Infrastructure.DataAccess.Configuration
{
    internal class FinanceConfiguration :  IEntityTypeConfiguration<Finance>
    {
        public void Configure(EntityTypeBuilder<Finance> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            builder.ToTable(nameof(Finance));

            builder.HasKey(p => new { p.Id });

            builder.Property(b => b.Id).IsRequired();
            builder.Property(b => b.UserId).IsRequired();
            builder.Property(b => b.ProjectId).IsRequired();
            builder.HasOne(c => c.Project).WithMany(e => e.Finances).HasForeignKey(e => e.ProjectId);

        }
    }
}
