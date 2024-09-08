using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BancolombiaStarter.Backend.Domain.Entities;

namespace BancolombiaStarter.Backend.Infrastructure.DataAccess.Configuration
{
    internal class CommentsConfiguration :  IEntityTypeConfiguration<Comments>
    {
        public void Configure(EntityTypeBuilder<Comments> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            builder.ToTable(nameof(Comments));

            builder.HasKey(p => new { p.Id });

            builder.Property(b => b.Id).IsRequired();
            builder.Property(b => b.Observations).IsRequired();
            builder.Property(b => b.ProjectId).IsRequired();
            builder.Property(b => b.IdUser).IsRequired();


            builder.HasIndex(b => b.Id).IsUnique(); 
            builder.HasIndex(b => b.IdUser); 
            builder.HasIndex(b => b.ProjectId); 

            builder.HasOne(c => c.Project).WithMany(e => e.Comments).HasForeignKey(e => e.ProjectId);

        }
    }
}
