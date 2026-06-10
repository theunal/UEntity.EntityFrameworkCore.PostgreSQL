using Microsoft.EntityFrameworkCore;
using UEntity.EntityFrameworkCore.PostgreSQL;

namespace Tests.Tests.Verification;

public interface IBaseEntity
{
    int Id { get; set; }
}

public class TestEntity : IBaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Value { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class TestDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class TestUser : IBaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class VerifDbContext : DbContext
{
    public VerifDbContext() { }
    public VerifDbContext(DbContextOptions<VerifDbContext> options) : base(options) { }

    public DbSet<TestEntity> TestEntities => Set<TestEntity>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=VerificationTestDb;Username=postgres;Password=12345");
        }
    }
}

public class ConcreteVerifRepository(VerifDbContext context)
    : EfEntityRepositoryBase<TestEntity, VerifDbContext, IBaseEntity>(context);
