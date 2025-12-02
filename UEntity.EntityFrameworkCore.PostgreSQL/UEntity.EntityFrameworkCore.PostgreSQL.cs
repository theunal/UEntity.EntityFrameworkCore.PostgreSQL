using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System.Linq.Expressions;
using System.Threading;

namespace UEntity.EntityFrameworkCore.PostgreSQL;

public class EfEntityRepositoryBase<TEntity, TContext>(TContext context) :
    IEntityRepository<TEntity> where TEntity : class, IEntity, new() where TContext : DbContext, new()
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
    public TEntity? FirstOrDefault(Expression<Func<TEntity, object>> sort, bool? asNoTracking = false)
    {
        IQueryable<TEntity> query = context.Set<TEntity>();
        if (asNoTracking == true)
        {
            query = query.AsNoTracking();
        }

        return query.OrderBy(sort).FirstOrDefault();
    }
    public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, object>> sort,
        CancellationToken cancellationToken = default, bool? asNoTracking = false)
    {
        IQueryable<TEntity> query = context.Set<TEntity>();
        if (asNoTracking == true)
        {
            query = query.AsNoTracking();
        }

        return query.OrderBy(sort).FirstOrDefaultAsync(cancellationToken);
    }

    // last
    public TEntity? LastOrDefault(Expression<Func<TEntity, object>> sort, bool? asNoTracking = false)
    {
        IQueryable<TEntity> query = context.Set<TEntity>();
        if (asNoTracking == true)
        {
            query = query.AsNoTracking();
        }

        return query.OrderByDescending(sort).FirstOrDefault();
    }
    public Task<TEntity?> LastOrDefaultAsync(Expression<Func<TEntity, object>> sort,
        CancellationToken cancellationToken = default, bool? asNoTracking = false)
    {
        IQueryable<TEntity> query = context.Set<TEntity>();
        if (asNoTracking == true)
        {
            query = query.AsNoTracking();
        }

        return query.OrderByDescending(sort).FirstOrDefaultAsync(cancellationToken);
    }

    // get
    public TEntity? Get(
        Expression<Func<TEntity, bool>> filter,
        EntitySortModel<TEntity>? sort = null,
        bool? asNoTracking = false,
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

    // get select
    public TResult? Select<TResult>(
        Expression<Func<TEntity, TResult>> select, // 👈 Yeni Projeksiyon Parametresi
        Expression<Func<TEntity, bool>> filter,
        EntitySortModel<TEntity>? sort = null,
        bool? asNoTracking = false) // Projeksiyon tipinin referans tip olmasını varsayalım
    {
        IQueryable<TEntity>? query = Sort(filter, sort, asNoTracking);
        IQueryable<TResult> selectedQuery = query.Select(select);
        return selectedQuery.FirstOrDefault();
    }

    // select as
    public TResult? SelectAs<TResult>(
        Expression<Func<TEntity, bool>> filter,
        EntitySortModel<TEntity>? sort = null,
        bool? asNoTracking = false)
        where TResult : new()
    {
        IQueryable<TEntity>? query = Sort(filter, sort, asNoTracking);
        IQueryable<TResult> selectedQuery = query.SelectAs<TEntity, TResult>();
        return selectedQuery.FirstOrDefault();
    }

    // get async
    public Task<TEntity?> GetAsync(
        Expression<Func<TEntity, bool>> filter,
        EntitySortModel<TEntity>? sort = null,
        bool? asNoTracking = false,
        CancellationToken cancellationToken = default,
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

    // get async select
    public Task<TResult?> SelectAsync<TResult>(
        Expression<Func<TEntity, TResult>> select, // 👈 Yeni Projeksiyon Parametresi
        Expression<Func<TEntity, bool>> filter,
        EntitySortModel<TEntity>? sort = null,
        bool? asNoTracking = false,
        CancellationToken cancellationToken = default) // Projeksiyon tipinin referans tip olmasını varsayalım
    {
        IQueryable<TEntity>? query = Sort(filter, sort, asNoTracking);
        IQueryable<TResult> selectedQuery = query.Select(select);
        return selectedQuery.FirstOrDefaultAsync(cancellationToken);
    }

    // select as async
    public Task<TResult?> SelectAsAsync<TResult>(
        Expression<Func<TEntity, bool>> filter,
        EntitySortModel<TEntity>? sort = null,
        bool? asNoTracking = false,
        CancellationToken cancellationToken = default)
        where TResult : new()
    {
        IQueryable<TEntity>? query = Sort(filter, sort, asNoTracking);
        IQueryable<TResult> selectedQuery = query.SelectAs<TEntity, TResult>();
        return selectedQuery.FirstOrDefaultAsync(cancellationToken);
    }

    // get all
    public List<TEntity> GetAll(
        Expression<Func<TEntity, bool>>? filter = null,
        EntitySortModel<TEntity>? sort = null,
        bool? asNoTracking = false,
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

    // get select all
    public List<TResult> SelectAll<TResult>(
        Expression<Func<TEntity, TResult>> select,
        Expression<Func<TEntity, bool>>? filter = null,
        EntitySortModel<TEntity>? sort = null,
        bool? asNoTracking = false)
    {
        IQueryable<TEntity>? query = Sort(filter, sort, asNoTracking);
        IQueryable<TResult> selectedQuery = query.Select(select);
        return [.. selectedQuery];
    }

    // select as all
    public List<TResult> SelectAsAll<TResult>(
        Expression<Func<TEntity, bool>> filter,
        EntitySortModel<TEntity>? sort = null,
        bool? asNoTracking = false)
        where TResult : new()
    {
        IQueryable<TEntity>? query = Sort(filter, sort, asNoTracking);
        IQueryable<TResult> selectedQuery = query.SelectAs<TEntity, TResult>();
        return [.. selectedQuery];
    }

    // get all async
    public Task<List<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        EntitySortModel<TEntity>? sort = null,
        bool? asNoTracking = false,
        CancellationToken cancellationToken = default,
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
        return query.ToListAsync(cancellationToken);
    }

    // get async select all
    public Task<List<TResult>> SelectAllAsync<TResult>(
        Expression<Func<TEntity, TResult>> select,
        Expression<Func<TEntity, bool>>? filter = null,
        EntitySortModel<TEntity>? sort = null,
        bool? asNoTracking = false,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity>? query = Sort(filter, sort, asNoTracking);
        IQueryable<TResult> selectedQuery = query.Select(select);
        return selectedQuery.ToListAsync(cancellationToken);
    }

    // select as all async
    public Task<List<TResult>> SelectAsAllAsync<TResult>(
        Expression<Func<TEntity, bool>> filter,
        EntitySortModel<TEntity>? sort = null,
        bool? asNoTracking = false,
        CancellationToken cancellationToken = default)
        where TResult : new()
    {
        IQueryable<TEntity>? query = Sort(filter, sort, asNoTracking);
        IQueryable<TResult> selectedQuery = query.SelectAs<TEntity, TResult>();
        return selectedQuery.ToListAsync(cancellationToken);
    }

    // get array
    public TEntity[] GetArray(
        Expression<Func<TEntity, bool>>? filter = null,
        EntitySortModel<TEntity>? sort = null,
        bool? asNoTracking = false,
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

    // get select array
    public TResult[] SelectArray<TResult>(
        Expression<Func<TEntity, TResult>> select,
        Expression<Func<TEntity, bool>>? filter = null,
        EntitySortModel<TEntity>? sort = null,
        bool? asNoTracking = false)
    {
        IQueryable<TEntity>? query = Sort(filter, sort, asNoTracking);
        IQueryable<TResult> selectedQuery = query.Select(select);
        return [.. selectedQuery];
    }

    // get all async
    public Task<TEntity[]> GetArrayAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        EntitySortModel<TEntity>? sort = null,
        bool? asNoTracking = false,
        CancellationToken cancellationToken = default,
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
        return query.ToArrayAsync(cancellationToken);
    }

    // get async select all
    public Task<TResult[]> SelectArrayAsync<TResult>(
        Expression<Func<TEntity, TResult>> select,
        Expression<Func<TEntity, bool>>? filter = null,
        EntitySortModel<TEntity>? sort = null,
        bool? asNoTracking = false)
    {
        IQueryable<TEntity>? query = Sort(filter, sort, asNoTracking);
        IQueryable<TResult> selectedQuery = query.Select(select);
        return selectedQuery.ToArrayAsync();
    }

    // paginate
    public Paginate<TEntity> GetListPaginate(
        int page, int size,
        Expression<Func<TEntity, bool>>? filter = null,
        EntitySortModel<TEntity>? sort = null,
        bool? asNoTracking = false,
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
    public Task<Paginate<TEntity>> GetListPaginateAsync(
        int page, int size,
        Expression<Func<TEntity, bool>>? filter = null,
        EntitySortModel<TEntity>? sort = null,
        bool? asNoTracking = false,
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

    // add
    public int Add(TEntity entity)
    {
        context.Set<TEntity>().Add(entity);
        return context.SaveChanges();
    }
    public async Task<int> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await context.Set<TEntity>().AddAsync(entity, cancellationToken);
        return await context.SaveChangesAsync(cancellationToken);
    }

    // add range
    public int AddRange(IEnumerable<TEntity> entities)
    {
        context.AddRange(entities);
        return context.SaveChanges();
    }
    public async Task<int> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await context.AddRangeAsync(entities, cancellationToken);
        return await context.SaveChangesAsync(cancellationToken);
    }

    // update
    public int Update(TEntity entity)
    {
        context.Set<TEntity>().Update(entity);
        return context.SaveChanges();
    }
    public Task<int> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        context.Set<TEntity>().Update(entity);
        return context.SaveChangesAsync(cancellationToken);
    }

    // update range
    public int UpdateRange(IEnumerable<TEntity> entities)
    {
        context.UpdateRange(entities);
        return context.SaveChanges();
    }
    public Task<int> UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        context.UpdateRange(entities);
        return context.SaveChangesAsync(cancellationToken);
    }

    // execute update
    public int ExecuteUpdate(
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls,
        Expression<Func<TEntity, bool>>? filter = null)
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
    public Task<int> ExecuteUpdateAsync(
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls,
        Expression<Func<TEntity, bool>>? filter = null,
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
        context.Set<TEntity>().Remove(entity);
        return context.SaveChanges();
    }
    public Task<int> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        context.Set<TEntity>().Remove(entity);
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
        return context.Set<TEntity>()
            .AsNoTracking()
            .All(filter);
    }
    public Task<bool> AllAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default)
    {
        return context.Set<TEntity>()
            .AsNoTracking()
            .AllAsync(filter, cancellationToken);
    }

    // any
    public bool Any(Expression<Func<TEntity, bool>> filter)
    {
        return context.Set<TEntity>()
            .AsNoTracking()
            .Any(filter);
    }
    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default)
    {
        return context.Set<TEntity>()
            .AsNoTracking()
            .AnyAsync(filter, cancellationToken);
    }

    // max
    public TResult? Max<TResult>(Expression<Func<TEntity, TResult>> selector)
    {
        return context.Set<TEntity>()
            .AsNoTracking()
            .Max(selector);
    }
    public Task<TResult> MaxAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        CancellationToken cancellationToken = default)
    {
        return context.Set<TEntity>()
            .AsNoTracking()
            .MaxAsync(selector, cancellationToken);
    }
    public TEntity? MaxBy<TResult>(
        Func<TEntity, TResult> keySelector,
        Expression<Func<TEntity, bool>>? filter = null,
        bool? asNoTracking = false)
    {
        return Filter(filter, asNoTracking).MaxBy(keySelector);
    }

    // min
    public TResult? Min<TResult>(Expression<Func<TEntity, TResult>> filter)
    {
        return context.Set<TEntity>()
            .AsNoTracking()
            .Min(filter);
    }
    public Task<TResult> MinAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        CancellationToken cancellationToken = default)
    {
        return context.Set<TEntity>()
            .AsNoTracking()
            .MinAsync(selector, cancellationToken);
    }
    public TEntity? MinBy<TResult>(
        Func<TEntity, TResult> keySelector,
        Expression<Func<TEntity, bool>>? filter = null,
        bool? asNoTracking = false)
    {
        return Filter(filter, asNoTracking).MinBy(keySelector);
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