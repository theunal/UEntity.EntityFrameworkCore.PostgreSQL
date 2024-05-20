using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace UEntity.EntityFrameworkCore.PostgreSQL;

/// <summary>
/// Defines a repository interface for entities of type T.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public interface IEntityRepository<T> where T : class, IEntity, new()
{
    /// <summary>
    /// Counts the number of entities that match the specified filter.
    /// </summary>
    /// <param name="filter">The filter expression.</param>
    /// <returns>The number of entities that match the filter.</returns>
    int Count(Expression<Func<T, bool>>? filter = null);

    /// <summary>
    /// Counts the number of entities that match the specified filter.
    /// </summary>
    /// <param name="filter">The filter expression.</param>
    /// <returns>The number of entities that match the filter.</returns>
    Task<int> CountAsync(Expression<Func<T, bool>>? filter = null);

    /// <summary>
    /// Retrieves a single entity that matches the specified filter, with optional sorting and tracking behavior.
    /// </summary>
    /// <param name="filter">The filter expression.</param>
    /// <param name="sort">The sorting model.</param>
    /// <param name="asNoTracking">Whether to track the entity or not.</param>
    /// <returns>The entity that matches the filter, or null if no entity is found.</returns>
    T? Get(Expression<Func<T, bool>> filter, EntitySortModel<T>? sort = null, bool? asNoTracking = false);

    /// <summary>
    /// Retrieves a single entity that matches the specified filter and selects the specified result, with optional sorting and tracking behavior.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="filter">The filter expression.</param>
    /// <param name="select">The selection function.</param>
    /// <param name="sort">The sorting model.</param>
    /// <param name="asNoTracking">Whether to track the entity or not.</param>
    /// <returns>The selected result of the entity that matches the filter, or null if no entity is found.</returns>
    TResult? Get<TResult>(Expression<Func<T, bool>> filter, Func<T, TResult> select, EntitySortModel<T>? sort = null, bool? asNoTracking = false);

    /// <summary>
    /// Asynchronously retrieves a single entity that matches the specified filter, with optional sorting and tracking behavior.
    /// </summary>
    /// <param name="filter">The filter expression.</param>
    /// <param name="sort">The sorting model.</param>
    /// <param name="asNoTracking">Whether to track the entity or not.</param>
    /// <returns>A task that represents the asynchronous operation, containing the entity that matches the filter, or null if no entity is found.</returns>
    Task<T?> GetAsync(Expression<Func<T, bool>> filter, EntitySortModel<T>? sort = null, bool? asNoTracking = false);

    /// <summary>
    /// Retrieves all entities that match the specified filter, with optional sorting and tracking behavior.
    /// </summary>
    /// <param name="filter">The filter expression.</param>
    /// <param name="sort">The sorting model.</param>
    /// <param name="asNoTracking">Whether to track the entities or not.</param>
    /// <returns>A list of entities that match the filter.</returns>
    List<T> GetAll(Expression<Func<T, bool>>? filter = null, EntitySortModel<T>? sort = null, bool? asNoTracking = false);

    /// <summary>
    /// Retrieves all entities that match the specified filter and selects the distinct result, with optional sorting and tracking behavior.
    /// </summary>
    /// <typeparam name="TResult">The type of the distinct result.</typeparam>
    /// <param name="filter">The filter expression.</param>
    /// <param name="sort">The sorting model.</param>
    /// <param name="asNoTracking">Whether to track the entities or not.</param>
    /// <param name="distincby">The distinct by function.</param>
    /// <returns>A list of distinct results of entities that match the filter.</returns>
    List<T> GetAll<TResult>(Expression<Func<T, bool>>? filter = null, EntitySortModel<T>? sort = null, bool? asNoTracking = false, Func<T, TResult>? distincby = null);

    /// <summary>
    /// Asynchronously retrieves all entities that match the specified filter, with optional sorting and tracking behavior.
    /// </summary>
    /// <param name="filter">The filter expression.</param>
    /// <param name="sort">The sorting model.</param>
    /// <param name="asNoTracking">Whether to track the entities or not.</param>
    /// <returns>A task that represents the asynchronous operation, containing a list of entities that match the filter.</returns>
    Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, EntitySortModel<T>? sort = null, bool? asNoTracking = false);

    /// <summary>
    /// Retrieves an array of entities that match the specified filter, with optional sorting and tracking behavior.
    /// </summary>
    /// <param name="filter">The filter expression.</param>
    /// <param name="sort">The sorting model.</param>
    /// <param name="asNoTracking">Whether to track the entities or not.</param>
    /// <returns>An array of entities that match the filter.</returns>
    T[] GetArray(Expression<Func<T, bool>>? filter = null, EntitySortModel<T>? sort = null, bool? asNoTracking = false);

    /// <summary>
    /// Retrieves an array of entities that match the specified filter and selects the distinct result, with optional sorting and tracking behavior.
    /// </summary>
    /// <typeparam name="TResult">The type of the distinct result.</typeparam>
    /// <param name="filter">The filter expression.</param>
    /// <param name="sort">The sorting model.</param>
    /// <param name="asNoTracking">Whether to track the entities or not.</param>
    /// <param name="distincby">The distinct by function.</param>
    /// <returns>An array of distinct results of entities that match the filter.</returns>
    T[] GetArray<TResult>(Expression<Func<T, bool>>? filter = null, EntitySortModel<T>? sort = null, bool? asNoTracking = false, Func<T, TResult>? distincby = null);

    /// <summary>
    /// Asynchronously retrieves an array of entities that match the specified filter, with optional sorting and tracking behavior.
    /// </summary>
    /// <param name="filter">The filter expression.</param>
    /// <param name="sort">The sorting model.</param>
    /// <param name="asNoTracking">Whether to track the entities or not.</param>
    /// <returns>A task that represents the asynchronous operation, containing an array of entities that match the filter.</returns>
    Task<T[]> GetArrayAsync(Expression<Func<T, bool>>? filter = null, EntitySortModel<T>? sort = null, bool? asNoTracking = false);

    /// <summary>
    /// Retrieves a paginated list of entities that match the specified filter, with optional sorting and tracking behavior.
    /// </summary>
    /// <param name="page">The page number.</param>
    /// <param name="size">The size of the page.</param>
    /// <param name="filter">The filter expression.</param>
    /// <param name="sort">The sorting model.</param>
    /// <param name="asNoTracking">Whether to track the entities or not.</param>
    /// <returns>A paginated list of entities that match the filter.</returns>
    Paginate<T> GetListPaginate(int page, int size, Expression<Func<T, bool>>? filter = null, EntitySortModel<T>? sort = null, bool? asNoTracking = false);

    /// <summary>
    /// Retrieves a paginated list of distinct results for entities that match the specified filter, with optional sorting and tracking behavior.
    /// </summary>
    /// <typeparam name="TResult">The type of the distinct result.</typeparam>
    /// <param name="page">The page number.</param>
    /// <param name="size">The size of the page.</param>
    /// <param name="filter">The filter expression.</param>
    /// <param name="sort">The sorting model.</param>
    /// <param name="asNoTracking">Whether to track the entities or not.</param>
    /// <param name="distincby">The distinct by function.</param>
    /// <returns>A paginated list of distinct results of entities that match the filter.</returns>
    Paginate<T> GetListPaginate<TResult>(int page, int size, Expression<Func<T, bool>>? filter = null, EntitySortModel<T>? sort = null, bool? asNoTracking = false, Func<T, TResult>? distincby = null);

    /// <summary>
    /// Asynchronously retrieves a paginated list of entities that match the specified filter, with optional sorting and tracking behavior.
    /// </summary>
    /// <param name="page">The page number.</param>
    /// <param name="size">The size of the page.</param>
    /// <param name="filter">The filter expression.</param>
    /// <param name="sort">The sorting model.</param>
    /// <param name="asNoTracking">Whether to track the entities or not.</param>
    /// <returns>A task that represents the asynchronous operation, containing a paginated list of entities that match the filter.</returns>
    Task<Paginate<T>> GetListPaginateAsync(int page, int size, Expression<Func<T, bool>>? filter = null, EntitySortModel<T>? sort = null, bool? asNoTracking = false);

    /// <summary>
    /// Retrieves a list of selected results for entities that match the specified filter, with optional sorting and tracking behavior.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="select">The selection function.</param>
    /// <param name="filter">The filter expression.</param>
    /// <param name="sort">The sorting model.</param>
    /// <param name="asNoTracking">Whether to track the entities or not.</param>
    /// <returns>A list of selected results of entities that match the filter.</returns>
    List<TResult> GetSelectList<TResult>(Func<T, TResult> select, Expression<Func<T, bool>>? filter = null, EntitySortModel<T>? sort = null, bool? asNoTracking = false);

    /// <summary>
    /// Retrieves an array of selected results for entities that match the specified filter, with optional sorting and tracking behavior.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="select">The selection function.</param>
    /// <param name="filter">The filter expression.</param>
    /// <param name="sort">The sorting model.</param>
    /// <param name="asNoTracking">Whether to track the entities or not.</param>
    /// <returns>An array of selected results of entities that match the filter.</returns>
    TResult[] GetSelectArray<TResult>(Func<T, TResult> select, Expression<Func<T, bool>>? filter = null, EntitySortModel<T>? sort = null, bool? asNoTracking = false);

    /// <summary>
    /// Retrieves a paginated list of selected results for entities that match the specified filter, with optional sorting and tracking behavior.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="page">The page number.</param>
    /// <param name="size">The size of the page.</param>
    /// <param name="select">The selection function.</param>
    /// <param name="filter">The filter expression.</param>
    /// <param name="sort">The sorting model.</param>
    /// <param name="asNoTracking">Whether to track the entities or not.</param>
    /// <returns>A paginated list of selected results of entities that match the filter.</returns>
    Paginate<TResult> GetSelectPaginateList<TResult>(int page, int size, Func<T, TResult> select, Expression<Func<T, bool>>? filter = null, EntitySortModel<T>? sort = null,
        bool? asNoTracking = false);

    /// <summary>
    /// Retrieves a list of entities that match the specified filter and takes the specified number of entities, with optional sorting and tracking behavior.
    /// </summary>
    /// <param name="count">The number of entities to take.</param>
    /// <param name="sort">The sorting model.</param>
    /// <param name="filter">The filter expression.</param>
    /// <param name="asNoTracking">Whether to track the entities or not.</param>
    /// <returns>A list of entities that match the filter.</returns>
    List<T> GetTakeList(int count, EntitySortModel<T> sort, Expression<Func<T, bool>>? filter = null, bool? asNoTracking = false);

    /// <summary>
    /// Retrieves an array of entities that match the specified filter and takes the specified number of entities, with optional sorting and tracking behavior.
    /// </summary>
    /// <param name="count">The number of entities to take.</param>
    /// <param name="sort">The sorting model.</param>
    /// <param name="filter">The filter expression.</param>
    /// <param name="asNoTracking">Whether to track the entities or not.</param>
    /// <returns>An array of entities that match the filter.</returns>
    T[] GetTakeArray(int count, EntitySortModel<T> sort, Expression<Func<T, bool>>? filter = null, bool? asNoTracking = false);

    /// <summary>
    /// Adds the specified entity to the repository.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    void Add(T entity);

    /// <summary>
    /// Asynchronously adds the specified entity to the repository.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>A task that represents the asynchronous add operation.</returns>
    Task AddAsync(T entity);

    /// <summary>
    /// Adds the specified range of entities to the repository.
    /// </summary>
    /// <param name="entities">The entities to add.</param>
    void AddRange(IEnumerable<T> entities);

    /// <summary>
    /// Asynchronously adds the specified range of entities to the repository.
    /// </summary>
    /// <param name="entities">The entities to add.</param>
    /// <returns>A task that represents the asynchronous add range operation.</returns>
    Task AddRangeAsync(IEnumerable<T> entities);

    /// <summary>
    /// Updates the specified entity in the repository.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    void Update(T entity);

    /// <summary>
    /// Asynchronously updates the specified entity in the repository.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>A task that represents the asynchronous update operation.</returns>
    Task UpdateAsync(T entity);

    /// <summary>
    /// Updates the specified range of entities in the repository.
    /// </summary>
    /// <param name="entities">The entities to update.</param>
    void UpdateRange(IEnumerable<T> entities);

    /// <summary>
    /// Asynchronously updates the specified range of entities in the repository.
    /// </summary>
    /// <param name="entities">The entities to update.</param>
    /// <returns>A task that represents the asynchronous update range operation.</returns>
    Task UpdateRangeAsync(IEnumerable<T> entities);

    /// <summary>
    /// Executes an update operation on entities that match the specified filter.
    /// </summary>
    /// <param name="setPropertyCalls">The expression specifying the update.</param>
    /// <param name="filter">The filter expression.</param>
    /// <returns>The number of affected entities.</returns>
    int ExecuteUpdate(Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls, Expression<Func<T, bool>>? filter = null);

    /// <summary>
    /// Asynchronously executes an update operation on entities that match the specified filter.
    /// </summary>
    /// <param name="setPropertyCalls">The expression specifying the update.</param>
    /// <param name="filter">The filter expression.</param>
    /// <returns>A task that represents the asynchronous update operation, containing the number of affected entities.</returns>
    Task<int> ExecuteUpdateAsync(Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls, Expression<Func<T, bool>>? filter = null);

    /// <summary>
    /// Deletes the specified entity from the repository.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    void Delete(T entity);

    /// <summary>
    /// Asynchronously deletes the specified entity from the repository.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteAsync(T entity);

    /// <summary>
    /// Deletes the specified range of entities from the repository.
    /// </summary>
    /// <param name="entities">The entities to delete.</param>
    void DeleteRange(IEnumerable<T> entities);

    /// <summary>
    /// Asynchronously deletes the specified range of entities from the repository.
    /// </summary>
    /// <param name="entities">The entities to delete.</param>
    /// <returns>A task that represents the asynchronous delete range operation.</returns>
    Task DeleteRangeAsync(IEnumerable<T> entities);

    /// <summary>
    /// Executes a delete operation on entities that match the specified filter.
    /// </summary>
    /// <param name="filter">The filter expression.</param>
    /// <returns>The number of affected entities.</returns>
    int ExecuteDelete(Expression<Func<T, bool>>? filter = null);

    /// <summary>
    /// Asynchronously executes a delete operation on entities that match the specified filter.
    /// </summary>
    /// <param name="filter">The filter expression.</param>
    /// <returns>A task that represents the asynchronous delete operation, containing the number of affected entities.</returns>
    Task<int> ExecuteDeleteAsync(Expression<Func<T, bool>>? filter = null);

    /// <summary>
    /// Determines whether all entities match the specified filter.
    /// </summary>
    /// <param name="filter">The filter expression.</param>
    /// <returns>True if all entities match the filter; otherwise, false.</returns>
    bool All(Expression<Func<T, bool>> filter);

    /// <summary>
    /// Asynchronously determines whether all entities match the specified filter.
    /// </summary>
    /// <param name="filter">The filter expression.</param>
    /// <returns>A task that represents the asynchronous operation, containing true if all entities match the filter; otherwise, false.</returns>
    Task<bool> AllAsync(Expression<Func<T, bool>> filter);

    /// <summary>
    /// Determines whether any entities match the specified filter.
    /// </summary>
    /// <param name="filter">The filter expression.</param>
    /// <returns>True if any entities match the filter; otherwise, false.</returns>
    bool Any(Expression<Func<T, bool>> filter);

    /// <summary>
    /// Asynchronously determines whether any entities match the specified filter.
    /// </summary>
    /// <param name="filter">The filter expression.</param>
    /// <returns>A task that represents the asynchronous operation, containing true if any entities match the filter; otherwise, false.</returns>
    Task<bool> AnyAsync(Expression<Func<T, bool>> filter);

    /* Aggregate Operations */

    /// <summary>
    /// Retrieves the maximum value of the specified expression.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="filter">The filter expression.</param>
    /// <returns>The maximum value.</returns>
    TResult? Max<TResult>(Expression<Func<T, TResult>> filter);

    /// <summary>
    /// Asynchronously retrieves the maximum value of the specified expression.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="filter">The filter expression.</param>
    /// <returns>A task that represents the asynchronous operation, containing the maximum value.</returns>
    /// 
    Task<TResult> MaxAsync<TResult>(Expression<Func<T, TResult>> filter);

    /// <summary>
    /// Retrieves the entity with the maximum value of the specified key selector function.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <param name="keyselect">The key selector function.</param>
    /// <param name="filter">The filter expression.</param>
    /// <param name="asNoTracking">Whether to track the entity or not.</param>
    /// <returns>The entity with the maximum value.</returns>
    T? MaxBy<TKey>(Func<T, TKey> keyselect, Expression<Func<T, bool>>? filter = null, bool? asNoTracking = false);

    /// <summary>
    /// Retrieves the minimum value of the specified expression.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="filter">The filter expression.</param>
    /// <returns>The minimum value.</returns>
    TResult? Min<TResult>(Expression<Func<T, TResult>> filter);

    /// <summary>
    /// Asynchronously retrieves the minimum value of the specified expression.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="filter">The filter expression.</param>
    /// <returns>A task that represents the asynchronous operation, containing the minimum value.</returns>
    Task<TResult> MinAsync<TResult>(Expression<Func<T, TResult>> filter);

    /// <summary>
    /// Retrieves the entity with the minimum value of the specified key selector function.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <param name="keyselect">The key selector function.</param>
    /// <param name="filter">The filter expression.</param>
    /// <param name="asNoTracking">Whether to track the entity or not.</param>
    /// <returns>The entity with the minimum value.</returns>
    T? MinBy<TKey>(Func<T, TKey> keyselect, Expression<Func<T, bool>>? filter = null, bool? asNoTracking = false);
}
public class EfEntityRepositoryBase<TEntity, TContext>(TContext context) : IEntityRepository<TEntity> where TEntity : class, IEntity, new() where TContext : DbContext, new()
{
    // count
    public int Count(Expression<Func<TEntity, bool>>? filter = null) => (filter is not null ? context.Set<TEntity>().Where(filter) : context.Set<TEntity>()).Count();
    public Task<int> CountAsync(Expression<Func<TEntity, bool>>? filter = null) => (filter is not null ? context.Set<TEntity>().Where(filter) : context.Set<TEntity>()).CountAsync();

