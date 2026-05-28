using AsyncPlate.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Infrastructure.Configurations
{
    public class AdminConfigurations : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {
            builder.HasKey(c => c.Id);


            builder.HasOne(c => c.AppUser)
                        .WithOne(a => a.Admin)
                        .HasForeignKey<Admin>(c => c.AppUserId)
                        .IsRequired()
                        .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
