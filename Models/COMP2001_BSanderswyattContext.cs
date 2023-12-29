using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Coursework2001.Models
{
    public class COMP2001_BSanderswyattContext : DbContext
    {
        public COMP2001_BSanderswyattContext()
        {
        }

        public COMP2001_BSanderswyattContext(DbContextOptions<COMP2001_BSanderswyattContext> options)
            : base(options)
        {
        }

        // DbSet properties for the tables
        public DbSet<User> Users { get; set; }
        public DbSet<Activities> Activities { get; set; }
        public DbSet<UserActivities> UserActivities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Add any additional configurations here if needed
            modelBuilder.Entity<UserActivities>()
                .HasKey(ua => new { ua.UserActivitiesID });

            modelBuilder.Entity<UserActivities>()
                .HasOne(ua => ua.User)
                .WithMany(u => u.UserActivities)
                .HasForeignKey(ua => ua.UserID);

            modelBuilder.Entity<UserActivities>()
                .HasOne(ua => ua.Activity)
                .WithMany(a => a.UserActivities)
                .HasForeignKey(ua => ua.ActivityID);
        }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //Build the configuration
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                //Get the connection string from appsettings
                var connectionString = configuration.GetConnectionString("DB_2001");

                //Use the connection string for DBContext
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}
