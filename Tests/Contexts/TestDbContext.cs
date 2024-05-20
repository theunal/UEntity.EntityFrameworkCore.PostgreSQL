using Microsoft.EntityFrameworkCore;
using Tests.Entities;

namespace Tests.Contexts
{
    public class TestDbContext : DbContext
    {
        public DbSet<TestEntity> Entities { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
                => optionsBuilder.UseNpgsql("Host=localhost;Database=TestDb;Username=postgres;Password=12345");
    }
}