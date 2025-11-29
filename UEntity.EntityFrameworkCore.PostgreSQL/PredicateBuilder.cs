using System.Linq.Expressions;

namespace UEntity.EntityFrameworkCore.PostgreSQL;

public static class PredicateBuilder
{
    public static Expression<Func<T, bool>> NewQuery<T>(bool @is) => x => @is;
    public static Expression<Func<T, bool>> NewQuery<T>(Expression<Func<T, bool>> predicate) => predicate;
}