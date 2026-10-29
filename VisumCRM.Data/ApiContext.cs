using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace VisumCRM.Data
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options) : base(options)
        {

        }
        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries().Where(x => x.Entity.GetType().GetProperty("Created") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("Created").CurrentValue = VisumCRM.Core.ConvertDateTimeToLong(DateTime.Now);
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property("Created").IsModified = false;
                    entry.Property("Updated").CurrentValue = VisumCRM.Core.ConvertDateTimeToLong(DateTime.Now);
                }
            }
            return base.SaveChanges();
        }

        public DbSet<Entity.Country> Countries { get; set; }
        public DbSet<Entity.Postalzone> Postalzones { get; set; }
        public DbSet<Entity.Location> Locations { get; set; }
        public DbSet<Entity.User> Users { get; set; }
        public DbSet<Entity.License> Licenses { get; set; }
        public DbSet<Entity.Company> Companies { get; set; }
    }
}
