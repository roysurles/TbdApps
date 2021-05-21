using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Tbd.Shared.Pagination
{
    /// <summary>
    /// Represents the parameters required for pagination.
    /// </summary>
    public class PaginationRequestModel : IPaginationRequestModel
    {
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

        /// <inheritdoc/>
        [JsonIgnore]
        [JsonPropertyName("offset")]
        public int Offset => (PageNumber - 1) * PageSize;

        /// <inheritdoc/>
        [JsonIgnore]
        [JsonPropertyName("fetch")]
        public int Fetch => PageSize;

        /// <inheritdoc/>
        public IPaginationRequestModel SetPagination(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;

            return this;
        }
    }

    /// <summary>
    /// Represents the parameters required for pagination.
    /// </summary>
    public interface IPaginationRequestModel
    {
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
        IPaginationRequestModel SetPagination(int pageNumber, int pageSize);
    }
}
