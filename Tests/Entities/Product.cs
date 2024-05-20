using Tests.Contexts;
using UEntity.EntityFrameworkCore.PostgreSQL;

namespace Tests.Entities;

public class Product : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}

public interface IProductDal : IEntityRepository<Product>
{

}

public class ProductDal(TestDbContext context) : EfEntityRepositoryBase<Product, TestDbContext>(context), IProductDal
{

}