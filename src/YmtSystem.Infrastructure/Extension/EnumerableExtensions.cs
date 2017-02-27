namespace Ymatou.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    public static class EnumerableExtensions
    {
        [DebuggerStepThrough]
        public static void Each<T>(this IEnumerable<T> instance, Action<T> action)
        {
            if (instance != null)
            {
                foreach (T item in instance)
                {
                    action(item);
                }
            }
        }
        public static bool IsEmpty<T>(this IEnumerable<T> list, bool isEmptyThrowOut = false)
        {
            if (list == null || !list.Any() || list.Count() <= 0)
            {
                if (isEmptyThrowOut) throw new ArgumentNullException("list is null");
                return true;
            }
            return false;
        }

        public static IEnumerable<T> GetWhere<T>(this IEnumerable<T> list, IEnumerable<T> filter, Func<T, T, bool> fn)
        {
            if (filter.IsEmpty()) return list;
            var tmpList = new List<T>();
            foreach (var item in list)
            {
                foreach (var filerItem in filter)
                {
                    var value = fn(item, filerItem);
                    if (value)
                        tmpList.Add(item);
                }
            }
            return tmpList;
        }

        public static IEnumerable<T> AddRange2<T>(this IEnumerable<T> list, IEnumerable<T> rangeList)
        {
            if (list.IsEmpty()) return null;
            if (rangeList.IsEmpty()) return list;
            var tmpList = list.ToList();

            foreach (var item in rangeList)
            {
                //if (item!=null && !item.Equals(default(T)))
                tmpList.Add(item);
            }
            return tmpList;
        }
        //[DebuggerStepThrough]
        //public static void ParallelEach<T>(this IEnumerable<T> instance, Action<T> action)
        //{
        //    if (instance != null)
        //    {
        //        Parallel.ForEach(instance, item => action(item));
        //    }
        //}
    }
}
