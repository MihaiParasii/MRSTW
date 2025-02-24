using System.Collections;
using Microsoft.EntityFrameworkCore;

namespace MRSTW.BusinessLogicLayer.Common.Models;

public class PaginatedList<T>(IReadOnlyCollection<T> items, int count, int pageNumber, int pageSize) : IEnumerable<T>
{
    public IReadOnlyCollection<T> Items { get; } = items;
    public int TotalCount { get; } = count;
    private int PageNumber { get; } = pageNumber;
    private int TotalPages { get; } = (int)Math.Ceiling(count / (double)pageSize);

    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;

    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
    {
        int count = await source.CountAsync();
        var items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedList<T>(items, count, pageNumber, pageSize);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return Items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
