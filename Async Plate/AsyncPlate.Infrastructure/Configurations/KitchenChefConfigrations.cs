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
    public class KitchenChefConfigrations : IEntityTypeConfiguration<KitchenChef>
    {
        public void Configure(EntityTypeBuilder<KitchenChef> builder)
        {
            builder.HasKey(k => k.Id);

            builder.HasOne(k => k.AppUser)
                .WithOne(a => a.KitchenChef)
                .HasForeignKey<KitchenChef>(k => k.AppUserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
