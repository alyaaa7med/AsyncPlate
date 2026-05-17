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
    public class NotificationConfigurations : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(n => n.Id);

            builder.Property(n => n.Message).IsRequired().HasMaxLength(500);

            builder.Property(n => n.CreatedAt).IsRequired();

            builder.Property(n => n.IsRead).IsRequired();

            builder.HasOne(n => n.Customer)
                   .WithMany(c => c.Notifications)
                   .HasForeignKey(n => n.CustomerId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);//was cascade
        }
    }
}