    // get
    public TEntity? Get(Expression<Func<TEntity, bool>> filter, EntitySortModel<TEntity>? sort = null, bool? asNoTracking = false) =>
        Sort(filter, sort, asNoTracking).FirstOrDefault();
    public TResult? Get<TResult>(Expression<Func<TEntity, bool>> filter, Func<TEntity, TResult> select, EntitySortModel<TEntity>? sort = null, bool? asNoTracking = false) =>
        Select(select, filter, sort, asNoTracking).FirstOrDefault();
    public Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> filter, EntitySortModel<TEntity>? sort = null, bool? asNoTracking = false) =>
        Sort(filter, sort, asNoTracking).FirstOrDefaultAsync();

    // get all

    public List<TEntity> GetAll(Expression<Func<TEntity, bool>>? filter = null, EntitySortModel<TEntity>? sort = null, bool? asNoTracking = false) =>
         [.. Sort(filter, sort, asNoTracking)];
    public List<TEntity> GetAll<TResult>(Expression<Func<TEntity, bool>>? filter = null, EntitySortModel<TEntity>? sort = null, bool? asNoTracking = false, Func<TEntity, TResult>? distincby = null) =>
        [.. distincby is not null ? DistincBy(distincby, filter, sort, asNoTracking) : Sort(filter, sort, asNoTracking)];
    public Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null, EntitySortModel<TEntity>? sort = null, bool? asNoTracking = false) =>
        Sort(filter, sort, asNoTracking).ToListAsync();

    // get array
    public TEntity[] GetArray(Expression<Func<TEntity, bool>>? filter = null, EntitySortModel<TEntity>? sort = null, bool? asNoTracking = false) =>
        [.. Sort(filter, sort, asNoTracking)];
    public TEntity[] GetArray<TResult>(Expression<Func<TEntity, bool>>? filter = null, EntitySortModel<TEntity>? sort = null, bool? asNoTracking = false, Func<TEntity, TResult>? distincby = null) =>
        [.. distincby is not null ? DistincBy(distincby, filter, sort, asNoTracking) : Sort(filter, sort, asNoTracking)];
    public Task<TEntity[]> GetArrayAsync(Expression<Func<TEntity, bool>>? filter = null, EntitySortModel<TEntity>? sort = null, bool? asNoTracking = false) =>
        Sort(filter, sort, asNoTracking).ToArrayAsync();

    // paginate
    public Paginate<TEntity> GetListPaginate(int page, int size, Expression<Func<TEntity, bool>>? filter = null, EntitySortModel<TEntity>? sort = null, bool? asNoTracking = false)
        => Sort(filter, sort, asNoTracking).ToPaginate(page, size);
    public Paginate<TEntity> GetListPaginate<TResult>(int page, int size, Expression<Func<TEntity, bool>>? filter = null, EntitySortModel<TEntity>? sort = null, bool? asNoTracking = false, Func<TEntity, TResult>? distincby = null)
        => (distincby is not null ? DistincBy(distincby, filter, sort, asNoTracking) : Sort(filter, sort, asNoTracking)).ToPaginate(page, size);
    public Task<Paginate<TEntity>> GetListPaginateAsync(int page, int size, Expression<Func<TEntity, bool>>? filter = null, EntitySortModel<TEntity>? sort = null, bool? asNoTracking = false)
        => Sort(filter, sort, asNoTracking).ToPaginateAsync(page, size);

    // select
    public List<TResult> GetSelectList<TResult>(Func<TEntity, TResult> select, Expression<Func<TEntity, bool>>? filter = null, EntitySortModel<TEntity>? sort = null, bool? asNoTracking = false) =>
        [.. Select(select, filter, sort, asNoTracking)];
    public TResult[] GetSelectArray<TResult>(Func<TEntity, TResult> select, Expression<Func<TEntity, bool>>? filter = null, EntitySortModel<TEntity>? sort = null, bool? asNoTracking = false) =>
        [.. Select(select, filter, sort, asNoTracking)];
    public Paginate<TResult> GetSelectPaginateList<TResult>(int page, int size, Func<TEntity, TResult> select, Expression<Func<TEntity, bool>>? filter = null, EntitySortModel<TEntity>? sort = null, bool? asNoTracking = false)
        => Select(select, filter, sort, asNoTracking).ToPaginate(page, size);

    // get take
    public List<TEntity> GetTakeList(int count, EntitySortModel<TEntity> sort, Expression<Func<TEntity, bool>>? filter = null, bool? asNoTracking = false)
        => [.. Take(count, filter, sort, asNoTracking)];
    public TEntity[] GetTakeArray(int count, EntitySortModel<TEntity> sort, Expression<Func<TEntity, bool>>? filter = null, bool? asNoTracking = false)
        => [.. Take(count, filter, sort, asNoTracking)];

    // add
    public void Add(TEntity entity) { context.Entry(entity).State = EntityState.Added; context.SaveChanges(); }
    public Task AddAsync(TEntity entity) { context.Entry(entity).State = EntityState.Added; return context.SaveChangesAsync(); }

    // add range
    public void AddRange(IEnumerable<TEntity> entities) { context.AddRange(entities); context.SaveChanges(); }
    public Task AddRangeAsync(IEnumerable<TEntity> entities) => Task.WhenAll(context.AddRangeAsync(entities), context.SaveChangesAsync());

    // update
    public void Update(TEntity entity) { context.Entry(entity).State = EntityState.Modified; context.SaveChanges(); }
    public Task UpdateAsync(TEntity entity) { context.Entry(entity).State = EntityState.Modified; return context.SaveChangesAsync(); }

    // update range
    public void UpdateRange(IEnumerable<TEntity> entities) { context.UpdateRange(entities); context.SaveChanges(); }
    public Task UpdateRangeAsync(IEnumerable<TEntity> entities) { context.UpdateRange(entities); return context.SaveChangesAsync(); }

    // execute update
    public int ExecuteUpdate(Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls, Expression<Func<TEntity, bool>>? filter = null) =>
        filter is not null ? context.Set<TEntity>().Where(filter).ExecuteUpdate(setPropertyCalls) : context.Set<TEntity>().ExecuteUpdate(setPropertyCalls);
    public Task<int> ExecuteUpdateAsync(Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls, Expression<Func<TEntity, bool>>? filter = null) =>
       filter is not null ? context.Set<TEntity>().Where(filter).ExecuteUpdateAsync(setPropertyCalls) : context.Set<TEntity>().ExecuteUpdateAsync(setPropertyCalls);

    // delete
    public void Delete(TEntity entity) { context.Entry(entity).State = EntityState.Deleted; context.SaveChanges(); }
    public Task DeleteAsync(TEntity entity) { context.Entry(entity).State = EntityState.Deleted; return context.SaveChangesAsync(); }

    // delete range
    public void DeleteRange(IEnumerable<TEntity> entities) { context.RemoveRange(entities); context.SaveChanges(); }
    public Task DeleteRangeAsync(IEnumerable<TEntity> entities) { context.RemoveRange(entities); return context.SaveChangesAsync(); }

    // execute delete
    public int ExecuteDelete(Expression<Func<TEntity, bool>>? filter = null) => filter is not null ?
        context.Set<TEntity>().Where(filter).ExecuteDelete() : context.Set<TEntity>().ExecuteDelete();
    public Task<int> ExecuteDeleteAsync(Expression<Func<TEntity, bool>>? filter = null) => filter is not null ?
        context.Set<TEntity>().Where(filter).ExecuteDeleteAsync() : context.Set<TEntity>().ExecuteDeleteAsync();

    // all
    public bool All(Expression<Func<TEntity, bool>> filter) => context.Set<TEntity>().All(filter);
    public Task<bool> AllAsync(Expression<Func<TEntity, bool>> filter) => context.Set<TEntity>().AllAsync(filter);

    // any
    public bool Any(Expression<Func<TEntity, bool>> filter) => context.Set<TEntity>().Any(filter);
    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter) => context.Set<TEntity>().AnyAsync(filter);

    // max
    public TResult? Max<TResult>(Expression<Func<TEntity, TResult>> filter) => context.Set<TEntity>().Max(filter);
    public Task<TResult> MaxAsync<TResult>(Expression<Func<TEntity, TResult>> filter) => context.Set<TEntity>().MaxAsync(filter);
    public TEntity? MaxBy<TResult>(Func<TEntity, TResult> keyselect, Expression<Func<TEntity, bool>>? filter = null, bool? asNoTracking = false)
        => Filter(filter, asNoTracking).MaxBy(keyselect);

    // min
    public TResult? Min<TResult>(Expression<Func<TEntity, TResult>> filter) => context.Set<TEntity>().Min(filter);
    public Task<TResult> MinAsync<TResult>(Expression<Func<TEntity, TResult>> filter) => context.Set<TEntity>().MinAsync(filter);
    public TEntity? MinBy<TResult>(Func<TEntity, TResult> keyselect, Expression<Func<TEntity, bool>>? filter = null, bool? asNoTracking = false)
        => Filter(filter, asNoTracking).MinBy(keyselect);

    private IEnumerable<TEntity> Take(int count, Expression<Func<TEntity, bool>>? filter = null, EntitySortModel<TEntity>? sort = null, bool? asNoTracking = false)
        => Sort(filter, sort, asNoTracking).Take(count);
    private IEnumerable<TResult> Select<TResult>(Func<TEntity, TResult> select, Expression<Func<TEntity, bool>>? filter = null, EntitySortModel<TEntity>? sort = null, bool? asNoTracking = false)
        => Sort(filter, sort, asNoTracking).Select(select);
    private IEnumerable<TEntity> DistincBy<TResult>(Func<TEntity, TResult> distinc, Expression<Func<TEntity, bool>>? filter = null, EntitySortModel<TEntity>? sort = null, bool? asNoTracking = false)
        => Sort(filter, sort, asNoTracking).GroupBy(distinc).Select(x => x.First());
    private IQueryable<TEntity> Sort(Expression<Func<TEntity, bool>>? filter = null, EntitySortModel<TEntity>? sort = null, bool? asNoTracking = false)
    {
        IQueryable<TEntity> query = Filter(filter, asNoTracking);
        return sort != null ? sort.SortType == SortOrder.Ascending ? query.OrderBy(sort.Sort) : query.OrderByDescending(sort.Sort) : query;
    }
    private IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>>? filter = null, bool? asNoTracking = false)
    {
        IQueryable<TEntity> query = asNoTracking is true ? context.Set<TEntity>().AsNoTracking() : context.Set<TEntity>();
        return filter != null ? query.Where(filter) : query;
    }
}

