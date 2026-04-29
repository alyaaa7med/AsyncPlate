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
    public class InventoryConfigurations : IEntityTypeConfiguration<Inventory>
    {
        public void Configure(EntityTypeBuilder<Inventory> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.PurchasedUnitPrice).HasColumnType("decimal(18,2)");
            builder.Property(i => i.CurrentStock).HasColumnType("decimal(18,4)"); 
            builder.Property(i=> i.MinStockLevel).HasColumnType("decimal(18,4)");

            builder.HasOne(i => i.Supplier)
                .WithMany(s => s.Inventories)
                .HasForeignKey(i => i.SupplierId)
                .IsRequired();


        }
    }
}
