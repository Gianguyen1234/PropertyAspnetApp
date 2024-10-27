using Microsoft.EntityFrameworkCore;
using PropertyApp.Models;

namespace PropertyApp.Data
{
    public class PropertyContext : DbContext
    {
        public PropertyContext(DbContextOptions<PropertyContext> options) : base(options) { }

        public DbSet<Property> Properties { get; set; }
    }
}
