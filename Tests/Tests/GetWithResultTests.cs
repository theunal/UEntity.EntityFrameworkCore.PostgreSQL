using Tests.Entities;

namespace Tests.Tests;

[TestFixture]
public class GetWithResultTests
{
    private IProductDal _productDal = new ProductDal(SetupDbContext.DbContext);

    [Test]
    public void GetWithResult_WithValidFilter_ReturnsResult()
    {
        // Arrange
        int expectedId = 1;
        string expectedName = "ProductName1";

        // Act
        var result = _productDal.Get(e => e.Id == expectedId, e => e.Name);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(expectedName, Is.EqualTo(result));
    }

    [Test]
    public void GetWithResult_WithInvalidFilter_ReturnsNull()
    {
        // Arrange
        int invalidId = 1000;

        // Act
        var result = _productDal.Get(e => e.Id == invalidId, e => e.Name);

        // Assert
        Assert.That(result, Is.Null);
    }
}