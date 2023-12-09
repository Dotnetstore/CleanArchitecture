using System.Linq.Expressions;

namespace Application.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<TSource> WhereNullable<TSource, TParameter>(this IQueryable<TSource> source,
        TParameter parameter, Expression<Func<TSource, bool>> predicate)
    {
        return parameter is null ? source : source.Where(predicate);
    }
}