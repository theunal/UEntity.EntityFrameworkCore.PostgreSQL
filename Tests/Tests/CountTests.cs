using Tests.Entities;

namespace Tests.Tests;

[TestFixture]
public class CountTests
{
    private IProductDal _productDal = new ProductDal(SetupDbContext.DbContext);

    [Test]
    public void Count_ValidFilter_ReturnsCorrectCount()
    {
        // Act
        var result = _productDal.Count(x => x.Id % 2 == 0);

        // Assert
        Assert.That(result, Is.EqualTo(50)); // Çünkü 100 veri var ve filtreleme sonucunda 50 çift sayı olmalı
    }

    [Test]
    public void Count_NullFilter_ReturnsTotalCount()
    {
        // Act
        var result = _productDal.Count();

        // Assert
        Assert.That(result, Is.EqualTo(100)); // 100 veri var
    }

    [Test]
    public async Task Count_ValidFilter_ReturnsCorrectCountAsync()
    {
        // Act
        var result = await _productDal.CountAsync(x => x.Id % 2 == 0);

        // Assert
        Assert.That(result, Is.EqualTo(50)); // Çünkü 100 veri var ve filtreleme sonucunda 50 çift sayı olmalı
    }

    [Test]
    public async Task Count_NullFilter_ReturnsTotalCountAsync()
    {
        // Act
        var result = await _productDal.CountAsync();

        // Assert
        Assert.That(result, Is.EqualTo(100)); // 100 veri var
    }
}
