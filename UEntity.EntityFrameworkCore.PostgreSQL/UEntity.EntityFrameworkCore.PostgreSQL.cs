using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System.Linq.Expressions;

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
    Task<int> CountAsync(Expression<Func<T, bool>>? filter = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the first element of a sequence that satisfies a specified condition
    /// or a default value if no such element is found, using the specified sorting criteria.
    /// </summary>
    /// <param name="sort">An expression to sort the elements of the sequence.</param>
    /// <returns>The first element that satisfies the condition or default value.</returns>
    /// <typeparam name="T">The type of the elements of the sequence.</typeparam>
    T? FirstOrDefault(Expression<Func<T, object>> sort);

    /// <summary>
    /// Asynchronously returns the first element of a sequence that satisfies a specified condition
    /// or a default value if no such element is found, using the specified sorting criteria.
    /// </summary>
    /// <param name="sort">An expression to sort the elements of the sequence.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains 
    /// the first element that satisfies the condition or default value.</returns>
    /// <typeparam name="T">The type of the elements of the sequence.</typeparam>
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, object>> sort, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the last element of a sequence that satisfies a specified condition
    /// or a default value if no such element is found, using the specified sorting criteria.
    /// </summary>
    /// <param name="sort">An expression to sort the elements of the sequence.</param>
    /// <returns>The last element that satisfies the condition or default value.</returns>
    /// <typeparam name="T">The type of the elements of the sequence.</typeparam>
    T? LastOrDefault(Expression<Func<T, object>> sort);

    /// <summary>
    /// Asynchronously returns the last element of a sequence that satisfies a specified condition
    /// or a default value if no such element is found, using the specified sorting criteria.
    /// </summary>
    /// <param name="sort">An expression to sort the elements of the sequence.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains 
    /// the last element that satisfies the condition or default value.</returns>
    /// <typeparam name="T">The type of the elements of the sequence.</typeparam>
    Task<T?> LastOrDefaultAsync(Expression<Func<T, object>> sort, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a single entity that matches the specified filter, with optional sorting and tracking behavior.
    /// </summary>
    /// <param name="filter">The filter expression.</param>
    /// <param name="sort">The sorting model.</param>
    /// <param name="asNoTracking">Whether to track the entity or not.</param>
    /// <returns>The entity that matches the filter, or null if no entity is found.</returns>
    T? Get(Expression<Func<T, bool>> filter, EntitySortModel<T>? sort = null, bool? asNoTracking = false, params Expression<Func<T, object?>>[]? includes);

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
    Task<T?> GetAsync(Expression<Func<T, bool>> filter, EntitySortModel<T>? sort = null, bool? asNoTracking = false, CancellationToken cancellationToken = default,
        params Expression<Func<T, object?>>[]? includes);

    /// <summary>
    /// Retrieves all entities that match the specified filter, with optional sorting and tracking behavior.
    /// </summary>
    /// <param name="filter">The filter expression.</param>
    /// <param name="sort">The sorting model.</param>
    /// <param name="asNoTracking">Whether to track the entities or not.</param>
    /// <returns>A list of entities that match the filter.</returns>
    List<T> GetAll(Expression<Func<T, bool>>? filter = null, EntitySortModel<T>? sort = null, bool? asNoTracking = false,
        params Expression<Func<T, object?>>[]? includes);

    /// <summary>
    /// Retrieves all entities that match the specified filter and selects the distinct result, with optional sorting and tracking behavior.
    /// </summary>
    /// <typeparam name="TResult">The type of the distinct result.</typeparam>
    /// <param name="filter">The filter expression.</param>
    /// <param name="sort">The sorting model.</param>
    /// <param name="asNoTracking">Whether to track the entities or not.</param>
    /// <param name="distinctby">The distinct by function.</param>
    /// <returns>A list of distinct results of entities that match the filter.</returns>
    List<T> GetAll<TResult>(Expression<Func<T, bool>>? filter = null, EntitySortModel<T>? sort = null, bool? asNoTracking = false, Func<T, TResult>? distinctby = null);

    /// <summary>
    /// Asynchronously retrieves all entities that match the specified filter, with optional sorting and tracking behavior.
    /// </summary>
    /// <param name="filter">The filter expression.</param>
    /// <param name="sort">The sorting model.</param>
    /// <param name="asNoTracking">Whether to track the entities or not.</param>
    /// <returns>A task that represents the asynchronous operation, containing a list of entities that match the filter.</returns>
    Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, EntitySortModel<T>? sort = null, bool? asNoTracking = false,
        CancellationToken cancellationToken = default, params Expression<Func<T, object?>>[]? includes);

    /// <summary>
    /// Retrieves an array of entities that match the specified filter, with optional sorting and tracking behavior.
    /// </summary>
    /// <param name="filter">The filter expression.</param>
    /// <param name="sort">The sorting model.</param>
    /// <param name="asNoTracking">Whether to track the entities or not.</param>
    /// <returns>An array of entities that match the filter.</returns>
    T[] GetArray(Expression<Func<T, bool>>? filter = null, EntitySortModel<T>? sort = null, bool? asNoTracking = false,
        params Expression<Func<T, object?>>[]? includes);

    /// <summary>
    /// Retrieves an array of entities that match the specified filter and selects the distinct result, with optional sorting and tracking behavior.
    /// </summary>
    /// <typeparam name="TResult">The type of the distinct result.</typeparam>
    /// <param name="filter">The filter expression.</param>
    /// <param name="sort">The sorting model.</param>
    /// <param name="asNoTracking">Whether to track the entities or not.</param>
    /// <param name="distinctby">The distinct by function.</param>
    /// <returns>An array of distinct results of entities that match the filter.</returns>
    T[] GetArray<TResult>(Expression<Func<T, bool>>? filter = null, EntitySortModel<T>? sort = null, bool? asNoTracking = false, Func<T, TResult>? distinctby = null);

    /// <summary>
    /// Asynchronously retrieves an array of entities that match the specified filter, with optional sorting and tracking behavior.
    /// </summary>
    /// <param name="filter">The filter expression.</param>
    /// <param name="sort">The sorting model.</param>
    /// <param name="asNoTracking">Whether to track the entities or not.</param>
    /// <returns>A task that represents the asynchronous operation, containing an array of entities that match the filter.</returns>
    Task<T[]> GetArrayAsync(Expression<Func<T, bool>>? filter = null, EntitySortModel<T>? sort = null, bool? asNoTracking = false, CancellationToken cancellationToken = default,
        params Expression<Func<T, object?>>[]? includes);

    /// <summary>
    /// Retrieves a paginated list of entities that match the specified filter, with optional sorting and tracking behavior.
    /// </summary>
    /// <param name="page">The page number.</param>
    /// <param name="size">The size of the page.</param>
    /// <param name="filter">The filter expression.</param>
    /// <param name="sort">The sorting model.</param>
    /// <param name="asNoTracking">Whether to track the entities or not.</param>
    /// <returns>A paginated list of entities that match the filter.</returns>
    Paginate<T> GetListPaginate(int page, int size, Expression<Func<T, bool>>? filter = null, EntitySortModel<T>? sort = null, bool? asNoTracking = false,
        params Expression<Func<T, object?>>[]? includes);

    /// <summary>
    /// Retrieves a paginated list of distinct results for entities that match the specified filter, with optional sorting and tracking behavior.
    /// </summary>
    /// <typeparam name="TResult">The type of the distinct result.</typeparam>
    /// <param name="page">The page number.</param>
    /// <param name="size">The size of the page.</param>
    /// <param name="filter">The filter expression.</param>
    /// <param name="sort">The sorting model.</param>
    /// <param name="asNoTracking">Whether to track the entities or not.</param>
    /// <param name="distinctby">The distinct by function.</param>
    /// <returns>A paginated list of distinct results of entities that match the filter.</returns>
    Paginate<T> GetListPaginate<TResult>(int page, int size, Expression<Func<T, bool>>? filter = null, EntitySortModel<T>? sort = null,
        bool? asNoTracking = false, Func<T, TResult>? distinctby = null);

    /// <summary>
    /// Asynchronously retrieves a paginated list of entities that match the specified filter, with optional sorting and tracking behavior.
    /// </summary>
    /// <param name="page">The page number.</param>
    /// <param name="size">The size of the page.</param>
    /// <param name="filter">The filter expression.</param>
    /// <param name="sort">The sorting model.</param>
    /// <param name="asNoTracking">Whether to track the entities or not.</param>
    /// <returns>A task that represents the asynchronous operation, containing a paginated list of entities that match the filter.</returns>
    Task<Paginate<T>> GetListPaginateAsync(int page, int size, Expression<Func<T, bool>>? filter = null, EntitySortModel<T>? sort = null, bool? asNoTracking = false,
        params Expression<Func<T, object?>>[]? includes);

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

    // select many list
    List<TResult> GetSelectManyList<TResult>(Func<T, IEnumerable<TResult>> selectMany, Expression<Func<T, bool>>? filter = null, EntitySortModel<T>? sort = null, bool? asNoTracking = false, bool? distinct = null);
    TResult[] GetSelectManyArray<TResult>(Func<T, IEnumerable<TResult>> selectMany, Expression<Func<T, bool>>? filter = null, EntitySortModel<T>? sort = null, bool? asNoTracking = false, bool? distinct = null);

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
    Task AddAsync(T entity, CancellationToken cancellationToken = default);

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
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

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
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

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
    Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes an update operation on entities that match the specified filter.
    /// </summary>
    /// <param name="setPropertyCalls">The expression specifying the update.</param>
    /// <param name="filter">The filter expression.</param>
    /// <returns>The number of affected entities.</returns>
    int ExecuteUpdate(Action<UpdateSettersBuilder<T>> setPropertyCalls, Expression<Func<T, bool>>? filter = null);

    /// <summary>
    /// Asynchronously executes an update operation on entities that match the specified filter.
    /// </summary>
    /// <param name="setPropertyCalls">The expression specifying the update.</param>
    /// <param name="filter">The filter expression.</param>
    /// <returns>A task that represents the asynchronous update operation, containing the number of affected entities.</returns>
    Task<int> ExecuteUpdateAsync(Action<UpdateSettersBuilder<T>> setPropertyCalls,
        Expression<Func<T, bool>>? filter = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the specified entity from the repository.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    int Delete(T entity);

    /// <summary>
    /// Asynchronously deletes the specified entity from the repository.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task<int> DeleteAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the specified range of entities from the repository.
    /// </summary>
    /// <param name="entities">The entities to delete.</param>
    int DeleteRange(IEnumerable<T> entities);

    /// <summary>
    /// Asynchronously deletes the specified range of entities from the repository.
    /// </summary>
    /// <param name="entities">The entities to delete.</param>
    /// <returns>A task that represents the asynchronous delete range operation.</returns>
    Task<int> DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

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
    Task<int> ExecuteDeleteAsync(Expression<Func<T, bool>>? filter = null, CancellationToken cancellationToken = default);

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
    Task<bool> AllAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);

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
    Task<bool> AnyAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);

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
    Task<TResult> MaxAsync<TResult>(Expression<Func<T, TResult>> filter, CancellationToken cancellationToken = default);

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
    Task<TResult> MinAsync<TResult>(Expression<Func<T, TResult>> filter, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the entity with the minimum value of the specified key selector function.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <param name="keyselect">The key selector function.</param>
    /// <param name="filter">The filter expression.</param>
    /// <param name="asNoTracking">Whether to track the entity or not.</param>
    /// <returns>The entity with the minimum value.</returns>
    T? MinBy<TKey>(Func<T, TKey> keyselect, Expression<Func<T, bool>>? filter = null, bool? asNoTracking = false);

    /// <summary>
    /// Returns an IQueryable of entities that match the specified filter and sorting criteria.
    /// </summary>
    /// <param name="filter">The filter expression.</param>
    /// <param name="sort">The sorting criteria.</param>
    /// <param name="asNoTracking">Whether to track the entity or not.</param>
    /// <returns>An IQueryable of entities.</returns>
    IQueryable<T> AsQueryable(bool? asNoTracking = false);
}
public class EfEntityRepositoryBase<TEntity, TContext>(TContext context) : IEntityRepository<TEntity> where TEntity : class, IEntity, new() where TContext : DbContext, new()
{
    // count
    public int Count(Expression<Func<TEntity, bool>>? filter = null)
    {
        IQueryable<TEntity> query = context.Set<TEntity>();
        if (filter == null)
        {
            return query.AsNoTracking().Count();
        }
        else
        {
            return query.AsNoTracking().Where(filter).Count();
        }
    }
    public Task<int> CountAsync(Expression<Func<TEntity, bool>>? filter = null, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = context.Set<TEntity>();
        if (filter == null)
        {
            return query.AsNoTracking().CountAsync(cancellationToken);
        }
        else
        {
            return query.AsNoTracking().Where(filter).CountAsync(cancellationToken);
        }
    }

