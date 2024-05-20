using Tests.Entities;

namespace Tests.Tests;

[TestFixture]
public class AddTests
{
    private IProductDal _productDal = new ProductDal(SetupDbContext.DbContext);

    public class Model
    {
        public string Name { get; set; } = null!;
    }

    [Test, Order(1)]
    public void AddRange_ValidEntities_AddsEntitiesToDatabase()
    {

        var model = _productDal.Get<Model>(x => x.Id == 5, x => new Model
        {
            Name = x.Name
        });

        var models = _productDal.GetSelectList<Model>(x => new Model
        {
            Name = x.Name
        }, x => x.Name.Contains(""));

        // Arrange
        var newProducts = new List<Product>
        {
            new Product { Id = 8888, Name = "ProductName11" },
            new Product { Id = 8889, Name = "ProductName12" }
        };

        // Act
        _productDal.AddRange(newProducts);

        var results = _productDal.GetAll(p => p.Id == 8888 || p.Id == 8889);

        // Assert
        Assert.That(results, Is.Not.Null);
        Assert.That(results, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(results.Exists(p => p.Id == 8888 && p.Name == "ProductName11"), Is.True);
            Assert.That(results.Exists(p => p.Id == 8889 && p.Name == "ProductName12"), Is.True);
        });

        _productDal.DeleteRange(newProducts);

        // Arrange
        var newProduct = new Product
        {
            Id = 999,
            Name = "ProductName999"
        };

        // Act
        _productDal.Add(newProduct);

        var result = _productDal.Get(x => x.Id == newProduct.Id);

        // Assert
        Assert.That(actual: result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(newProduct.Id));
            Assert.That(result.Name, Is.EqualTo(newProduct.Name));
        });

        _productDal.Delete(result);
    }

    [Test, Order(3)]
    public void Add_DuplicateId_ThrowsException()
    {
        // Arrange
        var duplicateProduct = new Product
        {
            Id = 1,
            Name = "DuplicateProduct"
        };

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() =>
        {
            _productDal.Add(duplicateProduct);
        });

        Assert.That(ex, Is.Not.Null);
    }

    [Test, Order(2)]
    public void AddRange_DuplicateId_ThrowsException()
    {
        // Arrange
        var duplicateProducts = new List<Product>
        {
            new Product { Id = 1, Name = "DuplicateProduct1" },
            new Product { Id = 2, Name = "DuplicateProduct2" }
        };

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() =>
        {
            _productDal.AddRange(duplicateProducts);
        });

        Assert.That(ex, Is.Not.Null);
    }
}
