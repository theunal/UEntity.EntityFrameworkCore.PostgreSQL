using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System.Linq.Expressions;

namespace UEntity.EntityFrameworkCore.PostgreSQL;

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