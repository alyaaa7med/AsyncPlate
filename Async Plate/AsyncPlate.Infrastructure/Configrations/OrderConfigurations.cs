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
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(o => o.TotalAmountPrice).HasColumnType("decimal(18,2)");
            builder.Property(o => o.TotalFee).HasColumnType("decimal(18,2)");
            builder.Property(o => o.TotalFeeTotal).HasColumnType("decimal(18,2)");
            builder.HasOne(o => o.Customer)
                   .WithMany(c => c.Orders)
                   .HasForeignKey(o => o.CustomerId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.KitchenChef)
                     .WithMany(k => k.Orders)
                     .HasForeignKey(o => o.KitchenChefId)
                     .IsRequired(false)
                     .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
