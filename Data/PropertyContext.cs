using Microsoft.EntityFrameworkCore;
using PropertyApp.Models;

namespace PropertyApp.Data
{
    public class PropertyContext : DbContext
    {
        public PropertyContext(DbContextOptions<PropertyContext> options) : base(options) { }

        public DbSet<Property> Properties { get; set; }

         protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Property>()
                .Property(p => p.Price)
                .HasPrecision(18, 2); // Set precision and scale for Price

            base.OnModelCreating(modelBuilder);
        }
    }
}
