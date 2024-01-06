using CommunityToolkit.Mvvm.Collections;

namespace Contacts.Extensions;


public static class ObservableGroupedCollectionExtensions
{

    /// <summary>
    /// Finds an item in an ObservableGroupedCollection.
    /// </summary>
    /// <typeparam name="TKey">The type of the key in the ObservableGroupedCollection.</typeparam>
    /// <typeparam name="TElement">The type of the elements in the ObservableGroupedCollection.</typeparam>
    /// <param name="ogc">The ObservableGroupedCollection to search in.</param>
    /// <param name="lefTElement">The item to find in the collection.</param>
    /// <returns>The first element that matches the specified item, or null if no such element is found.</returns>
    public static TElement? FindItem<TKey, TElement>(
            this ObservableGroupedCollection<TKey, TElement> ogc, TElement lefTElement)
    where TKey : notnull
    where TElement : class, IComparable<TElement>
    {
        var ogcAsEnumerable = (IEnumerable<ObservableGroup<TKey, TElement>>)ogc;
        return ogcAsEnumerable.SelectMany(group => group)
                  .FirstOrDefault(rightElement => rightElement.CompareTo(lefTElement) == 0);
    }

    /// <summary>
    /// Adds the items from the specified collection to the ObservableGroupedCollection.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the ObservableGroupedCollection.</typeparam>
    /// <typeparam name="TElement">The type of the elements in the ObservableGroupedCollection.</typeparam>
    /// <param name="ogc">The ObservableGroupedCollection<TKey, TElement> to add the items to.</param>
    /// <param name="tempFiltered">The collection of items to add to the ObservableGroupedCollection<TKey, TElement>.</param>
    /// <param name="keyComparer">The Comparer<TKey> implementation to use when comparing keys.</param>
    /// <param name="itemComparer">The Comparer<TElement> implementation to use when comparing items.</param>
    /// <param name="createKey">A function that creates a key from an item in the collection.</param>
    /// <exception cref="ArgumentNullException">Thrown when ogc, tempFiltered, keyComparer, itemComparer, or createKey is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when an item with the same key already exists in the ObservableGroupedCollection.</exception>
    /// <exception cref="ArgumentException">Thrown when createKey returns a null value.</exception>
    public static void AddItems<TKey, TElement>(this ObservableGroupedCollection<TKey, TElement> ogc,
        IEnumerable<TElement> tempFiltered, Comparer<TKey> keyComparer,
        Comparer<TElement> itemComparer, Func<TElement, TKey> createKey)
        where TKey : notnull
        where TElement : class, IComparable<TElement>

    {
        foreach (var contact in tempFiltered)
        {
            if (ogc.FindItem(contact) is null)
            {
                var key = createKey(contact);
                ogc.InsertItem(key, keyComparer, contact, itemComparer);
            }
        }
    }

    /// <summary>
    /// Removes items from an ObservableGroupedCollection based on a filter.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys used in the ObservableGroupedCollection. TKey must be a non-null type.</typeparam>
    /// <typeparam name="TElement">The type of the elements stored in the ObservableGroupedCollection. TElement must be a reference type.</typeparam>
    /// <param name="ogc">The ObservableGroupedCollection from which to remove items.</param>
    /// <param name="tempFiltered">The collection of items to use as a filter for removal. Only items present in tempFiltered will be removed from ogc.</param>
    public static void RemoveItems<TKey, TElement>(this ObservableGroupedCollection<TKey, TElement> ogc, IEnumerable<TElement> tempFiltered)
        where TKey : notnull
        where TElement : class
    {
        for (int i = ogc.Count - 1; i >= 0; i--)
        {
            ObservableGroup<TKey, TElement> observableGroup = ogc[i];
            foreach (TElement contact in observableGroup.Reverse())
            {
                if (!tempFiltered.Contains(contact))
                {
                    observableGroup.Remove(contact);

                }
            }
            if (observableGroup.Count == 0)
            {
                ogc.Remove(observableGroup);
            }
        }
    }
}

