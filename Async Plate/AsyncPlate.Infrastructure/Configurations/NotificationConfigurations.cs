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
    public class NotificationConfigurations : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(n => n.Id);

            builder.Property(n => n.Message).IsRequired().HasMaxLength(500);

            builder.Property(n => n.CreatedAt).IsRequired();

            builder.Property(n => n.IsRead).IsRequired();

            builder.HasOne(n => n.User)
                   .WithMany(c => c.Notifications)
                   .HasForeignKey(n => n.userId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);//was cascade
        }
    }
}
