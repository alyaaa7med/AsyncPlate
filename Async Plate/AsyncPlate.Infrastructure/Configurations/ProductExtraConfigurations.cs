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
    public class ProductExtraConfigurations : IEntityTypeConfiguration<ProductExtra>
    {
        public void Configure(EntityTypeBuilder<ProductExtra> builder)
        {
            builder.HasKey(pe => new { pe.ProductId, pe.ExtraProductId });

            builder.HasOne(pe => pe.Product)
                   .WithMany(p => p.MainProducts)
                   .HasForeignKey(pe => pe.ProductId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict); 


            builder.HasOne(pe => pe.ExtraProduct)
                   .WithMany(p => p.ExtraProducts)
                   .HasForeignKey(pe => pe.ExtraProductId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}
