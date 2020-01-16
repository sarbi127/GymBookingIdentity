using System;
using System.Collections.Generic;
using System.Text;
using GymBookingNC19.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GymBookingNC19.Data
{
    //Här sätter vi ApplicationUser, IdentityRole samt att vi använder oss av string som id
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<GymClass>  GymClasses { get; set; }
        public DbSet<ApplicationUserGymClass> ApplicationUserGymClasses { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Körs alltid först annars skrivs vår egen konfiguration över av default inställningarna i base 
            base.OnModelCreating(builder);

            //Definerar upp en kompositnyckel i kopplingstabellen
            builder.Entity<ApplicationUserGymClass>()
                .HasKey(k => new 
                { 
                    k.ApplicationUserId,
                    k.GymClassId 
                });

            builder.Entity<GymClass>().HasQueryFilter(g => g.StartDate > DateTime.Now);
        }
    }
}
