using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tests.Entities;

namespace Tests.Tests;

[TestFixture]
public class DeleteTests
{
    private IProductDal _productDal = new ProductDal(SetupDbContext.DbContext);

    [Test]
    public void Delete_ValidEntity_DeletesEntityFromDatabase()
    {
        // Arrange
        var productToDelete = _productDal.Get(p => p.Id == 3);

        // Act
        _productDal.Delete(productToDelete!);

        var result = _productDal.Get(p => p.Id == 3);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task DeleteRange_ValidEntities_DeletesEntitiesFromDatabase()
    {
        // Arrange
        var productsToDelete = await _productDal.GetArrayAsync(p => p.Id == 1 || p.Id == 2);

        // Act
        _productDal.DeleteRange(productsToDelete);

        var result = await _productDal.GetAllAsync(p => p.Id == 1 || p.Id == 2);

        // Assert
        Assert.That(result, Is.Empty);
    }
}