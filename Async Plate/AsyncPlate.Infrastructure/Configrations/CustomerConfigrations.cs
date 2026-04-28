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
    public class CustomerConfigrations : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {

            //app : customer = 1(may) : 1(must)

            
            builder.HasOne(c => c.AppUser)
                        .WithOne()
                        .HasForeignKey<Customer>(c => c.AppUserId)
                        .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
