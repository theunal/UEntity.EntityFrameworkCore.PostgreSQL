using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using UEntity.EntityFrameworkCore.PostgreSQL;
using Tests.Entities;

namespace Tests.Tests;

[TestFixture]
public class GetListPaginateTests
{
    private IProductDal _productDal = new ProductDal(SetupDbContext.DbContext);

    [Test]
    public void GetListPaginate_WithValidFilterAndSort_ReturnsPaginatedItems()
    {
        // Arrange
        int pageIndex = 0;
        int pageSize = 10;
        int expectedCount = 10;
        string expectedFirstName = "ProductName21";

        // Act
        var result = _productDal.GetListPaginate(pageIndex, pageSize, x => x.Id > 20, sort: new EntitySortModel<Product>
        {
            Sort = x => x.Id,
            SortType = SortOrder.Ascending
        });

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Items, Has.Count.EqualTo(expectedCount));
            Assert.That(result.Items[0].Name, Is.EqualTo(expectedFirstName));
        });
    }

    [Test]
    public void GetListPaginate_WithFilter_ReturnsFilteredItems()
    {
        // Arrange
        int pageIndex = 0;
        int pageSize = 10;
        int expectedCount = 9;

        // Act
        var result = _productDal.GetListPaginate(pageIndex, pageSize, p => p.Id >= 1 && p.Id < 10);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Items, Has.Count.EqualTo(expectedCount));
            Assert.That(result.Items[0].Id, Is.EqualTo(1));
            Assert.That(result.Items[expectedCount - 1].Id, Is.EqualTo(9));
        });
    }

    [Test]
    public void GetListPaginate_WithSort_ReturnsSortedItems()
    {
        // Arrange
        int pageIndex = 0;
        int pageSize = 10;
        int expectedCount = 10;
        string expectedFirstName = "ProductName1";
        var sort = new EntitySortModel<Product> { Sort = x => x.Name, SortType = SortOrder.Ascending };

        // Act
        var result = _productDal.GetListPaginate(pageIndex, pageSize, sort: sort);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Items, Has.Count.EqualTo(expectedCount));
            Assert.That(result.Items[0].Name, Is.EqualTo(expectedFirstName));
        });
    }

    [Test]
    public void GetListPaginate_WithSortDescending_ReturnsSortedItems()
    {
        // Arrange
        int pageIndex = 0;
        int pageSize = 10;
        int expectedCount = 10;
        int expectedFirstId = 100;
        var sort = new EntitySortModel<Product> { Sort = x => x.Id, SortType = SortOrder.Descending };

        // Act
        var result = _productDal.GetListPaginate(pageIndex, pageSize, sort: sort);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Items, Has.Count.EqualTo(expectedCount));
            Assert.That(result.Items[0].Id, Is.EqualTo(expectedFirstId));
        });
    }
}
