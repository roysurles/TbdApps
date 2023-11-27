namespace Tbd.Shared.Extensions;

public static class IEnumerableExtensions
{
    public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> items) =>
        new(items);

    public static IEnumerable<T> Flatten<T>(this IEnumerable<T> e, Func<T, IEnumerable<T>> f) =>
        e.SelectMany(c => f(c).Flatten(f)).Concat(e);

    public static bool IsFirstItem<T>(this IEnumerable<T> items, T item) =>
        items.Any() && items.ToList().IndexOf(item).Equals(0);

    public static bool IsLastItem<T>(this IEnumerable<T> items, T item) =>
        items.Any() && items.ToList().IndexOf(item).Equals(items.Count() - 1);

    public static T NextItem<T>(this IEnumerable<T> items, T item)
    {
        var list = items.ToList();
        return items.IsLastItem(item) ? item : list[list.IndexOf(item) + 1];
    }

    public static T PreviousItem<T>(this IEnumerable<T> items, T item)
    {
        var list = items.ToList();
        return items.IsFirstItem(item) ? item : list[list.IndexOf(item) - 1];
    }
}
