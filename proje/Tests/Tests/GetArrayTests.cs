using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Tests.Entities;
using UEntity.EntityFrameworkCore.PostgreSQL;

namespace Tests.Tests;

[TestFixture]
public class GetArrayTests
{
    private IProductDal _productDal = new ProductDal(SetupDbContext.DbContext);
    [Test]
    public void GetArray_WithValidFilter_ReturnsItems()
    {
        // Arrange
        int expectedCount = 9;

        // Act
        var result = _productDal.GetArray(e => e.Id >= 1 && e.Id < 10);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Length.EqualTo(expectedCount));
    }

    [Test]
    public void GetArray_WithInvalidFilter_ReturnsAllItems()
    {
        // Arrange
        int expectedCount = 100;

        // Act
        var result = _productDal.GetArray();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Length.EqualTo(expectedCount));
    }

    [Test]
    public void GetArray_WithSort_ReturnsSortedItems()
    {
        // Arrange
        int expectedFirstId = 1;
        string expectedFirstName = "ProductName1";

        // Act
        var result = _productDal.GetArray(sort: new EntitySortModel<Product> { Sort = x => x.Id });

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result[0].Id, Is.EqualTo(expectedFirstId));
            Assert.That(result[0].Name, Is.EqualTo(expectedFirstName));
        });
    }

    [Test]
    public void GetArray_WithSort_ReturnsSortedResults()
    {
        // Arrange
        var sortModel = new EntitySortModel<Product>
        {
            Sort = p => p.Name,
            SortType = SortOrder.Ascending
        };

        // Act
        var result = _productDal.GetArray(sort: sortModel);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Length.EqualTo(100));
        Assert.Multiple(() =>
        {
            Assert.That(result[0].Name, Is.EqualTo("ProductName1"));
            Assert.That(result[1].Name, Is.EqualTo("ProductName10"));
            Assert.That(result[2].Name, Is.EqualTo("ProductName100"));
        });
    }

    [Test]
    public void GetArray_WithSortDescending_ReturnsSortedResults()
    {
        // Arrange
        var sortModel = new EntitySortModel<Product>
        {
            Sort = p => p.Id,
            SortType = SortOrder.Descending
        };

        // Act
        var result = _productDal.GetArray(sort: sortModel);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Length.EqualTo(100));
        Assert.Multiple(() =>
        {
            Assert.That(result[0].Id, Is.EqualTo(100));
            Assert.That(result[1].Id, Is.EqualTo(99));
            Assert.That(result[2].Id, Is.EqualTo(98));
        });
    }
}
