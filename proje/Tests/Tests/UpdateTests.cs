using Tests.Entities;

namespace Tests.Tests;

[TestFixture]
public class UpdateTests
{
    private IProductDal _productDal = new ProductDal(SetupDbContext.DbContext);

    [Test]
    public void Update_ValidEntity_UpdatesEntityInDatabase()
    {
        // Arrange
        var product = new Product
        {
            Id = 1001,
            Name = "ProductName1001"
        };
        _productDal.Add(product);

        // Act
        product!.Name = "UpdatedProductName1001";
        _productDal.Update(product);

        var result = _productDal.Get(p => p.Id == product.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo("UpdatedProductName1001"));

        _productDal.DeleteAsync(product);
    }

    [Test]
    public void UpdateRange_ValidEntities_UpdatesEntitiesInDatabase()
    {
        // Arrange
        var products = new List<Product>
        {
            new()
            {
                Id = 1005,
                Name = "ProductName1005"
            },
            new()
            {
                Id = 1006,
                Name = "ProductName1006"
            }
        };
        _productDal.AddRange(products);

        // Act
        products.First().Name = "UpdatedProductName1005";
        products.Last().Name = "UpdatedProductName1006";
        _productDal.UpdateRange(products);

        var results = _productDal.GetAll(x => x.Id == 1005 || x.Id == 1006);

        // Assert
        Assert.That(results, Is.Not.Null);
        Assert.That(results, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(results.FirstOrDefault(p => p.Id == 1005 && p.Name == "UpdatedProductName1005"), Is.Not.Null);
            Assert.That(results.FirstOrDefault(p => p.Id == 1006 && p.Name == "UpdatedProductName1006"), Is.Not.Null);
        });

        _productDal.DeleteRange(products);
    }
}

