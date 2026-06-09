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
    public class CustomerConfigrations : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {

            //app : customer = 1(may) : 1(must)

            builder.HasKey(c => c.Id);


            builder.HasOne(c => c.AppUser)
                        .WithOne(a => a.Customer)
                        .HasForeignKey<Customer>(c => c.AppUserId)
                        .IsRequired()
                        .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
