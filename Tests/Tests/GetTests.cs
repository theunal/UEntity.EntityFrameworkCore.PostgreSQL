using Tests.Entities;

namespace Tests.Tests;

[TestFixture]
public class GetTests
{
    private IProductDal _productDal = new ProductDal(SetupDbContext.DbContext);

    [Test]
    public void Get_WithValidFilter_ReturnsItem()
    {
        // Arrange
        int expectedId = 1;
        string expectedName = "ProductName1";

        // Act
        var result = _productDal.Get(e => e.Id == expectedId);

        // Assert

        Assert.That(result, Is.Not.Null);
        Assert.That(expectedId, Is.EqualTo(result.Id));
        Assert.That(expectedName, Is.EqualTo(result.Name));
    }

    [Test]
    public void Get_WithInvalidFilter_ReturnsNull()
    {
        // Arrange
        int invalidId = 1000;

        // Act
        var result = _productDal.Get(e => e.Id == invalidId);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetAsync_WithValidFilter_ReturnsItem()
    {
        // Arrange
        int expectedId = 2;
        string expectedName = "ProductName2";

        // Act
        var result = await _productDal.GetAsync(e => e.Id == expectedId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(expectedId, Is.EqualTo(result.Id));
        Assert.That(expectedName, Is.EqualTo(result.Name));
    }

    [Test]
    public async Task GetAsync_WithInvalidFilter_ReturnsNull()
    {
        // Arrange
        int invalidId = 1000;

        // Act
        var result = await _productDal.GetAsync(e => e.Id == invalidId);

        // Assert
        Assert.That(result, Is.Null);
    }
}