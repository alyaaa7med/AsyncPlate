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
    public class RecipeConfigurations : IEntityTypeConfiguration<Recipe>
    {


        public void Configure(EntityTypeBuilder<Recipe> builder)
        {
            builder.HasKey(r => new { r.ProductId, r.InventoryId });

            builder.HasOne(r => r.Product)
                 .WithMany(p => p.Recipes)
                 .HasForeignKey(r => r.ProductId)
                 .IsRequired()
                 .OnDelete(DeleteBehavior.Cascade);//لو مسحت المنتج يمسح الوصفة لان ملهاش لازمة

            builder.HasOne(r => r.Inventory)
                  .WithMany(i => i.Recipes)
                  .HasForeignKey(r => r.InventoryId)
                  .IsRequired()
                  .OnDelete(DeleteBehavior.Restrict);// لو مسحت المكون شوف لو حد مستخدمه الاول
        }
    }
}

