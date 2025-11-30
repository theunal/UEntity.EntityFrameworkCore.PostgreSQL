using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace UEntity.EntityFrameworkCore.PostgreSQL;

public static class UEntityExtensions
{
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

    public static IQueryable<TDestination> SelectAs<TSource, TDestination>(
        this IQueryable<TSource> query)
        where TDestination : new()
    {
        Type sourceType = typeof(TSource);
        Type destinationType = typeof(TDestination);

        ParameterExpression parameter = Expression.Parameter(sourceType, "x");
        List<MemberBinding> bindings = [];

        foreach (var destProp in destinationType.GetProperties())
        {
            // Aynı isimde property var mı?
            var sourceProp = sourceType.GetProperty(destProp.Name);
            if (sourceProp == null) continue;

            // x.{Property}
            var sourceValue = Expression.Property(parameter, sourceProp);

            // {Property} = x.{Property}
            var binding = Expression.Bind(destProp, sourceValue);

            bindings.Add(binding);
        }

        var body = Expression.MemberInit(Expression.New(destinationType), bindings);
        var selector = Expression.Lambda<Func<TSource, TDestination>>(body, parameter);

        return query.Select(selector);
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