using CommunityToolkit.Mvvm.Collections;

namespace Contacts.Extensions;

//private void AddContacts(List<Contact> tempFiltered)
//{
//    foreach (Contact contact in tempFiltered)
//    {
//        string key = GetGroupName(contact);
//        var group = ContactsDataSource.FirstGroupByKeyOrDefault(key);

//        if (group != null && !group.Contains(contact))
//        {
//            ContactsDataSource.AddItem(key, contact);
//        }
//        else if (group == null)
//        {
//            ContactsDataSource.InsertItem(
//               key: key,
//               keyComparer: Comparer<string>.Default,
//               item: contact,
//               itemComparer: Comparer<Contact>.Create(
//                   static (left, right) => Comparer<string>.Default.Compare(left.ToString(), right.ToString())));
//        }
//    }
//}

public static class ObservableGroupedCollectionExtensions
{
    public static void AddItemsNicu<TElement>(this ObservableGroupedCollection<string, TElement> collection,
         IEnumerable<TElement> items) 
    {
        foreach (TElement item in items)
        {
            string key = "key";
            var group = collection.FirstGroupByKeyOrDefault(key);

            if (group != null && !group.Contains(item))
            {
                collection.AddItem(key, item);
            }
            else if (group == null)
            {
                collection.InsertItem(
                   key: key,
                   keyComparer: Comparer<string>.Default,
                   item: item,
                   itemComparer: Comparer<TElement>.Create(
                       static (left, right) => Comparer<string>.Default.Compare(left.ToString(), right.ToString())));
            }
        }

    }

    private static string GetGroupName(Contact contact)
    {
        return contact.Name.First().ToString().ToUpper();
    }
    private static string GetGroupNameGeneric<T>(T obj)
    {
        if(obj is Contact contact)
        {
            return contact.Name.First().ToString().ToUpper();
        }
        
        return string.Empty;
    }
}


//private void RemoveContacts(List<Contact> tempFiltered)
//{
//    for (int i = ContactsDataSource.Count - 1; i >= 0; i--)
//    {
//        ObservableGroup<string, Contact> observableGroup = ContactsDataSource[i];
//        foreach (Contact contact in observableGroup.Reverse())
//        {
//            if (!tempFiltered.Contains(contact))
//            {
//                observableGroup.Remove(contact);

//            }
//        }
//        if (observableGroup.Count == 0)
//        {
//            ContactsDataSource.Remove(observableGroup);
//        }
//    }
//}