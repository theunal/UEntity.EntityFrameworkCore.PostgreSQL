using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using UEntity.EntityFrameworkCore.PostgreSQL;

namespace Tests.Tests.Verification;

[TestFixture]
public class IssueVerificationTests
{
    private VerifDbContext _db = null!;
    private const string ConnString = "Host=localhost;Port=5433;Database=VerificationTestDb;Username=postgres;Password=12345";

    [SetUp]
    public void Setup()
    {
        _db = CreateDatabase();
        SeedData(_db);
    }

    [TearDown]
    public void Teardown()
    {
        try { _db?.Database.EnsureDeleted(); } catch { }
        _db?.Dispose();
    }

    #region CRITICAL: Expression.Invoke SQL Translation Issue

    [Test]
    public void CRITICAL_And_ExpressionInvoke_SqLTranslation()
    {
        var repo = new ConcreteVerifRepository(_db);

        var p1 = (Expression<Func<TestEntity, bool>>)(x => x.Value > 10);
        var p2 = (Expression<Func<TestEntity, bool>>)(x => x.Name.Contains("a"));
        var combined = p1.And(p2);

        Exception? ex = null;
        List<TestEntity>? results = null;
        try
        {
            results = repo.GetAll(combined);
        }
        catch (Exception e)
        {
            ex = e;
        }

        if (ex != null)
        {
            TestContext.Out.WriteLine($"And() with Expression.Invoke THREW on PostgreSQL: {ex.GetType().Name}: {ex.Message}");
            Assert.Fail($"Expression.Invoke Npgsql'de SQL ceviri hatasi veriyor: {ex.Message}");
        }
        else
        {
            TestContext.Out.WriteLine("And() with Expression.Invoke CALISIYOR (PostgreSQL)");
            Assert.That(results, Is.Not.Null.And.Not.Empty);
            Assert.That(results!.Count, Is.EqualTo(3));
            Assert.That(results.All(r => r.Value > 10 && r.Name.Contains("a")), Is.True);
        }
    }

    [Test]
    public void CRITICAL_Or_ExpressionInvoke_SqLTranslation()
    {
        var repo = new ConcreteVerifRepository(_db);

        var p1 = (Expression<Func<TestEntity, bool>>)(x => x.Value > 10);
        var p2 = (Expression<Func<TestEntity, bool>>)(x => x.Name.Contains("A"));
        var combined = p1.Or(p2);

        Exception? ex = null;
        List<TestEntity>? results = null;
        try
        {
            results = repo.GetAll(combined);
        }
        catch (Exception e)
        {
            ex = e;
        }

        if (ex != null)
        {
            TestContext.Out.WriteLine($"Or() with Expression.Invoke THREW on PostgreSQL: {ex.GetType().Name}: {ex.Message}");
            Assert.Fail($"Expression.Invoke Npgsql'de SQL ceviri hatasi veriyor: {ex.Message}");
        }
        else
        {
            TestContext.Out.WriteLine("Or() with Expression.Invoke CALISIYOR (PostgreSQL)");
            Assert.That(results, Is.Not.Null.And.Not.Empty);
            Assert.That(results!.Count, Is.EqualTo(4));
        }
    }

    [Test]
    public void CRITICAL_ExpressionTree_HasInvocationNode()
    {
        var p1 = (Expression<Func<TestEntity, bool>>)(x => x.Value > 10);
        var p2 = (Expression<Func<TestEntity, bool>>)(x => x.Name.Contains("A"));
        var combined = p1.And(p2);

        var count = CountInvocationExpressions(combined);
        TestContext.Out.WriteLine($"Expression tree'de {count} adet InvocationExpression node'u var");
        Assert.That(count, Is.EqualTo(0), "And() InvocationExpression uretmemeli — ExpressionVisitor ile parametre replace edilmeli");
    }

    #endregion

    #region IMPORTANT: Bare catch swallows errors

