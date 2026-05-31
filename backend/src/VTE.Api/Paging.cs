using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace VTE.Api;

public record PagedResult<T>(int Page, int PageSize, int Total, IReadOnlyList<T> Items);

// A whitelist of "sortable column-name" -> entity property selector. Lets the API accept
// arbitrary UI field names while keeping us safe from open-ended ORDER BY injection.
public sealed class SortMap<T>
{
    private readonly Dictionary<string, LambdaExpression> _map = new(StringComparer.OrdinalIgnoreCase);
    private readonly LambdaExpression _default;
    private readonly bool _defaultDesc;

    public SortMap(Expression<Func<T, object?>> defaultKey, bool defaultDescending = false)
    { _default = defaultKey; _defaultDesc = defaultDescending; }

    public SortMap<T> Add<TKey>(string field, Expression<Func<T, TKey>> selector)
    { _map[field] = selector; return this; }

    public IOrderedQueryable<T> Apply(IQueryable<T> q, string? sortBy, string? sortDir)
    {
        var desc = string.Equals(sortDir, "desc", StringComparison.OrdinalIgnoreCase);
        if (sortBy is null || !_map.TryGetValue(sortBy, out var lambda))
        { lambda = _default; desc = _defaultDesc; }
        var method = desc ? "OrderByDescending" : "OrderBy";
        var expr = Expression.Call(typeof(Queryable), method,
            new[] { typeof(T), lambda.ReturnType }, q.Expression, Expression.Quote(lambda));
        return (IOrderedQueryable<T>)q.Provider.CreateQuery<T>(expr);
    }
}

public static class PagingExtensions
{
    public const int DefaultPageSize = 50;
    public const int MaxPageSize     = 200;

    public static (int page, int pageSize) Normalise(int? page, int? pageSize)
    {
        var p = page is null or < 1 ? 1 : page.Value;
        var s = pageSize is null or < 1 ? DefaultPageSize : Math.Min(pageSize.Value, MaxPageSize);
        return (p, s);
    }

    // Read-only paginate. Caller must apply OrderBy first (paging is undefined without an order).
    public static async Task<PagedResult<T>> ToPagedAsync<T>(
        this IQueryable<T> query, int? page, int? pageSize, CancellationToken ct = default)
    {
        var (p, s) = Normalise(page, pageSize);
        var total = await query.CountAsync(ct);
        var items = await query.Skip((p - 1) * s).Take(s).ToListAsync(ct);
        return new PagedResult<T>(p, s, total, items);
    }
}
