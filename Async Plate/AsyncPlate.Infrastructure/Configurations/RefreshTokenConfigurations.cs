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
    public class RefreshTokenConfigurations : IEntityTypeConfiguration<RefreshToken>
    {

        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(rt => rt.Id);

            //builder.Property(rt => rt.RefreshTokenValue)
            //    .IsRequired()
            //    .HasMaxLength(500);
            builder.Property(r => r.RefreshTokenValue)
                   .HasColumnType("nvarchar(max)"); //to solve the error of max length of 4000 characters

            builder.Property(rt=> rt.IsExpired)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(rt => rt.IsRevoked)
                .IsRequired()
                .HasDefaultValue(false);

            builder.HasOne(rt => rt.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}