namespace System
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class GeneralExtensions_lg
    {
        public static void EachHandle<T>(this IEnumerable<T> e, Action<T> handle)
        {
            foreach (var item in e)
            {
                handle(item);
            }
        }
    }
}
