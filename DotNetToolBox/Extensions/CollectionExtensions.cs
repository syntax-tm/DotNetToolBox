using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DotNetToolBox.Extensions
{
    public static class CollectionExtensions
    {

        /// <summary>
        /// Converts the source <paramref name="collection&lt;T&gt;"/> to an <see cref="ObservableCollection&lt;T&gt;"/>.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of item contained in the <see cref="IEnumerable&lt;T&gt;"/> source <paramref name="collection"/></typeparam>
        /// <param name="collection">The source <see cref="IEnumerable&lt;T&gt;"/> collection of items to add to the <see cref="ObservableCollection&lt;T&gt;"/></param>
        /// <returns>An <see cref="ObservableCollection&lt;T&gt;"/> containing the items from the <see cref="IEnumerable&lt;T&gt;"/> source.</returns>
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            var itemList = collection.ToList();
            if (!itemList.Any()) return new ObservableCollection<T>();
            return new ObservableCollection<T>(itemList);
        }

        public static string OxbridgeAnd(this IEnumerable<string> collection)
        {
            var list = collection?.ToList();
            if (list == null || !list.Any()) return string.Empty;
            
            //  item 1, item 2, and item 3
            if (list.Count >= 3)
            {
                var delimited = string.Join(", ", list.Take(list.Count - 1));
                var output = string.Concat(delimited, ", and ", list.LastOrDefault());
                return output;
            }

            //  item 1 and item 2
            if (list.Count == 2)
            {
                var delimited = string.Join(" and ", list);
                return delimited;
            }

            //  item 1
            return list.First();
        }

        public static bool ContainsIgnoreCase(this IEnumerable<string> collection, string value)
        {
            var collectionList = collection.ToList();
            return collectionList.Any(item => item.EqualsIgnoreCase(value));
        }

    }
}
