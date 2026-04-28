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
    public class ProductConfigrations : IEntityTypeConfiguration<Product>
    {
        //category : product =  1(may ) : m(must)

        public void Configure(EntityTypeBuilder<Product> builder)
        {



            builder.HasOne(g => g.Category)
                .WithMany(g => g.Products)
                .HasForeignKey(g => g.CategoryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);//if the category is deleted but no products attached to it,
                                                   //then the category will be deleted,
                                                   //but if there are products attached to it,
                                                   //then the category will not be deleted 
                                                   //this operation is done in SQL server = DB not in the code
        }
    }
}
