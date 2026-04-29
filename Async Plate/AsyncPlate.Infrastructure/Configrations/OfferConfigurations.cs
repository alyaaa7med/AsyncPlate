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
    public class OfferConfigurations : IEntityTypeConfiguration<Offer>
    {
        public void Configure(EntityTypeBuilder<Offer> builder)
        {
            builder.HasKey(o => o.Id);

            //builder.Property(o => o.Title)
            //       .IsRequired()
            //       .HasMaxLength(100);

            //builder.Property(o => o.Description)
            //       .HasMaxLength(500);

            builder.Property(o => o.DiscountPercentage)
                   .HasColumnType("decimal(18,2)");

            //builder.Property(o => o.StartDate)
            //       .IsRequired();

            //builder.Property(o => o.EndDate)
            //       .IsRequired();
        }
    }
}