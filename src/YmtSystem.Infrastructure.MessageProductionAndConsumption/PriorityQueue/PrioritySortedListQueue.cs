/**********************************************************
 * Description:优先队列（二项队列） SortedList实现
 * Author:lg
 * T:2012.8.28
 **********************************************************/

namespace YmtSystem.Infrastructure.MPAC.PQ
{
    using System;
    using System.Collections.Generic;
    using System.Linq;   
   
    public sealed class PrioritySortedListQueue<K, V> where K : IComparer<K>
    {
        private static readonly object lockObject = new object();
        private  SortedList<K, Queue<V>> list = new SortedList<K, Queue<V>>();      

        public int Count
        {
            get
            {
                lock (lockObject)
                {
                    return list.Sum(keyValuePair => keyValuePair.Value.Count);
                }
            }
        }

        public void Push(K priority, V item)
        {
            lock (lockObject)
            {
                if (!this.list.ContainsKey(priority))
                    this.list.Add(priority, new Queue<V>());
                this.list[priority].Enqueue(item);
            }
        }

        public V Dequeue()
        {
            lock (lockObject)
            {
                if (this.list.Count > 0)
                {
                    V obj = this.list.First().Value.Dequeue();
                    if (this.list.First().Value.Count == 0)
                        this.list.Remove(this.list.First().Key);
                    return obj;
                }
            }
            return default(V);
        }

        public V PopPriority(K priority)
        {
            lock (lockObject)
            {
                if (this.list.ContainsKey(priority))
                {
                    V obj = this.list[priority].Dequeue();
                    if (this.list[priority].Count == 0)
                        this.list.Remove(priority);
                    return obj;
                }
            }
            return default(V);
        }

        public IEnumerable<V> PopAllPriority(K priority)
        {
            List<V> ret = new List<V>();
            lock (lockObject)
            {
                if (this.list.ContainsKey(priority))
                {
                    while (this.list.ContainsKey(priority) && this.list[priority].Count > 0)
                        ret.Add(PopPriority(priority));
                    return ret;
                }
            }
            return ret;
        }

        public V Peek()
        {
            lock (lockObject)
            {
                if (this.list.Count > 0)
                    return this.list.First().Value.Peek();
            }
            return default(V);
        }

        public IEnumerable<V> PeekAll()
        {
            List<V> ret = new List<V>();
            lock (lockObject)
            {
                foreach (KeyValuePair<K, Queue<V>> keyValuePair in list)
                    ret.AddRange(keyValuePair.Value.AsEnumerable());
            }
            return ret;
        }

        public IEnumerable<V> DequeueAll()
        {
            List<V> ret = new List<V>();
            lock (lockObject)
            {
                while (this.list.Count > 0)
                    ret.Add(Dequeue());
            }
            return ret;
        }
    }
}
