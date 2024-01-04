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
        public DbSet<UserActivities> UserActivities { get; set; }
        public DbSet<Activities> Activities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("CW2");

            // Add any additional configurations here if needed

            // Configure primary keys
            modelBuilder.Entity<User>()
                .HasKey(u => u.Email);

            modelBuilder.Entity<Activities>()
                .HasKey(a => a.ActivityID);

            modelBuilder.Entity<UserActivities>()
                .ToTable("User-Activities")
                .HasKey(ua => ua.UserActivitiesID);

            // Configure relationships
            modelBuilder.Entity<UserActivities>()
                .HasOne(ua => ua.User)
                .WithMany(u => u.UserActivities)
                .HasForeignKey(ua => ua.Email);

            modelBuilder.Entity<UserActivities>()
                .HasOne(ua => ua.Activities)
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
