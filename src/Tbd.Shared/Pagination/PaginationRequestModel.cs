namespace Tbd.Shared.Pagination;

/// <summary>
/// Represents the parameters required for pagination.
/// </summary>
public class PaginationRequestModel<TDbDto> : IPaginationRequestModel<TDbDto> where TDbDto : class
{
    [MinLength(1, ErrorMessage = "OrderBy clause is required! Provide at least one OrderBy clause item.")]
    public OrderByClauseList<TDbDto> OrderByClause { get; init; } = new();

    /// <summary>
    /// Desired Page Number of Total Pages.
    /// </summary>
    [Range(1, int.MaxValue)]
    [JsonPropertyName("pageNumber")]
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Desired Page Size indicating the number of items on a page.
    /// </summary>
    [Range(1, int.MaxValue)]
    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }

    /// <summary>
    /// Derives the Offset for Sql. This is (PageNumber - 1) * PageSize
    /// </summary>
    [JsonIgnore]
    [JsonPropertyName("offset")]
    public int Offset => (PageNumber - 1) * PageSize;

    /// <summary>
    /// Derives the Fetch for Sql.  This is the same as PageSize
    /// </summary>
    [JsonIgnore]
    [JsonPropertyName("fetch")]
    public int Fetch => PageSize;

    /// <summary>
    /// Fluent set PageNumber & PageSize
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns>PaginationRequestModel</returns>
    public IPaginationRequestModel<TDbDto> SetPagination(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;

        return this;
    }
}

/// <summary>
/// Represents the parameters required for pagination.
/// </summary>
public interface IPaginationRequestModel<TDbDto> where TDbDto : class
{
    OrderByClauseList<TDbDto> OrderByClause { get; init; }

    /// <summary>
    /// Desired Page Number of Total Pages.
    /// </summary>
    int PageNumber { get; set; }

    /// <summary>
    /// Desired Page Size indicating the number of items on a page.
    /// </summary>
    int PageSize { get; set; }

    /// <summary>
    /// Derives the Offset for Sql. This is (PageNumber - 1) * PageSize
    /// </summary>
    [JsonIgnore]
    int Offset { get; }

    /// <summary>
    /// Derives the Fetch for Sql.  This is the same as PageSize
    /// </summary>
    [JsonIgnore]
    int Fetch { get; }

    /// <summary>
    /// Fluent set PageNumber & PageSize
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns>IPaginationRequestModel</returns>
    IPaginationRequestModel<TDbDto> SetPagination(int pageNumber, int pageSize);
}