    // first 
    public TEntity? FirstOrDefault(Expression<Func<TEntity, object>> sort)
    {
        // todo - sort - asnotracking
        return context.Set<TEntity>().OrderBy(sort).FirstOrDefault();
    }
    public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, object>> sort, CancellationToken cancellationToken = default)
    {
        return context.Set<TEntity>().OrderBy(sort).FirstOrDefaultAsync(cancellationToken);
    }

    // last
    public TEntity? LastOrDefault(Expression<Func<TEntity, object>> sort)
    {
        return context.Set<TEntity>().OrderByDescending(sort).FirstOrDefault();
    }
    public Task<TEntity?> LastOrDefaultAsync(Expression<Func<TEntity, object>> sort, CancellationToken cancellationToken = default)
    {
        return context.Set<TEntity>().OrderByDescending(sort).FirstOrDefaultAsync(cancellationToken);
    }

    // get
    public TEntity? Get(Expression<Func<TEntity, bool>> filter, EntitySortModel<TEntity>? sort = null, bool? asNoTracking = false,
        params Expression<Func<TEntity, object?>>[]? includes)
    {
        IQueryable<TEntity>? query = Sort(filter, sort, asNoTracking);
        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }
        return query.FirstOrDefault();
    }
    public TResult? Get<TResult>(Expression<Func<TEntity, bool>> filter, Func<TEntity, 
        TResult> select, EntitySortModel<TEntity>? sort = null, bool? asNoTracking = false)
    {
        return Select(select, filter, sort, asNoTracking).FirstOrDefault();
    }
    public Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> filter, 
        EntitySortModel<TEntity>? sort = null, bool? asNoTracking = false, CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object?>>[]? includes)
    {
        IQueryable<TEntity>? query = Sort(filter, sort, asNoTracking);
        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }
        return query.FirstOrDefaultAsync(cancellationToken);
    }

    // get all

    public List<TEntity> GetAll(Expression<Func<TEntity, bool>>? filter = null, EntitySortModel<TEntity>? sort = null,
        bool? asNoTracking = false, params Expression<Func<TEntity, object?>>[]? includes)
    {
        IQueryable<TEntity>? query = Sort(filter, sort, asNoTracking);
        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }
        return [.. query];
    }
    public List<TEntity> GetAll<TResult>(Expression<Func<TEntity, bool>>? filter = null, EntitySortModel<TEntity>? sort = null,
        bool? asNoTracking = false, Func<TEntity, TResult>? distinctby = null)
    {
        return [.. distinctby is not null ? DistincBy(distinctby, filter, sort, asNoTracking) : Sort(filter, sort, asNoTracking)];
    }
    public Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null, EntitySortModel<TEntity>? sort = null,
        bool? asNoTracking = false, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object?>>[]? includes)
    {
        IQueryable<TEntity>? query = Sort(filter, sort, asNoTracking);
        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }
        return query.ToListAsync(cancellationToken);
    }

    // get array
    public TEntity[] GetArray(Expression<Func<TEntity, bool>>? filter = null, EntitySortModel<TEntity>? sort = null, bool? asNoTracking = false,
        params Expression<Func<TEntity, object?>>[]? includes)
    {
        IQueryable<TEntity>? query = Sort(filter, sort, asNoTracking);
        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }
        return [.. query];
    }
    public TEntity[] GetArray<TResult>(Expression<Func<TEntity, bool>>? filter = null, EntitySortModel<TEntity>? sort = null, bool? asNoTracking = false, Func<TEntity, TResult>? distinctby = null)
    {
        return [.. distinctby is not null ? DistincBy(distinctby, filter, sort, asNoTracking) : Sort(filter, sort, asNoTracking)];
    }
    public Task<TEntity[]> GetArrayAsync(Expression<Func<TEntity, bool>>? filter = null, EntitySortModel<TEntity>? sort = null, bool? asNoTracking = false,
        CancellationToken cancellationToken = default, params Expression<Func<TEntity, object?>>[]? includes)
    {
        IQueryable<TEntity>? query = Sort(filter, sort, asNoTracking);
        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }
        return query.ToArrayAsync(cancellationToken);
    }

    // paginate
    public Paginate<TEntity> GetListPaginate(int page, int size, Expression<Func<TEntity, bool>>? filter = null, EntitySortModel<TEntity>? sort = null, bool? asNoTracking = false,
        params Expression<Func<TEntity, object?>>[]? includes)
    {
        IQueryable<TEntity>? query = Sort(filter, sort, asNoTracking);
        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }
        return query.ToPaginate(page, size);
    }
    public Paginate<TEntity> GetListPaginate<TResult>(int page, int size, Expression<Func<TEntity, bool>>? filter = null, EntitySortModel<TEntity>? sort = null, bool? asNoTracking = false, Func<TEntity, TResult>? distinctby = null)
    {
        return (distinctby is not null ? DistincBy(distinctby, filter, sort, asNoTracking) : Sort(filter, sort, asNoTracking)).AsQueryable().ToPaginateAsync(page, size).GetAwaiter().GetResult();
    }
    public Task<Paginate<TEntity>> GetListPaginateAsync(int page, int size, Expression<Func<TEntity, bool>>? filter = null, EntitySortModel<TEntity>? sort = null, bool? asNoTracking = false,
        params Expression<Func<TEntity, object?>>[]? includes)
    {
        IQueryable<TEntity>? query = Sort(filter, sort, asNoTracking);
        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }
        return query.ToPaginateAsync(page, size);
    }

    // select
    public List<TResult> GetSelectList<TResult>(Func<TEntity, TResult> select, Expression<Func<TEntity, bool>>? filter = null, EntitySortModel<TEntity>? sort = null, bool? asNoTracking = false) =>
        [.. Select(select, filter, sort, asNoTracking)];
    public TResult[] GetSelectArray<TResult>(Func<TEntity, TResult> select, Expression<Func<TEntity, bool>>? filter = null, EntitySortModel<TEntity>? sort = null, bool? asNoTracking = false) =>
        [.. Select(select, filter, sort, asNoTracking)];
    public Paginate<TResult> GetSelectPaginateList<TResult>(int page, int size, Func<TEntity, TResult> select, Expression<Func<TEntity, bool>>? filter = null, EntitySortModel<TEntity>? sort = null, bool? asNoTracking = false)
        => Select(select, filter, sort, asNoTracking).ToPaginate(page, size);

    // select many
    public List<TResult> GetSelectManyList<TResult>(Func<TEntity, IEnumerable<TResult>> selectMany, Expression<Func<TEntity, bool>>? filter = null, EntitySortModel<TEntity>? sort = null, bool? asNoTracking = false,
        bool? distinct = null) => [.. Sort(filter, sort, asNoTracking).SelectMany(selectMany).DistinctIf(distinct ?? false)];
    public TResult[] GetSelectManyArray<TResult>(Func<TEntity, IEnumerable<TResult>> selectMany, Expression<Func<TEntity, bool>>? filter = null, EntitySortModel<TEntity>? sort = null, bool? asNoTracking = false,
        bool? distinct = null) => [.. Sort(filter, sort, asNoTracking).SelectMany(selectMany).DistinctIf(distinct ?? false)];

    // get take
    public List<TEntity> GetTakeList(int count, EntitySortModel<TEntity> sort, Expression<Func<TEntity, bool>>? filter = null, bool? asNoTracking = false)
        => [.. Take(count, filter, sort, asNoTracking)];
    public TEntity[] GetTakeArray(int count, EntitySortModel<TEntity> sort, Expression<Func<TEntity, bool>>? filter = null, bool? asNoTracking = false)
        => [.. Take(count, filter, sort, asNoTracking)];

    // add
    public void Add(TEntity entity) { context.Entry(entity).State = EntityState.Added; context.SaveChanges(); }
    public Task AddAsync(TEntity entity, CancellationToken cancellationToken = default) { context.Entry(entity).State = EntityState.Added; return context.SaveChangesAsync(cancellationToken); }

    // add range
    public void AddRange(IEnumerable<TEntity> entities) { context.AddRange(entities); context.SaveChanges(); }
    public Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) =>
        Task.WhenAll(context.AddRangeAsync(entities, cancellationToken), context.SaveChangesAsync(cancellationToken));

    // update
    public void Update(TEntity entity) { context.Entry(entity).State = EntityState.Modified; context.SaveChanges(); }
    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default) { context.Entry(entity).State = EntityState.Modified; return context.SaveChangesAsync(cancellationToken); }

    // update range
    public void UpdateRange(IEnumerable<TEntity> entities) { context.UpdateRange(entities); context.SaveChanges(); }
    public Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) { context.UpdateRange(entities); return context.SaveChangesAsync(cancellationToken); }

    // Action<UpdateSettersBuilder<TEntity>>

    // execute update
    public int ExecuteUpdate(Action<UpdateSettersBuilder<TEntity>> setPropertyCalls, Expression<Func<TEntity, bool>>? filter = null)
    {
        IQueryable<TEntity> query = context.Set<TEntity>();
        if (filter == null)
        {
            return query.ExecuteUpdate(setPropertyCalls);
        }
        else
        {
            return query.Where(filter).ExecuteUpdate(setPropertyCalls);
        }
    }
    public Task<int> ExecuteUpdateAsync(Action<UpdateSettersBuilder<TEntity>> setPropertyCalls, Expression<Func<TEntity, bool>>? filter = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = context.Set<TEntity>();
        if (filter == null)
        {
            return query.ExecuteUpdateAsync(setPropertyCalls, cancellationToken: cancellationToken);
        }
        else
        {
            return query.Where(filter).ExecuteUpdateAsync(setPropertyCalls, cancellationToken: cancellationToken);
        }
    }

    // delete
    public int Delete(TEntity entity)
    {
        context.Entry(entity).State = EntityState.Deleted;
        return context.SaveChanges();
    }
    public Task<int> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        context.Entry(entity).State = EntityState.Deleted;
        return context.SaveChangesAsync(cancellationToken);
    }

    // delete range
    public int DeleteRange(IEnumerable<TEntity> entities)
    {
        context.RemoveRange(entities);
        return context.SaveChanges();
    }
    public Task<int> DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        context.RemoveRange(entities);
        return context.SaveChangesAsync(cancellationToken);
    }

    // execute delete
    public int ExecuteDelete(Expression<Func<TEntity, bool>>? filter = null)
    {
        IQueryable<TEntity> query = context.Set<TEntity>();
        if (filter == null)
        {
            return query.ExecuteDelete();
        }
        else
        {
            return query.Where(filter).ExecuteDelete();
        }
    }
    public Task<int> ExecuteDeleteAsync(Expression<Func<TEntity, bool>>? filter = null, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = context.Set<TEntity>();
        if (filter == null)
        {
            return query.ExecuteDeleteAsync(cancellationToken);
        }
        else
        {
            return query.Where(filter).ExecuteDeleteAsync(cancellationToken);
        }
    }

    // all
    public bool All(Expression<Func<TEntity, bool>> filter)
    {
        return context.Set<TEntity>().All(filter);
    }
    public Task<bool> AllAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default)
    {
        return context.Set<TEntity>().AllAsync(filter, cancellationToken);
    }

    // any
    public bool Any(Expression<Func<TEntity, bool>> filter)
    {
        return context.Set<TEntity>().Any(filter);
    }
    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default)
    {
        return context.Set<TEntity>().AnyAsync(filter, cancellationToken);
    }

    // max
    public TResult? Max<TResult>(Expression<Func<TEntity, TResult>> filter)
    {
        return context.Set<TEntity>().Max(filter);
    }
    public Task<TResult> MaxAsync<TResult>(Expression<Func<TEntity, TResult>> filter, CancellationToken cancellationToken = default)
    {
        return context.Set<TEntity>().MaxAsync(filter, cancellationToken);
    }
    public TEntity? MaxBy<TResult>(Func<TEntity, TResult> keyselect, Expression<Func<TEntity, bool>>? filter = null, bool? asNoTracking = false)
    {
        return Filter(filter, asNoTracking).MaxBy(keyselect);
    }

    // min
    public TResult? Min<TResult>(Expression<Func<TEntity, TResult>> filter)
    {
        return context.Set<TEntity>().Min(filter);
    }
    public Task<TResult> MinAsync<TResult>(Expression<Func<TEntity, TResult>> filter, CancellationToken cancellationToken = default)
    {
        return context.Set<TEntity>().MinAsync(filter, cancellationToken);
    }
    public TEntity? MinBy<TResult>(Func<TEntity, TResult> keyselect, Expression<Func<TEntity, bool>>? filter = null, bool? asNoTracking = false)
    {
        return Filter(filter, asNoTracking).MinBy(keyselect);
    }

    public IQueryable<TEntity> AsQueryable(bool? asNoTracking = false)
    {
        IQueryable<TEntity> query = context.Set<TEntity>();
        return asNoTracking == true ? query.AsNoTracking() : query;
    }

    private IEnumerable<TEntity> Take(int count, Expression<Func<TEntity, bool>>? filter = null,
        EntitySortModel<TEntity>? sort = null, bool? asNoTracking = false)
    {
        return Sort(filter, sort, asNoTracking).Take(count);
    }
    private IEnumerable<TResult> Select<TResult>(Func<TEntity, TResult> select,
        Expression<Func<TEntity, bool>>? filter = null, EntitySortModel<TEntity>? sort = null, bool? asNoTracking = false)
    {
        return Sort(filter, sort, asNoTracking).Select(select);
    }
    private IEnumerable<TEntity> DistincBy<TResult>(Func<TEntity, TResult> distinc,
        Expression<Func<TEntity, bool>>? filter = null, EntitySortModel<TEntity>? sort = null, bool? asNoTracking = false)
    {
        return Sort(filter, sort, asNoTracking).GroupBy(distinc).Select(x => x.First());
    }
    private IQueryable<TEntity> Sort(Expression<Func<TEntity, bool>>? filter = null,
        EntitySortModel<TEntity>? sort = null, bool? asNoTracking = false)
    {
        IQueryable<TEntity> query = Filter(filter, asNoTracking);
        if (sort == null)
        {
            return query;
        }

        if (sort.SortType == SortOrder.Ascending)
        {
            return query.OrderBy(sort.Sort);
        }
        else
        {
            return query.OrderByDescending(sort.Sort);
        }
    }
    private IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>>? filter = null, bool? asNoTracking = false)
    {
        IQueryable<TEntity> query = context.Set<TEntity>();
        if (asNoTracking == true)
        {
            query = query.AsNoTracking();
        }

        if (filter == null)
        {
            return query;
        }
        else
        {
            return query.Where(filter);
        }
    }
}

