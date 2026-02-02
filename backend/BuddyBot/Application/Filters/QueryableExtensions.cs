using Domain.Filters;

namespace Application.Filters;
public static class QueryableExtensions
{
    public static IQueryable<T> ApplyFilters<T>( this IQueryable<T> query, IEnumerable<IFilter<T>> filters )
    {
        if ( filters == null )
        {
            return query;
        }

        foreach ( var filter in filters )
        {
            query = filter.Apply( query );
        }

        return query;
    }
}
