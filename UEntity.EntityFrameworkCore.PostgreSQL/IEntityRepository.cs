using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace UEntity.EntityFrameworkCore.PostgreSQL;

/// <summary>
/// Defines a repository interface for entities of type T.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public interface IEntityRepository<T> where T : class, IEntity, new()
{
    IQueryable<T> Query(
        Expression<Func<T, bool>> filter,
        EntitySortModel<T>? sort = null,
        bool? asNoTracking = false);

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
    T? FirstOrDefault(Expression<Func<T, object>> sort, bool? asNoTracking = false);

    /// <summary>
    /// Asynchronously returns the first element of a sequence that satisfies a specified condition
    /// or a default value if no such element is found, using the specified sorting criteria.
    /// </summary>
    /// <param name="sort">An expression to sort the elements of the sequence.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains 
    /// the first element that satisfies the condition or default value.</returns>
    /// <typeparam name="T">The type of the elements of the sequence.</typeparam>
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, object>> sort, CancellationToken cancellationToken = default, bool? asNoTracking = false);

    /// <summary>
    /// Returns the last element of a sequence that satisfies a specified condition
    /// or a default value if no such element is found, using the specified sorting criteria.
    /// </summary>
    /// <param name="sort">An expression to sort the elements of the sequence.</param>
    /// <returns>The last element that satisfies the condition or default value.</returns>
    /// <typeparam name="T">The type of the elements of the sequence.</typeparam>
    T? LastOrDefault(Expression<Func<T, object>> sort, bool? asNoTracking = false);

    /// <summary>
    /// Asynchronously returns the last element of a sequence that satisfies a specified condition
    /// or a default value if no such element is found, using the specified sorting criteria.
    /// </summary>
    /// <param name="sort">An expression to sort the elements of the sequence.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains 
    /// the last element that satisfies the condition or default value.</returns>
    /// <typeparam name="T">The type of the elements of the sequence.</typeparam>
    Task<T?> LastOrDefaultAsync(Expression<Func<T, object>> sort, CancellationToken cancellationToken = default, bool? asNoTracking = false);

    /// <summary>
    /// Retrieves a single entity that matches the specified filter, with optional sorting and tracking behavior.
    /// </summary>
    /// <param name="filter">The filter expression.</param>
    /// <param name="sort">The sorting model.</param>
    /// <param name="asNoTracking">Whether to track the entity or not.</param>
    /// <returns>The entity that matches the filter, or null if no entity is found.</returns>
    T? Get(
        Expression<Func<T, bool>> filter,
        EntitySortModel<T>? sort = null,
        bool? asNoTracking = false,
        bool? asSplitQuery = false,
        params Expression<Func<T, object?>>[]? includes);

    /// <summary>
    /// Retrieves a single entity that matches the specified filter and selects the specified result, with optional sorting and tracking behavior.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="filter">The filter expression.</param>
    /// <param name="select">The selection function.</param>
    /// <param name="sort">The sorting model.</param>
    /// <param name="asNoTracking">Whether to track the entity or not.</param>
    /// <returns>The selected result of the entity that matches the filter, or null if no entity is found.</returns>
    TResult? Select<TResult>(
            Expression<Func<T, TResult>> select, // 👈 Yeni Projeksiyon Parametresi
            Expression<Func<T, bool>> filter,
            EntitySortModel<T>? sort = null,
            bool? asNoTracking = false);

    TResult? SelectAs<TResult>(
            Expression<Func<T, bool>> filter,
            EntitySortModel<T>? sort = null,
            bool? asNoTracking = false) where TResult : new();

    /// <summary>
    /// Asynchronously retrieves a single entity that matches the specified filter, with optional sorting and tracking behavior.
    /// </summary>
    /// <param name="filter">The filter expression.</param>
    /// <param name="sort">The sorting model.</param>
    /// <param name="asNoTracking">Whether to track the entity or not.</param>
    /// <returns>A task that represents the asynchronous operation, containing the entity that matches the filter, or null if no entity is found.</returns>
    Task<T?> GetAsync(
        Expression<Func<T, bool>> filter,
        EntitySortModel<T>? sort = null,
        bool? asNoTracking = false,
        bool? asSplitQuery = false,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object?>>[]? includes);

    /// <summary>
    /// Retrieves a single entity that matches the specified filter and selects the specified result, with optional sorting and tracking behavior.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="filter">The filter expression.</param>
    /// <param name="select">The selection function.</param>
    /// <param name="sort">The sorting model.</param>
    /// <param name="asNoTracking">Whether to track the entity or not.</param>
    /// <returns>The selected result of the entity that matches the filter, or null if no entity is found.</returns>
    Task<TResult?> SelectAsync<TResult>(
            Expression<Func<T, TResult>> select, // 👈 Yeni Projeksiyon Parametresi
            Expression<Func<T, bool>> filter,
            EntitySortModel<T>? sort = null,
            bool? asNoTracking = false,
            CancellationToken cancellationToken = default);

    Task<TResult?> SelectAsAsync<TResult>(
            Expression<Func<T, bool>> filter,
            EntitySortModel<T>? sort = null,
            bool? asNoTracking = false,
            CancellationToken cancellationToken = default)
            where TResult : new();

    /// <summary>
    /// Retrieves all entities that match the specified filter, with optional sorting and tracking behavior.
    /// </summary>
    /// <param name="filter">The filter expression.</param>
    /// <param name="sort">The sorting model.</param>
    /// <param name="asNoTracking">Whether to track the entities or not.</param>
    /// <returns>A list of entities that match the filter.</returns>
    List<T> GetAll(
        Expression<Func<T, bool>>? filter = null,
        EntitySortModel<T>? sort = null,
        bool? asNoTracking = false,
        bool? asSplitQuery = false,
        params Expression<Func<T, object?>>[]? includes);

    List<TResult> SelectAll<TResult>(
            Expression<Func<T, TResult>> select,
            Expression<Func<T, bool>>? filter = null,
            EntitySortModel<T>? sort = null,
            bool? asNoTracking = false);

    Paginate<TResult> SelectPaginate<TResult>(
            int page, int size,
            Expression<Func<T, TResult>> select,
            Expression<Func<T, bool>>? filter = null,
            EntitySortModel<T>? sort = null,
            bool? asNoTracking = false);

    List<TResult> SelectAsAll<TResult>(
            Expression<Func<T, bool>>? filter = null,
            EntitySortModel<T>? sort = null,
            bool? asNoTracking = false)
            where TResult : new();

    Paginate<TResult> SelectAsPaginate<TResult>(
            int page, int size,
            Expression<Func<T, bool>>? filter = null,
            EntitySortModel<T>? sort = null,
            bool? asNoTracking = false)
            where TResult : new();

    /// <summary>
    /// Asynchronously retrieves all entities that match the specified filter, with optional sorting and tracking behavior.
    /// </summary>
    /// <param name="filter">The filter expression.</param>
    /// <param name="sort">The sorting model.</param>
    /// <param name="asNoTracking">Whether to track the entities or not.</param>
    /// <returns>A task that represents the asynchronous operation, containing a list of entities that match the filter.</returns>
    Task<List<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            EntitySortModel<T>? sort = null,
            bool? asNoTracking = false,
            bool? asSplitQuery = false,
            CancellationToken cancellationToken = default,
            params Expression<Func<T, object?>>[]? includes);

    Task<List<TResult>> SelectAllAsync<TResult>(
            Expression<Func<T, TResult>> select,
            Expression<Func<T, bool>>? filter = null,
            EntitySortModel<T>? sort = null,
            bool? asNoTracking = false,
            CancellationToken cancellationToken = default);

    Task<Paginate<TResult>> SelectPaginateAsync<TResult>(
            int page, int size,
            Expression<Func<T, TResult>> select,
            Expression<Func<T, bool>>? filter = null,
            EntitySortModel<T>? sort = null,
            bool? asNoTracking = false);

    Task<List<TResult>> SelectAsAllAsync<TResult>(
            Expression<Func<T, bool>>? filter = null,
            EntitySortModel<T>? sort = null,
            bool? asNoTracking = false,
            CancellationToken cancellationToken = default)
            where TResult : new();

    Task<Paginate<TResult>> SelectAsPaginateAsync<TResult>(
            int page, int size,
            Expression<Func<T, bool>>? filter = null,
            EntitySortModel<T>? sort = null,
            bool? asNoTracking = false)
            where TResult : new();

    // ARRAY
    public T[] GetArray(
        Expression<Func<T, bool>>? filter = null,
        EntitySortModel<T>? sort = null,
        bool? asNoTracking = false,
        bool? asSplitQuery = false,
        params Expression<Func<T, object?>>[]? includes);

    // get select array
    public TResult[] SelectArray<TResult>(
        Expression<Func<T, TResult>> select,
        Expression<Func<T, bool>>? filter = null,
        EntitySortModel<T>? sort = null,
        bool? asNoTracking = false);

    // get all async
    public Task<T[]> GetArrayAsync(
        Expression<Func<T, bool>>? filter = null,
        EntitySortModel<T>? sort = null,
        bool? asNoTracking = false,
        bool? asSplitQuery = false,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object?>>[]? includes);

    // get async select all
    public Task<TResult[]> SelectArrayAsync<TResult>(
        Expression<Func<T, TResult>> select,
        Expression<Func<T, bool>>? filter = null,
        EntitySortModel<T>? sort = null,
        bool? asNoTracking = false);


    /// <summary>
    /// Retrieves a paginated list of entities that match the specified filter, with optional sorting and tracking behavior.
    /// </summary>
    /// <param name="page">The page number.</param>
    /// <param name="size">The size of the page.</param>
    /// <param name="filter">The filter expression.</param>
    /// <param name="sort">The sorting model.</param>
    /// <param name="asNoTracking">Whether to track the entities or not.</param>
    /// <returns>A paginated list of entities that match the filter.</returns>
    Paginate<T> GetListPaginate(
        int page, int size, 
        Expression<Func<T, bool>>? filter = null, 
        EntitySortModel<T>? sort = null, 
        bool? asNoTracking = false,
        bool? asSplitQuery = false,
        params Expression<Func<T, object?>>[]? includes);

    /// <summary>
    /// Asynchronously retrieves a paginated list of entities that match the specified filter, with optional sorting and tracking behavior.
    /// </summary>
    /// <param name="page">The page number.</param>
    /// <param name="size">The size of the page.</param>
    /// <param name="filter">The filter expression.</param>
    /// <param name="sort">The sorting model.</param>
    /// <param name="asNoTracking">Whether to track the entities or not.</param>
    /// <returns>A task that represents the asynchronous operation, containing a paginated list of entities that match the filter.</returns>
    Task<Paginate<T>> GetListPaginateAsync(
        int page, int size, 
        Expression<Func<T, bool>>? filter = null, 
        EntitySortModel<T>? sort = null, 
        bool? asNoTracking = false,
        bool? asSplitQuery = false,
        params Expression<Func<T, object?>>[]? includes);

    /// <summary>
    /// Adds the specified entity to the repository.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    int Add(T entity);

    /// <summary>
    /// Asynchronously adds the specified entity to the repository.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>A task that represents the asynchronous add operation.</returns>
    Task<int> AddAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds the specified range of entities to the repository.
    /// </summary>
    /// <param name="entities">The entities to add.</param>
    int AddRange(IEnumerable<T> entities);

    /// <summary>
    /// Asynchronously adds the specified range of entities to the repository.
    /// </summary>
    /// <param name="entities">The entities to add.</param>
    /// <returns>A task that represents the asynchronous add range operation.</returns>
    Task<int> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the specified entity in the repository.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    int Update(T entity);

    /// <summary>
    /// Asynchronously updates the specified entity in the repository.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>A task that represents the asynchronous update operation.</returns>
    Task<int> UpdateAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the specified range of entities in the repository.
    /// </summary>
    /// <param name="entities">The entities to update.</param>
    int UpdateRange(IEnumerable<T> entities);

    /// <summary>
    /// Asynchronously updates the specified range of entities in the repository.
    /// </summary>
    /// <param name="entities">The entities to update.</param>
    /// <returns>A task that represents the asynchronous update range operation.</returns>
    Task<int> UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

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
    Task<int> ExecuteUpdateAsync(Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls,
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
}