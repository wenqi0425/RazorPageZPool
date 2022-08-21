using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ZPool.Models;

namespace ZPool.Models
{
    public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<int>, int>
    {
        // space for your DbSet<EntityName> properties

        public DbSet<Car> Cars { get; set; }
        public DbSet<Ride> Rides { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Review> Reviews { get; set; }
        
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        //  only derived class can visit it
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seeding a 'Admin' role to AspNetRoles table
            builder.Entity<IdentityRole<int>>()
                .HasData(new IdentityRole<int>
                {
                    Id = 1,
                    Name = "Admin",
                    NormalizedName = "ADMIN".ToUpper()
                });

            // Password hasher
            var hasher = new PasswordHasher<AppUser>();

            //Seeding the User to AspNetUsers table
            builder.Entity<AppUser>()
                .HasData(new AppUser
                {
                    Id = 1,
                    FirstName = "Admin",
                    LastName = "Admin",
                    Email = "admin@zealand.com",
                    UserName = "Default Admin",
                    NormalizedUserName = "ADMIN",
                    NormalizedEmail = "ADMIN@ZEALAND.COM",
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    PasswordHash = hasher.HashPassword(null, "SuperSecret1!"),
                    LockoutEnabled = true,
                });

            // Seeding the relation between the admin user and admin role to AspNetUserRoles table
            builder.Entity<IdentityUserRole<int>>().HasData(
                new IdentityUserRole<int>
                {
                    RoleId = 1,
                    UserId = 1
                });
        }


    }
}
