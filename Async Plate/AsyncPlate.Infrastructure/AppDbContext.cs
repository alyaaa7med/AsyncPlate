using AsyncPlate.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;


namespace AsyncPlate.Infrastructure
{


    public class AppDbContext : IdentityDbContext<AppUser>
    {
        //dbsets for the application execlude the auth
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            
            base.OnModelCreating(builder);
            // Configuration السطر السحري ده بيقرأ كل ملفات الـ  
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        }
       

    }
}