public static class UEntityPaginateExtension
{
    /// <summary>
    /// Converts a collection of entities into a paginated list.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="source">The source collection to paginate.</param>
    /// <param name="index">The page index (zero-based).</param>
    /// <param name="size">The size of each page.</param>
    /// <param name="from">The starting page index (default is 0).</param>
    /// <returns>A Paginate<T> object containing the paginated entities.</returns>
    public static Paginate<T> ToPaginate<T>(this IEnumerable<T> source, int index, int size, int from = 0)
    {
        try
        {
            int count = source.Count(); // paginate harici datanın sayısı çeker
            var pages = (int)Math.Ceiling(count / (double)size);

            return new Paginate<T>()
            {
                Index = index,
                Size = size,
                From = from,
                Count = count,
                Items = [.. source.Skip((index - from) * size).Take(size)], // paginate datasını çeker
                Pages = pages,
                HasPrevious = (index - from) > 0,
                HasNext = (index - from + 1) < pages
            };
        }
        catch
        {
            Console.WriteLine($"ToPaginate() => entity type: {typeof(T)}");

            return new Paginate<T>();
        }
    }

    /// <summary>
    /// Asynchronously converts a queryable collection of entities into a paginated list.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="source">The source queryable collection to paginate.</param>
    /// <param name="index">The page index (zero-based).</param>
    /// <param name="size">The size of each page.</param>
    /// <param name="from">The starting page index (default is 0).</param>
    /// <returns>A Task<Paginate>T>> object containing the paginated entities.</returns>
    public static async Task<Paginate<T>> ToPaginateAsync<T>(this IQueryable<T> source, int index, int size, int from = 0)
    {
        try
        {
            int count = await source.CountAsync(); // paginate harici datanın sayısı çeker
            var pages = (int)Math.Ceiling(count / (double)size);

            var items = await source.Skip((index - from) * size).Take(size).ToListAsync();  // paginate datasını çeker

            return new Paginate<T>()
            {
                Index = index,
                Size = size,
                From = from,
                Count = count,
                Items = items,
                Pages = pages,
                HasPrevious = (index - from) > 0,
                HasNext = (index - from + 1) < pages
            };
        }
        catch
        {
            Console.WriteLine($"ToPaginateAsync() => entity type: {typeof(T)}");

            return new Paginate<T>();
        }
    }
}

// main entity interface
public interface IEntity { }

public record EntitySortModel<T>
{
    public Expression<Func<T, object>> Sort { get; set; } = null!;
    public SortOrder SortType { get; set; } = SortOrder.Ascending;
}

public class PageRequest
{
    public int Page { get; set; } = 0;
    public int PageSize { get; set; } = 20;
}

public class Paginate<T>
{
    public int From { get; set; } = 0;
    public int Index { get; set; } = 0;
    public int Size { get; set; } = 0;
    public int Count { get; set; } = 0;
    public int Pages { get; set; } = 0;
    public List<T> Items { get; set; } = [];
    public bool HasPrevious { get; set; } = false;
    public bool HasNext { get; set; } = false;
}