public static class UEntityExtensions
{
    /// <summary>
    /// Asynchronously converts a queryable collection of entities into a paginated list.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="source">The source queryable collection to paginate.</param>
    /// <param name="index">The page index (zero-based).</param>
    /// <param name="size">The size of each page.</param>
    /// <param name="from">The starting page index (default is 0).</param>
    /// <returns>A Task<Paginate>T>> object containing the paginated entities.</returns>
    public static async Task<Paginate<T>> ToPaginateAsync<T>(this IQueryable<T> source, int page, int size)
    {
        page = page < 1 ? 1 : page;
        size = size <= 0 ? 5 : size;
        try
        {
            int total_count = await source.CountAsync(); // paginate harici datanın sayısı çeker
            var items = await source.Skip((page - 1) * size).Take(size).ToListAsync();  // paginate datasını çeker
            var pages_count = (int)Math.Ceiling(total_count / (double)size);
            return new Paginate<T>()
            {
                Page = page,
                Size = size,
                TotalCount = total_count,
                Items = items,
                PagesCount = pages_count,
                HasPrevious = page > 1,
                HasNext = page < pages_count
            };
        }
        catch
        {
            Console.WriteLine($"ToPaginateAsync() => entity type: {typeof(T)}");

            return new Paginate<T>();
        }
    }

