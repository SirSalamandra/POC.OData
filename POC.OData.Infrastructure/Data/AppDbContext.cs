using Microsoft.EntityFrameworkCore;
using POC.OData.Domain.Entities;

namespace POC.OData.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Client> Clients { get;set; }
        public DbSet<Product> Products { get;set; }
        public DbSet<Order> Orders { get;set; }
        public DbSet<OrderItem> OrderItems { get;set; }
    }
}