    [Test]
    public void IMPORTANT_ToPaginate_BareCatch_SwallowsExceptions()
    {
        using var brokenDb = BrokenDatabase();
        var result = brokenDb.TestEntities.ToPaginate(1, 10);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.TotalCount, Is.EqualTo(0));
        Assert.That(result.Items, Is.Null);
        TestContext.Out.WriteLine("Bare catch exception yuttu — hata gizleniyor!");
    }

    [Test]
    public async Task IMPORTANT_ToPaginateAsync_BareCatch_SwallowsExceptions()
    {
        await using var brokenDb = BrokenDatabase();
        var result = await brokenDb.TestEntities.ToPaginateAsync(1, 10);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.TotalCount, Is.EqualTo(0));
        Assert.That(result.Items, Is.Null);
        TestContext.Out.WriteLine("Bare catch async exception yuttu — hata gizleniyor!");
    }

    #endregion

    #region IMPORTANT: SelectAs Reflection

    [Test]
    public void IMPORTANT_SelectAs_Reflection_NoCaching()
    {
        var repo = new ConcreteVerifRepository(_db);

        var r1 = repo.SelectAsAll<TestDto>();
        var r2 = repo.SelectAsAll<TestDto>();

        Assert.That(r1, Is.Not.Empty);
        Assert.That(r2, Is.Not.Empty);
        Assert.That(r1.Count, Is.EqualTo(r2.Count));
        TestContext.Out.WriteLine("SelectAs her cagrida reflection kullaniyor — cache'lenebilir");
    }

    #endregion

    #region IMPORTANT: TContext new() constraint

    [Test]
    public void IMPORTANT_TContext_NewConstraint_DemonstratesUnnecessary()
    {
        var options = new DbContextOptionsBuilder<VerifDbContext>()
            .UseNpgsql(ConnString)
            .Options;

        using var db = new VerifDbContext(options);
        db.Database.EnsureCreated();

        var repo = new ConcreteVerifRepository(db);
        Assert.That(repo, Is.Not.Null);

        TestContext.Out.WriteLine("new() constraint'i zorunlu degil — DI ile calisiyor ama TContext'in parameterless ctor'u olmak zorunda");
    }

    #endregion

    #region MODERATE: CancellationToken missing

    [Test]
    public void MODERATE_CancellationToken_Missing()
    {
        var interfaceType = typeof(IEntityRepository<TestEntity, IBaseEntity>);
        var eksikMetodlar = new[] { "SelectPaginateAsync", "SelectAsPaginateAsync", "SelectArrayAsync" };

        foreach (var name in eksikMetodlar)
        {
            var method = interfaceType.GetMethods()
                .FirstOrDefault(m => m.Name == name && m.IsGenericMethodDefinition);

            if (method != null)
            {
                var hasCt = method.GetParameters()
                    .Any(p => p.ParameterType == typeof(CancellationToken));

                TestContext.Out.WriteLine($"{name}: CancellationToken={(hasCt ? "VAR" : "YOK")}");
                if (!hasCt)
                    TestContext.Out.WriteLine($"  -> {name} icin CancellationToken EKLEMELI!");
            }
        }
    }

    #endregion

    #region MODERATE: Type mismatch

    [Test]
    public void MODERATE_Paginate_TypeMismatch()
    {
        var paginateType = typeof(Paginate<>);
        var totalProp = paginateType.GetProperty("TotalCount")!;
        var pagesProp = paginateType.GetProperty("PagesCount")!;

        Assert.That(totalProp.PropertyType, Is.EqualTo(typeof(long)));
        Assert.That(pagesProp.PropertyType, Is.EqualTo(typeof(long)));

        TestContext.Out.WriteLine("Paginate model'de TotalCount/PagesCount = long, ToPaginate extension'da int kullaniliyor");
    }

    [Test]
    public void MODERATE_ToPaginate_IntToLong_Works()
    {
        var paginate = _db.TestEntities.ToPaginate(1, 10);

        Assert.That(paginate.TotalCount, Is.TypeOf<long>());
        Assert.That(paginate.PagesCount, Is.TypeOf<long>());
        Assert.That(paginate.TotalCount, Is.EqualTo(4));
    }

    #endregion

    #region MODERATE: MaxAsync nullable inconsistency

    [Test]
    public void MODERATE_Max_WithData_ReturnsCorrectValue()
    {
        var repo = new ConcreteVerifRepository(_db);
        var max = repo.Max(x => x.Value);
        Assert.That(max, Is.EqualTo(35));
    }

    [Test]
    public void MODERATE_Min_WithData_ReturnsCorrectValue()
    {
        var repo = new ConcreteVerifRepository(_db);
        var min = repo.Min(x => x.Value);
        Assert.That(min, Is.EqualTo(5));
    }

    [Test]
    public async Task MODERATE_MaxAsync_WithData_ReturnsCorrectValue()
    {
        var repo = new ConcreteVerifRepository(_db);
        var max = await repo.MaxAsync(x => x.Value);
        Assert.That(max, Is.EqualTo(35));
    }

    [Test]
    public async Task MODERATE_MinAsync_WithData_ReturnsCorrectValue()
    {
        var repo = new ConcreteVerifRepository(_db);
        var min = await repo.MinAsync(x => x.Value);
        Assert.That(min, Is.EqualTo(5));
    }

    [Test]
    public void MODERATE_Max_ReturnType_BothNullable()
    {
        var interfaceType = typeof(IEntityRepository<TestEntity, IBaseEntity>);
        var methods = interfaceType.GetMethods()
            .Where(m => m.Name is "Max" or "MaxAsync" or "Min" or "MinAsync")
            .ToArray();

        foreach (var method in methods)
        {
            var ret = method.ReturnType;
            if (ret.IsGenericType && ret.GetGenericTypeDefinition() == typeof(Task<>))
            {
                var inner = ret.GetGenericArguments()[0];
                var isNullable = inner.IsGenericType && inner.GetGenericTypeDefinition() == typeof(Nullable<>);
                TestContext.Out.WriteLine($"{method.Name}: Task<{inner.Name}> (nullable={isNullable})");
            }
            else
            {
                var isNullable = ret.IsGenericType && ret.GetGenericTypeDefinition() == typeof(Nullable<>);
                TestContext.Out.WriteLine($"{method.Name}: {ret.Name} (nullable={isNullable})");
            }
        }
    }

    #endregion

    #region MINOR: Typo

    [Test]
    public void MINOR_PackageTags_Typo()
    {
        var csprojPath = Path.Combine(
            "C:", "Users", "UNAL", "Desktop",
            "UEntity.EntityFrameworkCore.PostgreSQL",
            "UEntity.EntityFrameworkCore.PostgreSQL",
            "UEntity.EntityFrameworkCore.PostgreSQL.csproj");

        Assert.That(File.Exists(csprojPath), Is.True, "csproj bulunamadi");
        var content = File.ReadAllText(csprojPath);
        Assert.That(content, Does.Contain("postgresql"));
        TestContext.Out.WriteLine("PackageTags duzeltilmis: postgresql");
    }

    #endregion

    #region Helpers

    private static VerifDbContext CreateDatabase()
    {
        var ctx = new VerifDbContext();
        ctx.Database.EnsureCreated();
        ctx.TestEntities.ExecuteDelete();
        return ctx;
    }

    private static void SeedData(VerifDbContext db)
    {
        if (!db.TestEntities.Any())
        {
            db.TestEntities.AddRange(
                new TestEntity { Id = 1, Name = "Alpha", Value = 5, CreatedAt = DateTime.UtcNow },
                new TestEntity { Id = 2, Name = "Beta", Value = 15, CreatedAt = DateTime.UtcNow },
                new TestEntity { Id = 3, Name = "Gamma", Value = 25, CreatedAt = DateTime.UtcNow },
                new TestEntity { Id = 4, Name = "Delta", Value = 35, CreatedAt = DateTime.UtcNow }
            );
            db.SaveChanges();
        }
    }

    private static VerifDbContext BrokenDatabase()
    {
        var options = new DbContextOptionsBuilder<VerifDbContext>()
            .UseNpgsql("Host=localhost;Port=5433;Database=VerificationTestDb_NotFound;Username=postgres;Password=12345")
            .Options;

        return new VerifDbContext(options);
    }

    private static VerifDbContext EmptyDatabase()
    {
        var ctx = new VerifDbContext();
        ctx.Database.EnsureCreated();
        ctx.TestEntities.ExecuteDelete();
        return ctx;
    }

    private static int CountInvocationExpressions(Expression expr)
    {
        var visitor = new InvocationCountVisitor();
        visitor.Visit(expr);
        return visitor.Count;
    }

    private class InvocationCountVisitor : ExpressionVisitor
    {
        public int Count { get; private set; }
        protected override Expression VisitInvocation(InvocationExpression node)
        {
            Count++;
            return base.VisitInvocation(node);
        }
    }

    #endregion
}