    public static Paginate<T> ToPaginate<T>(this IQueryable<T> source, int page, int size)
    {
        page = page < 1 ? 1 : page;
        size = size <= 0 ? 5 : size;
        try
        {
            int total_count = source.Count(); // paginate harici datanın sayısı çeker
            var items = source.Skip((page - 1) * size).Take(size).ToList();  // paginate datasını çeker
            var pages_count = (int)Math.Ceiling(total_count / (double)size);
            return new Paginate<T>()
            {
                Page = page,
                Size = size,
                TotalCount = total_count,
                Items = items,
                PagesCount = pages_count,
                HasPrevious = page > 1,
                HasNext = page < pages_count
            };
        }
        catch
        {
            Console.WriteLine($"ToPaginate() => entity type: {typeof(T)}");

            return new Paginate<T>();
        }
    }
    public static Paginate<T> ToPaginate<T>(this IEnumerable<T> source, int page, int size)
    {
        page = page < 1 ? 1 : page;
        size = size <= 0 ? 5 : size;
        try
        {
            int total_count = source.Count();
            var items = source.Skip((page - 1) * size).Take(size).ToList();
            var pages_count = (int)Math.Ceiling(total_count / (double)size);
            return new Paginate<T>()
            {
                Page = page,
                Size = size,
                TotalCount = total_count,
                Items = items,
                PagesCount = pages_count,
                HasPrevious = page > 1,
                HasNext = page < pages_count
            };
        }
        catch
        {
            Console.WriteLine($"ToPaginate() => entity type: {typeof(T)}");

            return new Paginate<T>();
        }
    }
    public static IEnumerable<T> DistinctIf<T>(this IEnumerable<T> source, bool condition)
    {
        return condition ? source.Distinct() : source;
    }

    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> query, Expression<Func<T, bool>> predicate)
    {
        ArgumentNullException.ThrowIfNull(query);
        ArgumentNullException.ThrowIfNull(predicate);

        var parameter = Expression.Parameter(typeof(T));
        var combinedBody = Expression.AndAlso(Expression.Invoke(query, parameter), Expression.Invoke(predicate, parameter));
        return Expression.Lambda<Func<T, bool>>(combinedBody, parameter);
    }

    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> query, Expression<Func<T, bool>> predicate)
    {
        ArgumentNullException.ThrowIfNull(query);
        ArgumentNullException.ThrowIfNull(predicate);

        var parameter = Expression.Parameter(typeof(T));
        var combinedBody = Expression.OrElse(Expression.Invoke(query, parameter), Expression.Invoke(predicate, parameter));
        return Expression.Lambda<Func<T, bool>>(combinedBody, parameter);
    }

    public static Paginate<TDestination> ConvertItems<TSource, TDestination>(this Paginate<TSource> source, List<TDestination> items)
    {
        return new Paginate<TDestination>
        {
            TotalCount = source.TotalCount,
            HasNext = source.HasNext,
            HasPrevious = source.HasPrevious,
            Page = source.Page,
            PagesCount = source.PagesCount,
            Size = source.Size,
            Items = items
        };
    }
}

public static class PredicateBuilder
{
    public static Expression<Func<T, bool>> NewQuery<T>(bool @is) => x => @is;
    public static Expression<Func<T, bool>> NewQuery<T>(Expression<Func<T, bool>> predicate) => predicate;
}

// main entity interface
public interface IEntity { }

public record EntitySortModel<T>
{
    public Expression<Func<T, object?>> Sort { get; set; } = null!;
    public SortOrder SortType { get; set; } = SortOrder.Ascending;
}

public record PageRequest
{
    public int Page { get; set; }
    public int Size { get; set; }
}

public record Paginate<T>
{
    public int Page { get; set; }
    public int Size { get; set; }
    public long TotalCount { get; set; }
    public long PagesCount { get; set; }
    public bool HasPrevious { get; set; }
    public bool HasNext { get; set; }
    public List<T> Items { get; set; } = null!;
}