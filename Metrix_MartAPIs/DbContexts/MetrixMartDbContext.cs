using Metrix_MartAPIs.Model;
using Microsoft.EntityFrameworkCore;

namespace Metrix_MartAPIs.DbContexts
{
    public class MetrixMartDbContext : DbContext
    {
        public MetrixMartDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
    }
}
