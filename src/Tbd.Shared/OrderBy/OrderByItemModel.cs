using System.ComponentModel.DataAnnotations;

namespace Tbd.Shared.OrderBy
{
    /// <summary>
    /// Represents an item in the Order By clause
    /// </summary>
    public class OrderByItemModel
    {
        /// <summary>
        /// ctor
        /// </summary>
        public OrderByItemModel() { }

        /// <summary>
        /// ctor with parameters
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="direction"></param>
        /// <param name="position"></param>
        public OrderByItemModel(string propertyName, OrderByDirectionEnumeration direction, int position)
        {
            PropertyName = propertyName;
            Direction = direction;
            Position = position;
        }

        [Required]
        public string PropertyName { get; set; }

        public OrderByDirectionEnumeration Direction { get; set; } =
            OrderByDirectionEnumeration.Ascending;

        public int Position { get; set; }

        /// <summary>
        /// Renders $"{PropertyName} {Direction.DisplayName}"
        /// </summary>
        /// <returns></returns>
        public override string ToString() =>
            $"{PropertyName} {Direction.DisplayName}";
    }
}
