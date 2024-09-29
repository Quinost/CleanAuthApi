using System.Linq.Expressions;

namespace CleanAuth.Infrastructure.EF;

public static class EFExtensions
{
    public static IQueryable<TSource> WhereIf<TSource>(
        this IQueryable<TSource> source,
        bool ifPredict,
        Expression<Func<TSource, bool>> predicate)
    {
        return ifPredict ? source.Where(predicate) : source;
    }
}