using Tests.Entities;

namespace Tests.Tests;

[TestFixture]
public class AnyTests
{
    private IProductDal _productDal = new ProductDal(SetupDbContext.DbContext);

    [Test]
    public async Task AnyAsync_ValidFilter_ReturnsTrue()
    {
        var result = await _productDal.AnyAsync(x => x.Id == 1);

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task AnyAsync_InvalidFilter_ReturnsFalse()
    {
        var result = await _productDal.AnyAsync(x => x.Id == 98948);

        Assert.That(result, Is.False);
    }

    [Test]
    public async Task AnyAsync_FilterWithNoMatch_ReturnsFalse()
    {
        var result = await _productDal.AnyAsync(x => x.Name == "NonExistentProduct");

        Assert.That(result, Is.False);
    }

    [Test]
    public async Task AnyAsync_AllItemsMatch_ReturnsTrue()
    {
        var result = await _productDal.AnyAsync(x => x.Id >= 1);

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task AnyAsync_CancellationToken_CanBeCancelled()
    {
        using var cts = new CancellationTokenSource();
        var token = cts.Token;
        cts.Cancel();

        Assert.ThrowsAsync<OperationCanceledException>(() => _productDal.AnyAsync(x => x.Id == 1, token));
    }

    [Test]
    public void Any_ValidFilter_ReturnsTrue()
    {
        var result = _productDal.Any(x => x.Id == 50);

        Assert.That(result, Is.True);
    }

    [Test]
    public void Any_InvalidFilter_ReturnsFalse()
    {
        var result = _productDal.Any(x => x.Id == 999);

        Assert.That(result, Is.False);
    }
}
