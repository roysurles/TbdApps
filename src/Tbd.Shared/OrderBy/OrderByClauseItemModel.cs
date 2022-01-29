using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Tbd.Shared.OrderBy
{
    /// <summary>
    /// Represents an item in the Order By clause
    /// </summary>
    public class OrderByClauseItemModel<TDbDto> where TDbDto : class
    {
        /// <summary>
        /// ctor
        /// </summary>
        public OrderByClauseItemModel() { }

        /// <summary>
        /// ctor with parameters
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="direction"></param>
        public OrderByClauseItemModel(string propertyName, OrderByDirectionEnumeration direction)
        {
            PropertyName = propertyName;
            Direction = direction;            
        }

        [Required]
        private readonly string _propertyName;
        public string PropertyName
        {
            get => _propertyName;
            init
            {

                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException(nameof(PropertyName), $"{ClassName}.{nameof(PropertyName)} cannot be null or whitespace.");

                if (typeof(TDbDto).GetProperty(value, BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static) is null)
                    throw new ArgumentException($"Cannot set {ClassName}.{nameof(PropertyName)}, because {GenericTypeName} does not have a public property named: {value}.");

                _propertyName = value;
            }
        }

        public OrderByDirectionEnumeration Direction { get; init; } =
            OrderByDirectionEnumeration.Ascending;

        private string GenericTypeName => typeof(OrderByClauseItemModel<TDbDto>).GenericTypeArguments[0].Name;

        private string ClassName => $"{nameof(OrderByClauseItemModel<TDbDto>)}<{GenericTypeName}>";

        /// <summary>
        /// Renders $"{PropertyName} {Direction.DisplayName}"
        /// </summary>
        /// <returns></returns>
        public override string ToString() =>
            $"{PropertyName} {Direction.DisplayName}";
    }
}
