namespace ExpenseTrackerApi.Dto;

public sealed class PagedResult<T>
{
    public IReadOnlyList<T> Items { get; init; } = Array.Empty<T>();
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int TotalItems { get; init; }
    public int TotalPages => (int) Math.Ceiling((double)TotalItems / Math.Max(PageSize,1));

    public bool HasPrevious => Page > 1;
    public bool HasNext => Page < TotalPages;
}
