using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipTracker.Data
{
    public class InternshipTrackerDbContext : DbContext
    {
        public InternshipTrackerDbContext(DbContextOptions<InternshipTrackerDbContext> options) : base(options)
        {
        }
        public DbSet<Company> Companies { get; set; }
        public DbSet<InternshipApplication> InternshipApplications { get; set; }
        public DbSet<Status> Statuses { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Seed initial data for Status table
            modelBuilder.Entity<Status>().HasData(
                new Status { Id = 1, Name = "Applied" },
                new Status { Id = 2, Name = "Phone Screen" },
                new Status { Id = 3, Name = "Technical Interview" },
                new Status { Id = 4, Name = "Onsite Interview" },
                new Status { Id = 5, Name = "Offer" },
                new Status { Id = 6, Name = "Rejected" }
            );

            modelBuilder.Entity<Company>().HasData(
                new Company { Id = 1, Name = "Add Company" }
            );
        }
    }
}
