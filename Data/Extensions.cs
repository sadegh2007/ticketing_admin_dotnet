using ERP.Shared.Commons;
using X.PagedList;

namespace ERP.Ticketing.HttpApi.Data;

public static class Extensions
{
    public static async Task<PagedResult<T>> SortAndPagedResultAsync<T>(this IQueryable<T> query, FilterBase filter,
        CancellationToken token = default,
        string defaultSortField = "Id")
    {
        var result = await query.ToPagedListAsync(filter.Page, filter.Count, token);
        return new PagedResult<T>(result.ToArray(), result.TotalItemCount);
    }
}