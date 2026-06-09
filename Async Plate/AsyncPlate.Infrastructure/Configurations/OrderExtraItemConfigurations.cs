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
    public class OrderExtraItemConfigurations : IEntityTypeConfiguration<OrderExtraItem>
    {
        public void Configure(EntityTypeBuilder<OrderExtraItem> builder)
        {

            builder.HasKey(oi => oi.Id);

            builder.Property(x => x.Quantity).IsRequired();

            builder.Property(oi => oi.UnitPriceAtSale).HasColumnType("decimal(18,2)");

            builder.HasOne(oei => oei.OrderItem)
                .WithMany(oi => oi.Extras)
                .HasForeignKey(oei => oei.OrderItemId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);//was cascade; //to clean up database from old orders


            builder.HasOne(oei => oei.Product)
                .WithMany(p => p.OrderItemExtras)
                .HasForeignKey(oei => oei.ProductId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);//was cascade; //to clean up database from old products
        
        
        
        }
    }
}
