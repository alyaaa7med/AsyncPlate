using AsyncPlate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Infrastructure.Configurations
{
    public class OneTimeTokenConfigurations : IEntityTypeConfiguration<OneTimeToken>
    {
        public void Configure(EntityTypeBuilder<OneTimeToken> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(t => t.Token)
            .IsRequired()
            .HasMaxLength(200);

            builder.Property(t => t.ExpiryDate)
                .IsRequired();

            builder.Property(t => t.IsActive)
                .IsRequired();

            builder.HasOne(o => o.AppUser)
                .WithMany(a => a.OneTimeTokens)
                .HasForeignKey(o => o.AppUserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
