### This IEntityRepository interface represents a generic entity repository interface. This interface defines standard CRUD (Create, Read, Update, Delete) operations for interacting with entities in a database table and provides various methods for query operations.

## Features:

Various Query Methods: Various query methods such as Get, GetAll, GetArray, etc. support operations such as filtering, sorting, tracking.

Asynchronous Operations: Methods ending with async can run asynchronously so that the thread is not blocked.

Query Results: Query methods return entity objects, usually of type T, or specific properties of those entity objects.

Dynamic Filtering: Filtering operations can be configured dynamically using LINQ expressions.
Sorting and Tracking: Some methods take parameters to configure sorting or tracking behaviour.

Batch Operations: Batch operations such as AddRange, UpdateRange, DeleteRange can be used to manipulate entity objects in a collection.

This interface is often used to abstract the data access operations of entity classes and reduce dependencies. It allows database operations to be independently testable and improves the overall performance and readability of the code.

## Examples

```c#

// Definition
public class Product : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}

public interface IProductDal : IEntityRepository<Product>
{

}

public class ProductDal(TestDbContext context) : EfEntityRepositoryBase<Product, TestDbContext>(context), IProductDal
{

}

// Usage
public class ProductService(IProductDal productDal)
{
    // Counting the number of entities that match the specified filter
    int count = productDal.Count(e => e.IsActive);

    // Getting a single entity that matches the specified filter
    var entity = productDal.Get(e => e.Id == 1);

    // Getting all entities
    var allEntities = productDal.GetAll();

    // Get select
    var model = productDal.Get<Model>(x => x.Id == 5, x => new Model {
        Name = x.Name
    });

    // Get selects
    var models = productDal.GetSelectList<Model>(x => new Model {
        Name = x.Name
    }, x => x.Name.Contains("11"));

    // Getting an array of entities that match the specified filter and sorting them
    var sortedEntities = productDal.GetArray(filter: e => e.IsActive, sort: new EntitySortModel<MyEntity> { sort = x => x.Name });

    // Getting a paginated list of entities
    var paginatedList = productDal.GetListPaginate(page: 1, size: 10, filter: e => e.IsActive);

    // Adding a new entity to the repository
    var newEntity = new MyEntity { Name = "New Entity" };
    productDal.Add(newEntity);

    // Updating an existing entity in the repository
    var existingEntity = productDal.Get(e => e.Id == 1);
    existingEntity.Name = "Updated Entity";
    productDal.Update(existingEntity);

    // Deleting an entity from the repository
    var entityToDelete = productDal.Get(e => e.Id == 2);
    productDal.Delete(entityToDelete);

    // Asynchronously counting the number of entities that match the specified filter
    var countAsync = await productDal.CountAsync(e => e.IsActive);

    // Asynchronously getting a single entity that matches the specified filter
    var entityAsync = await productDal.GetAsync(e => e.Id == 1);

    // Asynchronously getting all entities
    var allEntitiesAsync = await productDal.GetAllAsync();

    // Asynchronously getting an array of entities that match the specified filter and sorting them
    var sortedEntitiesAsync = await productDal.GetArrayAsync(filter: e => e.IsActive, sort: new EntitySortModel<MyEntity> { sort = x => x.Name });

    // Asynchronously getting a paginated list of entities
    var paginatedListAsync = await productDal.GetListPaginateAsync(page: 1, size: 10, filter: e => e.IsActive);

    // Asynchronously adding a new entity to the repository
    var newEntityAsync = new MyEntity { Name = "New Entity" };
    await productDal.AddAsync(newEntityAsync);

    // Asynchronously updating an existing entity in the repository
    var existingEntityAsync = await productDal.GetAsync(e => e.Id == 1);
    existingEntityAsync.Name = "Updated Entity";
    await productDal.UpdateAsync(existingEntityAsync);

    // Asynchronously deleting an entity from the repository
    var entityToDeleteAsync = await productDal.GetAsync(e => e.Id == 2);
    await productDal.DeleteAsync(entityToDeleteAsync);

    // Asynchronously executing a delete operation on entities that match the specified filter
    await productDal.ExecuteDeleteAsync(x => x.Name.Contains("55"));

    // Asynchronously executing an update operation on entities that match the specified filter
    await agentDal.ExecuteUpdateAsync(x => x.SetProperty(p => p.Name, p => "Updated"), x => x.Id == 11);
}
