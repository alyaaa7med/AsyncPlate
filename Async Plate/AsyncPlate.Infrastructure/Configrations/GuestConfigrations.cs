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
    public class GuestConfigrations : IEntityTypeConfiguration<Guest>
    {
        public void Configure(EntityTypeBuilder<Guest> builder)
        {



            builder.Property(g => g.FirstName)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(g => g.LastName)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(g => g.Email)
                   .IsRequired()
                   .HasMaxLength(150);


            builder.HasIndex(g => g.Email)
                   .IsUnique()
                   .HasDatabaseName("Index_Unique_Guest_Email");


        }
    }
}
