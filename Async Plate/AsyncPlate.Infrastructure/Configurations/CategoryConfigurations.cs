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
    public class CategoryConfigurations : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {

            builder.HasKey(c => c.Id);

            //form gpt
            builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

            builder.Property(c => c.Description)
                .HasMaxLength(500);

            builder.Property(c => c.ImageUrl)
                .HasMaxLength(300);


            builder.HasOne(c => c.CurrentOffer)
                    .WithMany(o => o.Categories)
                    .HasForeignKey(c => c.OfferId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.SetNull);


        }
    }
}
