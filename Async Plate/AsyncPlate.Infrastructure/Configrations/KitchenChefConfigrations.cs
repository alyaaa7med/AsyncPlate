using AsyncPlate.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Infrastructure.Configrations
{
    public class KitchenChefConfigrations : IEntityTypeConfiguration<KitchenChef>
    {
        public void Configure(EntityTypeBuilder<KitchenChef> builder)
        {
            //app  : chef =  1(may) : 1 (must)
            builder.HasOne(k => k.AppUser)
                .WithOne(a => a.KitchenChef)
                .HasForeignKey<KitchenChef>(k => k.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
