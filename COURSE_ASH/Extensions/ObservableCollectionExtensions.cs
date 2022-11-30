using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COURSE_ASH.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static void SortBy<TSource,TKey>(this ObservableCollection<TSource> collection, Func<TSource,TKey> keySelector)
        {
            var tempList = collection.OrderBy(keySelector).ToList();
            collection.Clear();
            foreach (var value in tempList)
            {
                collection.Add(value);
            }
        }

        public static void FilterBy<TSource,TKey>(this ObservableCollection<TSource> collection, Func<TSource,bool> keySelector)
        {
            var tempList = collection.Where(keySelector).ToList();
            foreach (var value in tempList)
            {
                collection.Add(value);
            }
        }


    }
}
