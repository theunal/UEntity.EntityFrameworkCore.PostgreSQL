using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using UEntity.EntityFrameworkCore.PostgreSQL;
using Tests.Entities;

namespace Tests.Tests;

[TestFixture]
public class GetTakeListTests
{
    private IProductDal _productDal = new ProductDal(SetupDbContext.DbContext);

    [Test]
    public void GetTakeList_WithValidCountAndSort_ReturnsCorrectItems()
    {
        // Arrange
        int count = 10;

        // Act
        var result = _productDal.GetListPaginate(0, count, sort: new EntitySortModel<Product> { Sort = x => x.Id, SortType = SortOrder.Ascending });

        // Assert
        Assert.That(result?.Items, Is.Not.Null);
        Assert.That(result?.Items, Has.Count.EqualTo(count));
        Assert.Multiple(() =>
        {
            Assert.That(result?.Items.First().Id, Is.EqualTo(1));
            Assert.That(result?.Items.Last().Id, Is.EqualTo(count));
        });
    }

    [Test]
    public void GetTakeList_WithFilter_ReturnsFilteredItems()
    {
        // Arrange
        int count = 10;

        // Act
        var result = _productDal.GetListPaginate(0, count, sort: new EntitySortModel<Product> { Sort = x => x.Id, SortType = SortOrder.Ascending }, filter: p => p.Id > 50);

        // Assert
        Assert.That(result?.Items, Is.Not.Null);
        Assert.That(result?.Items, Has.Count.EqualTo(count));
        Assert.Multiple(() =>
        {
            Assert.That(result?.Items.First().Id, Is.EqualTo(51));
            Assert.That(result?.Items.Last().Id, Is.EqualTo(60));
        });
    }

    [Test]
    public void GetTakeList_WithSortDescending_ReturnsSortedItems()
    {
        // Arrange
        int count = 10;

        // Act
        var result = _productDal.GetListPaginate(0, count, sort: new EntitySortModel<Product> { Sort = x => x.Id, SortType = SortOrder.Descending });

        // Assert
        Assert.That(result?.Items, Is.Not.Null);
        Assert.That(result?.Items, Has.Count.EqualTo(count));
        Assert.Multiple(() =>
        {
            Assert.That(result?.Items.First().Id, Is.EqualTo(100));
            Assert.That(result?.Items.Last().Id, Is.EqualTo(91));
        });
    }

    [Test]
    public void GetTakeList_WithSortAndFilter_ReturnsSortedFilteredItems()
    {
        // Arrange
        int count = 5;

        // Act
        var result = _productDal.GetListPaginate(0, count, sort: new EntitySortModel<Product> { Sort = x => x.Id, SortType = SortOrder.Ascending }, filter: p => p.Name.Contains("Name9"));

        // Assert
        Assert.That(result?.Items, Is.Not.Null);
        Assert.That(result?.Items, Has.Count.EqualTo(count));
        Assert.Multiple(() =>
        {
            Assert.That(result?.Items.First().Name, Is.EqualTo("ProductName9"));
            Assert.That(result?.Items.Last().Name, Is.EqualTo("ProductName93"));
        });
    }
}