using System;
using System.Text.Json.Serialization;

namespace Tbd.Shared.Pagination
{
    /// <summary>
    /// Represent the resulting meta data properties of the corresponding pagination request.
    /// </summary>
    [Serializable]
    public class PaginationMetaDataModel : IPaginationMetaDataModel
    {
        /// <summary>
        /// ctor
        /// </summary>
        public PaginationMetaDataModel() { }
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="pageNumber">Requested page number.</param>
        /// <param name="pageSize">Requested page size.</param>
        /// <param name="totalItemCount">Total number of items for all pages.</param>
        public PaginationMetaDataModel(int pageNumber, int pageSize, int totalItemCount)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalItemCount = totalItemCount;
        }

        /// <summary>
        /// Total number of items for all pages.
        /// </summary>
        [JsonPropertyName("totalItemCount")]
        public int TotalItemCount { get; set; }

        /// <summary>
        /// Requested number of items on a page.
        /// </summary>
        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }

        /// <summary>
        /// Requested page number of total pages.
        /// </summary>
        [JsonPropertyName("pageNumber")]
        public int PageNumber { get; set; }

        /// <summary>
        /// Count of pages based on requested PageSize, requested PageNumber and TotalItemCount
        /// </summary>
        [JsonPropertyName("pageCount")]
        public int PageCount =>
            TotalItemCount > 0 && PageSize > 0
            ? (int)Math.Ceiling(TotalItemCount / (decimal)PageSize)
            : 0;

        /// <summary>
        /// Flag indicating if there is a previous page.
        /// </summary>
        [JsonPropertyName("hasPreviousPage")]
        public bool HasPreviousPage => PageNumber > 1;

        /// <summary>
        /// Flag indicating if there is a next page.
        /// </summary>
        [JsonPropertyName("hasNextPage")]
        public bool HasNextPage => PageNumber < PageCount;

        /// <summary>
        /// Flag indicating if this is the first page.
        /// </summary>
        [JsonPropertyName("isFirstPage")]
        public bool IsFirstPage => PageNumber == 1;

        /// <summary>
        /// Flag indicating if this is the last page.
        /// </summary>
        [JsonPropertyName("isLastPage")]
        public bool IsLastPage => PageNumber >= PageCount;

        /// <summary>
        /// The index of the TotalItemCount for the first item on this page.
        /// </summary>
        [JsonPropertyName("firstItemOnPage")]
        public int FirstItemOnPage => ((PageNumber - 1) * PageSize) + 1;

        /// <summary>
        /// The index of the TotalItemCount for the last item on this page.
        /// </summary>
        [JsonPropertyName("lastItemOnPage")]
        public int LastItemOnPage
        {
            get
            {
                var num = FirstItemOnPage + PageSize - 1;
                return num > TotalItemCount ? TotalItemCount : num;
            }
        }

        /// <summary>
        /// Url for the next page if there is a next page.
        /// </summary>
        [JsonPropertyName("nextPageUrl")]
        public string NextPageUrl { get; set; }

        /// <summary>
        /// Url for the previous page if there is a next page.
        /// </summary>
        [JsonPropertyName("previousPageUrl")]
        public string PreviousPageUrl { get; set; }

        /// <summary>
        /// Derives the NextPageUrl and PreviousPageUrl properties.
        /// </summary>
        /// <param name="actionContext">ActionContext</param>
        /// <param name="routeName">Specific route name.</param>
        /// <returns></returns>
        //public IPaginationMetaDataModel SetUrls(ActionContext actionContext, string routeName)
        //{
        //    // TODO:  see if this can be implemented w/o breaking the BlazorClient...see github issue
        //    var urlHelper = new UrlHelper(actionContext);
        //    var routeDictionary = new RouteValueDictionary();
        //    var queryCollection = actionContext.HttpContext.Request.Query;
        //    foreach (var key in queryCollection.Keys)
        //    {
        //        routeDictionary.Add(key, queryCollection[key]);
        //    }
        //    if (HasNextPage)
        //    {
        //        if (routeDictionary.ContainsKey("PageNumber"))
        //            routeDictionary["PageNumber"] = PageNumber + 1;
        //        NextPageUrl = urlHelper.Link(routeName, routeDictionary);
        //    }
        //    if (HasPreviousPage)
        //    {
        //        if (routeDictionary.ContainsKey("PageNumber"))
        //            routeDictionary["PageNumber"] = PageNumber - 1;
        //        PreviousPageUrl = urlHelper.Link(routeName, routeDictionary);
        //    }
        //    return this;
        //}
    }

    /// <summary>
    /// Represent the resulting meta data properties of the corresponding pagination request.
    /// </summary>
    public interface IPaginationMetaDataModel
    {
        /// <summary>
        /// Total number of items for all pages.
        /// </summary>
        int TotalItemCount { get; set; }

        /// <summary>
        /// Requested number of items on a page.
        /// </summary>
        int PageSize { get; set; }

        /// <summary>
        /// Requested page number of total pages.
        /// </summary>
        int PageNumber { get; set; }

        /// <summary>
        /// Count of pages based on requested PageSize, requested PageNumber and TotalItemCount
        /// </summary>
        int PageCount { get; }

        /// <summary>
        /// Flag indicating if there is a previous page.
        /// </summary>
        bool HasPreviousPage { get; }

        /// <summary>
        /// Flag indicating if there is a next page.
        /// </summary>
        bool HasNextPage { get; }

        /// <summary>
        /// Flag indicating if this is the first page.
        /// </summary>
        bool IsFirstPage { get; }

        /// <summary>
        /// Flag indicating if this is the last page.
        /// </summary>
        bool IsLastPage { get; }

        /// <summary>
        /// The index of the TotalItemCount for the first item on this page.
        /// </summary>
        int FirstItemOnPage { get; }

        /// <summary>
        /// The index of the TotalItemCount for the last item on this page.
        /// </summary>
        int LastItemOnPage { get; }

        /// <summary>
        /// Url for the next page if there is a next page.
        /// </summary>
        string NextPageUrl { get; set; }

        /// <summary>
        /// Url for the previous page if there is a next page.
        /// </summary>
        string PreviousPageUrl { get; set; }

        /// <summary>
        /// Derives the NextPageUrl and PreviousPageUrl properties.
        /// </summary>
        /// <param name="actionContext">ActionContext</param>
        /// <param name="routeName">Specific route name.</param>
        /// <returns></returns>
        //IPaginationMetaDataModel SetUrls(ActionContext actionContext, string routeName);
    }
}
