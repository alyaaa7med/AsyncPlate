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
    public class OfferConfigurations : IEntityTypeConfiguration<Offer>
    {
        public void Configure(EntityTypeBuilder<Offer> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Title).IsRequired().HasMaxLength(200);

            builder.Property(o => o.DiscountPercentage).IsRequired().HasColumnType("decimal(5,2)");

            builder.Property(o => o.StartDate).IsRequired();

            builder.Property(o => o.EndDate).IsRequired(false);

            builder.Property(o => o.IsActive).IsRequired();

        }
    }
}