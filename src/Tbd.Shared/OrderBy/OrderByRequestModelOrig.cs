//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;

using System.Text.Json.Serialization;

namespace Tbd.Shared.OrderBy
{
    /// <summary>
    /// Represents a generic item to sort by.
    /// </summary>
    /// <typeparam name="T">Desired Type</typeparam>
    public class OrderByRequestModelOrig<T>
    {
        /// <summary>
        /// Generic Item
        /// </summary>
        [JsonPropertyName("item")]
        public T Item { get; set; }

        ///// <summary>
        ///// SortOrder Enumeration
        ///// </summary>
        //[JsonPropertyName("sortOrderType")]
        //public SortOrderEnum SortOrder { get; set; }
    }
}

//    public class OrderByModel
//    {
//        public string PropertyName { get; set; }

//        public OrderByTypeEnumeration OrderByType { get; set; }
//    }

//    public class OrderByRequestModel
//    {
//        public OrderByRequestModel AddOrderBy(string propertyName, OrderByTypeEnumeration orderByType)
//        {
//            OrderBys.Add(new OrderByModel { PropertyName = propertyName, OrderByType = orderByType });
//            return this;
//        }

//        public IList<OrderByModel> OrderBys { get; set; } = new List<OrderByModel>();
//    }

//    //public static class Process
//    //{
//    //    public static void DoIt()
//    //    {
//    //        var orderByRequestModel = new OrderByRequestModel();
//    //        orderByRequestModel.AddOrderBy("ControllerName", OrderByTypeEnumeration.Ascending);
//    //        orderByRequestModel.AddOrderBy("ActionName", OrderByTypeEnumeration.Descending);

//    //        var data = new List<ApiLogDto>();
//    //        IQueryable<ApiLogDto> result = null;

//    //        var isFirst = true;
//    //        foreach (var orderBy in orderByRequestModel.OrderBys)
//    //        {
//    //            if (isFirst)
//    //                result = data.AsQueryable().OrderByEx(orderBy.PropertyName, orderBy.OrderByType == OrderByTypeEnumeration.Ascending);
//    //            else
//    //                result = result.OrderByEx(orderBy.PropertyName, orderBy.OrderByType == OrderByTypeEnumeration.Ascending);

//    //            isFirst = false;
//    //        }
//    //    }
//    //}

//    // https://www.codeproject.com/Questions/987016/How-to-build-dynamic-order-by-clause-in-LINQ-query
//    // https://asontu.github.io/2020/04/02/a-better-way-to-do-dynamic-orderby-in-c-sharp.html
//    // https://www.c-sharpcorner.com/blogs/dynamic-linq-multi-sorting
//    // https://jasonwatmore.com/post/2014/07/16/dynamic-linq-using-strings-to-sort-by-properties-and-child-object-properties
//    public static class IQueryableExtensions
//    {
//        // https://www.c-sharpcorner.com/article/dynamic-sorting-orderby-based-on-user-preference/
//        public static IQueryable<T> OrderByEx<T>(this IQueryable<T> source, string columnName, bool isAscending = true)
//        {
//            if (string.IsNullOrWhiteSpace(columnName))
//                return source;

//            ParameterExpression parameter = Expression.Parameter(source.ElementType, "");

//            MemberExpression property = Expression.Property(parameter, columnName);
//            LambdaExpression lambda = Expression.Lambda(property, parameter);

//            string methodName = isAscending ? "OrderBy" : "OrderByDescending";

//            Expression methodCallExpression = Expression.Call(typeof(Queryable)
//                , methodName
//                , new Type[] { source.ElementType, property.Type }
//                , source.Expression
//                , Expression.Quote(lambda));

//            return source.Provider.CreateQuery<T>(methodCallExpression);
//        }
//    }
//}
