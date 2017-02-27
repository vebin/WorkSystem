namespace System.Collections.Generic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;

    public static class EnumerableExtensions
    {
        [DebuggerStepThrough]
        public static void Each<T>(this IEnumerable<T> instance, Action<T> action, bool parallel = false, Action<Exception> errorHandler = null)
        {
            if (instance == null) return;
            if (parallel)
            {
                Parallel.ForEach(instance, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, action);
            }
            else
            {
                if (action != null)
                {
                    foreach (T item in instance)
                    {
                        try
                        {
                            action(item);
                        }
                        catch (Exception ex)
                        {
                            if (errorHandler != null)
                                errorHandler(ex);
                            else
                                throw;
                        }
                    }
                }
            }
        }
        [DebuggerStepThrough]
        public static void TryEach<T>(this IEnumerable<T> instance, Action<T> action, bool parallel = false, bool exceptionBreak = false, Action<Exception> handle = null)
        {
            if (instance == null) return;
            if (parallel)
            {
                Parallel.ForEach(instance, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, e =>
                {
                    try { action(e); }
                    catch (Exception ex)
                    {
                        if (handle != null) handle(ex);
                        if (exceptionBreak) throw new AggregateException(ex);
                    }
                });
            }
            else
            {
                foreach (T item in instance)
                {
                    try
                    {
                        action(item);
                    }
                    catch (Exception ex)
                    {
                        if (handle != null) handle(ex);
                        if (exceptionBreak) throw;
                    }
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

        public static string EnumerableItemToMd5(this IEnumerable args, string defVal = "")
        {
            if (args == null) return defVal;
            var str = new StringBuilder();
            foreach (var item in args)
            {
                str.Append(item.ToString());
            }
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] by = md5.ComputeHash(Encoding.UTF8.GetBytes(str.ToString()));
            md5.Clear();
            return BitConverter.ToString(by).Replace("-", "").ToLower();
        }

        public static void SequenceEqual<T>(this IEnumerable<T> val, IEnumerable<T> seed, Action action, bool notQqualThrowOut = false, Exception ex = null)
        {
            var result = val.SequenceEqual(seed);
            if (!result && notQqualThrowOut)
                throw ex;
            else if (!notQqualThrowOut && action != null)
                action();
        }
    }
}
