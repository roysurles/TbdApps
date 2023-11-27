namespace Tbd.Shared.Extensions;

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

    public static void InsertRange<T>(this ObservableCollection<T> observableCollection, int startIndex, IEnumerable<T> items)
    {
        if (startIndex < 0)
            return;

        if (items?.Any() == true)
        {
            var index = startIndex;
            foreach (var item in items)
                observableCollection.Insert(index++, item);
        }
    }

    public static void RemoveRange<T>(this ObservableCollection<T> observableCollection, int startIndex, int count)
    {
        if (!observableCollection.Any())
            return;

        if (startIndex < 0 || startIndex >= observableCollection.Count)
            return;

        if (count == 0 || startIndex + count > observableCollection.Count)
            return;

        for (int i = 0; i < count; i++)
            observableCollection.RemoveAt(startIndex);
    }
}
