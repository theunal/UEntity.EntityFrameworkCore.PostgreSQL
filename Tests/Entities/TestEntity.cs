using Tests.Contexts;
using UEntity.EntityFrameworkCore.PostgreSQL;

namespace Tests.Entities;

public class TestEntity : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}

public interface ITestEntityDal : IEntityRepository<TestEntity>
{

}

public class TestEntityDal(TestDbContext context) : EfEntityRepositoryBase<TestEntity, TestDbContext>(context), ITestEntityDal
{

}