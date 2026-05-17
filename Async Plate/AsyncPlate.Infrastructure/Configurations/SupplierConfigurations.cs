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
    public class SupplierConfigurations : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.HasKey(s => s.Id);

          


            builder.Property(s => s.ContactPhone).IsRequired().HasMaxLength(20);
            builder.Property(s => s.Name).IsRequired().HasMaxLength(100);
            builder.Property(s => s.ContactEmail).IsRequired().HasMaxLength(100);
            builder.Property(s => s.ContactPhone).IsRequired().HasMaxLength(20);
            builder.Property(s => s.Address).IsRequired().HasMaxLength(50);
            builder.Property(s => s.City).IsRequired().HasMaxLength(50);

        }
    }
}
