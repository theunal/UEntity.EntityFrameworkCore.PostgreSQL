using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace UEntity.PostgreSQL;
public interface IEntityRepository<T> where T : class, IEntity, new()
{
    int Count(Expression<Func<T, bool>>? filter = null);
    Task<int> CountAsync(Expression<Func<T, bool>>? filter = null);

    T? FirstOrDefault(Expression<Func<T, object>> sort);
    Task<T?> LastOrDefaultAsync(Expression<Func<T, object>> sort);

    T? LastOrDefault(Expression<Func<T, object>> sort);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, object>> sort);

    T? Get(Expression<Func<T, bool>> filter, EntitySortModel<T>? sort = null, bool? asNoTracking = false);
    TResult? Get<TResult>(Expression<Func<T, bool>> filter, Func<T, TResult> select, EntitySortModel<T>? sort = null, bool? asNoTracking = false);
    Task<T?> GetAsync(Expression<Func<T, bool>> filter, EntitySortModel<T>? sort = null, bool? asNoTracking = false);

    List<T> GetAll(Expression<Func<T, bool>>? filter = null, EntitySortModel<T>? sort = null, bool? asNoTracking = false);
    List<T> GetAll<TResult>(Expression<Func<T, bool>>? filter = null, EntitySortModel<T>? sort = null, bool? asNoTracking = false, Func<T, TResult>? distincby = null);
    Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, EntitySortModel<T>? sort = null, bool? asNoTracking = false);

    T[] GetArray(Expression<Func<T, bool>>? filter = null, EntitySortModel<T>? sort = null, bool? asNoTracking = false);
    T[] GetArray<TResult>(Expression<Func<T, bool>>? filter = null, EntitySortModel<T>? sort = null, bool? asNoTracking = false, Func<T, TResult>? distincby = null);
    Task<T[]> GetArrayAsync(Expression<Func<T, bool>>? filter = null, EntitySortModel<T>? sort = null, bool? asNoTracking = false);

    Paginate<T> GetListPaginate(int page, int size, Expression<Func<T, bool>>? filter = null, EntitySortModel<T>? sort = null, bool? asNoTracking = false);
    Paginate<T> GetListPaginate<TResult>(int page, int size, Expression<Func<T, bool>>? filter = null, EntitySortModel<T>? sort = null, bool? asNoTracking = false, Func<T, TResult>? distincby = null);
    Task<Paginate<T>> GetListPaginateAsync(int page, int size, Expression<Func<T, bool>>? filter = null, EntitySortModel<T>? sort = null, bool? asNoTracking = false);

    List<TResult> GetSelectList<TResult>(Func<T, TResult> select, Expression<Func<T, bool>>? filter = null, EntitySortModel<T>? sort = null, bool? asNoTracking = false);
    TResult[] GetSelectArray<TResult>(Func<T, TResult> select, Expression<Func<T, bool>>? filter = null, EntitySortModel<T>? sort = null, bool? asNoTracking = false);
    Paginate<TResult> GetSelectPaginateList<TResult>(int page, int size, Func<T, TResult> select, Expression<Func<T, bool>>? filter = null, EntitySortModel<T>? sort = null,
        bool? asNoTracking = false);

    List<T> GetTakeList(int count, EntitySortModel<T> sort, Expression<Func<T, bool>>? filter = null, bool? asNoTracking = false);
    T[] GetTakeArray(int count, EntitySortModel<T> sort, Expression<Func<T, bool>>? filter = null, bool? asNoTracking = false);

    void Add(T entity);
    Task AddAsync(T entity);

    void AddRange(IEnumerable<T> entities);
    Task AddRangeAsync(IEnumerable<T> entities);

    void Update(T entity);
    Task UpdateAsync(T entity);

    void UpdateRange(IEnumerable<T> entities);
    Task UpdateRangeAsync(IEnumerable<T> entities);

    int ExecuteUpdate(Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls, Expression<Func<T, bool>>? filter = null);
    Task<int> ExecuteUpdateAsync(Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls, Expression<Func<T, bool>>? filter = null);

    void Delete(T entity);
    Task DeleteAsync(T entity);

    void DeleteRange(IEnumerable<T> entities);
    Task DeleteRangeAsync(IEnumerable<T> entities);

    int ExecuteDelete(Expression<Func<T, bool>>? filter = null);
    Task<int> ExecuteDeleteAsync(Expression<Func<T, bool>>? filter = null);

    bool All(Expression<Func<T, bool>> filter);
    Task<bool> AllAsync(Expression<Func<T, bool>> filter);

    bool Any(Expression<Func<T, bool>> filter);
    Task<bool> AnyAsync(Expression<Func<T, bool>> filter);

    TResult? Max<TResult>(Expression<Func<T, TResult>> filter);
    Task<TResult> MaxAsync<TResult>(Expression<Func<T, TResult>> filter);
    T? MaxBy<TKey>(Func<T, TKey> keyselect, Expression<Func<T, bool>>? filter = null, bool? asNoTracking = false);

    TResult? Min<TResult>(Expression<Func<T, TResult>> filter);
    Task<TResult> MinAsync<TResult>(Expression<Func<T, TResult>> filter);
    T? MinBy<TKey>(Func<T, TKey> keyselect, Expression<Func<T, bool>>? filter = null, bool? asNoTracking = false);
}
public class EfEntityRepositoryBase<TEntity, TContext>(TContext context) : IEntityRepository<TEntity> where TEntity : class, IEntity, new() where TContext : DbContext, new()
{
    // count
    public int Count(Expression<Func<TEntity, bool>>? filter = null) => (filter is not null ? context.Set<TEntity>().Where(filter) : context.Set<TEntity>()).Count();
    public Task<int> CountAsync(Expression<Func<TEntity, bool>>? filter = null) => (filter is not null ? context.Set<TEntity>().Where(filter) : context.Set<TEntity>()).CountAsync();

    // last
    public TEntity? LastOrDefault(Expression<Func<TEntity, object>> sort) => context.Set<TEntity>().OrderByDescending(sort).FirstOrDefault();
    public Task<TEntity?> LastOrDefaultAsync(Expression<Func<TEntity, object>> sort) => context.Set<TEntity>().OrderByDescending(sort).FirstOrDefaultAsync();

    // first 
    public TEntity? FirstOrDefault(Expression<Func<TEntity, object>> sort) => context.Set<TEntity>().OrderBy(sort).FirstOrDefault();
    public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, object>> sort) => context.Set<TEntity>().OrderBy(sort).FirstOrDefaultAsync();

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
        return sort != null ? sort.sortType == SortType.Ascending ? query.OrderBy(sort.sort) : query.OrderByDescending(sort.sort) : query;
    }
    private IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>>? filter = null, bool? asNoTracking = false)
    {
        IQueryable<TEntity> query = asNoTracking is true ? context.Set<TEntity>().AsNoTracking() : context.Set<TEntity>();
        return filter != null ? query.Where(filter) : query;
    }
}

public static class UEntityPaginateExtension
{
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

public interface IEntity { }
public record EntitySortModel<T>
{
    public Expression<Func<T, object>> sort { get; set; } = null!;
    public SortType sortType { get; set; }
}
public enum SortType
{
    Ascending,
    Descending
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