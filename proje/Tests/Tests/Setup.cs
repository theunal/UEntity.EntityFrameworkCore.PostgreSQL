using Microsoft.EntityFrameworkCore;
using Tests.Contexts;
using Tests.Entities;

namespace Tests.Tests;

[SetUpFixture]
public class TestSetup
{
    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        SetupDbContext.DbContext = new TestDbContext();

        SetupDbContext.DbContext.Products.ExecuteDelete();
        SetupDbContext.DbContext.SaveChanges();

        SetupDbContext.DbContext.Products.AddRange(Enumerable.Range(1, 100).Select(i => new Product { Id = i, Name = $"ProductName{i}" }));
        SetupDbContext.DbContext.SaveChanges();
    }
}

public static class SetupDbContext
{
    public static TestDbContext DbContext { get; set; } = null!;
}
