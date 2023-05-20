using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Tbd.Shared.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static void AddRange<T>(this ObservableCollection<T> observableCollection, IEnumerable<T> items)
        {
            if (items?.Any() == true)
            {
                foreach (var item in items)
                    observableCollection.Add(item);
            }
        }

        public static void AddRange<T>(this ICollection<T> observableCollection, IEnumerable<T> items)
        {
            if (items?.Any() == true)
            {
                foreach (var item in items)
                    observableCollection.Add(item);
            }
        }
    }
}
