using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Tbd.Shared.OrderBy
{
    /// <summary>
    /// Represents a request to define an 'Order By' clause for embedded sql and/or dynamic sql
    /// by strong typing the columns / properties against the desired generic class
    /// </summary>
    /// <typeparam name="TDbDto">The desired database dto used for retrieval of records</typeparam>
    public class OrderByRequestModel<TDbDto> where TDbDto : class
    {
        /// <summary>
        /// List of OrderByItemModel's
        /// </summary>
        public List<OrderByItemModel> Items { get; set; } = new();

        /// <summary>
        /// Add OrderByItemModel (Ascending direction) to Items via strong typed expression
        /// If position is null, the Max position + 1 is used
        /// </summary>
        /// <typeparam name="TProperty">Desired dto public property</typeparam>
        /// <param name="expression">Desired MethodExpression</param>
        /// <param name="position">Concatenation order</param>
        /// <returns>OrderByRequestModel of TDbDto</returns>
        public OrderByRequestModel<TDbDto> OrderByAscending<TProperty>(Expression<Func<TDbDto, TProperty>> expression, int? position = null)
        {
            Items.Add(new OrderByItemModel(GetPropertyNameFromExpression(expression)
                , OrderByDirectionEnumeration.Ascending
                , position ?? GetMaxPosition() + 1));

            return this;
        }

        /// <summary>
        /// Add OrderByItemModel (Ascending direction) to Items via string (nameof(dto.Property))
        /// If position is null, the Max position + 1 is used
        /// </summary>
        /// <param name="propertyName">Desired dto public property</param>
        /// <param name="position">Concatenation order</param>
        /// <returns>OrderByRequestModel of TDbDto</returns>
        public OrderByRequestModel<TDbDto> OrderByAscending(string propertyName, int? position = null)
        {
            Items.Add(new OrderByItemModel(propertyName, OrderByDirectionEnumeration.Ascending, position ?? GetMaxPosition() + 1));

            return this;
        }

        /// <summary>
        /// Add OrderByItemModel (Descending direction) to Items via strong typed expression
        /// If position is null, the Max position + 1 is used
        /// </summary>
        /// <typeparam name="TProperty">Desired dto public property</typeparam>
        /// <param name="expression">Desired MethodExpression</param>
        /// <param name="position">Concatenation order</param>
        /// <returns>OrderByRequestModel of TDbDto</returns>
        public OrderByRequestModel<TDbDto> OrderByDescending<TProperty>(Expression<Func<TDbDto, TProperty>> expression, int? position = null)
        {
            Items.Add(new OrderByItemModel(GetPropertyNameFromExpression(expression)
                , OrderByDirectionEnumeration.Descending
                , position ?? GetMaxPosition() + 1));

            return this;
        }

        /// <summary>
        /// Add OrderByItemModel (Ascending direction) to Items via string (nameof(dto.Property))
        /// If position is null, the Max position + 1 is used
        /// </summary>
        /// <param name="propertyName">Desired dto public property</param>
        /// <param name="position">Concatenation order</param>
        /// <returns>OrderByRequestModel of TDbDto</returns>
        public OrderByRequestModel<TDbDto> OrderByDescending(string propertyName, int? position = null)
        {
            Items.Add(new OrderByItemModel(propertyName, OrderByDirectionEnumeration.Descending, position ?? GetMaxPosition() + 1));

            return this;
        }

        /// <summary>
        /// Get the maximum 'Position' number of the content in 'Items'
        /// </summary>
        /// <returns></returns>
        public int GetMaxPosition() =>
            Items.Count > 0 ? Items.Max(i => i.Position) : 0;

        /// <summary>
        /// Validates each item in Item's is a public property of the desired generic class
        /// </summary>
        /// <returns>Tuple of IsValid and IEnumerable of string (ErrorMessages)</returns>
        public (bool IsValid, IEnumerable<string> ErrorMessages) ValidateItemsArePublicProperties()
        {
            var errorMessages = new List<string>();

            if (Items.Any().Equals(false))
            {
                errorMessages.Add("At least one 'Item' is required!");
                return (false, errorMessages);
            }

            var type = typeof(TDbDto);
            var memberInfos = type.GetMembers();
            foreach (var item in Items)
            {
                var member = Array.Find(memberInfos, m => Equals(item.PropertyName, m.Name)
                                                     && Equals(m.MemberType, MemberTypes.Property));
                if (member is null)
                    errorMessages.Add($"{item.PropertyName} is not defined as a public property in: {type.FullName}!");
            }

            return (errorMessages.Any().Equals(false), errorMessages);
        }

        /// <summary>
        /// Renders a concatenated csv list of Items ordered by position
        /// </summary>
        /// <returns></returns>
        public override string ToString() =>
            string.Join(", ", Items.OrderBy(o => o.Position));

        /// <summary>
        /// Returns the desired property name of the MemberExpression
        /// Only MemberExpression is accepted - NotSupportedException is thrown for all other expressions
        /// </summary>
        /// <typeparam name="TProperty">The desired property</typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        protected virtual string GetPropertyNameFromExpression<TProperty>(Expression<Func<TDbDto, TProperty>> expression)
        {
            return expression.Body switch
            {
                MemberExpression memberExpression => memberExpression.Member.Name,
                _ => throw new NotSupportedException($"Only public properties are allowed.  This expression is not allowed:  {expression.GetType()}")
            };
        }
    }
}
