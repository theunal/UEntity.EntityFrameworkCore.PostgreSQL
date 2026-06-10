using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
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
            long total_count = source.LongCount(); // paginate harici datanın sayısı çeker
            var items = source.Skip((page - 1) * size).Take(size).ToList();  // paginate datasını çeker
            var pages_count = (long)Math.Ceiling(total_count / (double)size);
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
    public static async Task<Paginate<T>> ToPaginateAsync<T>(this IQueryable<T> source, int page, int size, CancellationToken cancellationToken = default)
    {
        page = page < 1 ? 1 : page;
        size = size <= 0 ? 5 : size;
        try
        {
            long total_count = await source.LongCountAsync(cancellationToken); // paginate harici datanın sayısı çeker
            var items = await source.Skip((page - 1) * size).Take(size).ToListAsync(cancellationToken);  // paginate datasını çeker
            var pages_count = (long)Math.Ceiling(total_count / (double)size);
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

        var replacer = new ParameterReplacer(predicate.Parameters[0], query.Parameters[0]);
        var combinedBody = Expression.AndAlso(query.Body, replacer.Visit(predicate.Body));
        return Expression.Lambda<Func<T, bool>>(combinedBody, query.Parameters[0]);
    }
    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> query, Expression<Func<T, bool>> predicate)
    {
        ArgumentNullException.ThrowIfNull(query);
        ArgumentNullException.ThrowIfNull(predicate);

        var replacer = new ParameterReplacer(predicate.Parameters[0], query.Parameters[0]);
        var combinedBody = Expression.OrElse(query.Body, replacer.Visit(predicate.Body));
        return Expression.Lambda<Func<T, bool>>(combinedBody, query.Parameters[0]);
    }

    private sealed class ParameterReplacer(ParameterExpression source, ParameterExpression target) : ExpressionVisitor
    {
        protected override Expression VisitParameter(ParameterExpression node)
            => node == source ? target : base.VisitParameter(node);
    }

    private static readonly ConcurrentDictionary<(Type, Type), LambdaExpression> _selectAsCache = [];

    public static IQueryable<TDestination> SelectAs<TSource, TDestination>(
        this IQueryable<TSource> query)
        where TDestination : new()
    {
        var key = (typeof(TSource), typeof(TDestination));
        var selector = (Expression<Func<TSource, TDestination>>)_selectAsCache.GetOrAdd(key, static k =>
        {
            Type sourceType = k.Item1;
            Type destinationType = k.Item2;

            ParameterExpression parameter = Expression.Parameter(sourceType, "x");
            List<MemberBinding> bindings = [];

            foreach (var destProp in destinationType.GetProperties())
            {
                var sourceProp = sourceType.GetProperty(destProp.Name);
                if (sourceProp == null) continue;

                var sourceValue = Expression.Property(parameter, sourceProp);
                var binding = Expression.Bind(destProp, sourceValue);
                bindings.Add(binding);
            }

            var body = Expression.MemberInit(Expression.New(destinationType), bindings);
            return Expression.Lambda<Func<TSource, TDestination>>(body, parameter);
        });

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