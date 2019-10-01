using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Entities {
    public class ApplicationUser : IdentityUser {
    }

    public class ApplicationRole : IdentityRole {
        public ApplicationRole()
        {
        }

        public ApplicationRole(string name, string description) : base(name)
        {
            Description = description;
        }

        public string Description { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Map roles to tables
            builder.Entity<IdentityRole>().ToTable("AspNetRoles");
            var role = builder.Entity<ApplicationRole>().ToTable("AspNetRoles");
            role.Property(r => r.Name).IsRequired();
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
