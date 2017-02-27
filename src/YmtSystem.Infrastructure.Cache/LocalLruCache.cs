using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YmtSystem.Infrastructure.Cache
{
    internal class CacheItem<K, V>
    {
        public CacheItem(K k, V v)
        {
            UseCount = 0;
            Key = k;
            Value = v;
        }

        public int UseCount = 0;
        public K Key;
        public V Value;
    }

    //Hot and cold
    public class LocalLruCache<K, V>
    {
        private int maxSize;
        private int hot;
        private Dictionary<K, LinkedListNode<CacheItem<K, V>>> cacheDic;
        private LinkedList<CacheItem<K, V>> chacheList;
        private volatile bool init = false;

        public LocalLruCache(int maxSize, int hot = 10)
        {
            if (!init)
            {
                this.init = true;
                this.hot = hot;
                this.maxSize = maxSize;
                this.cacheDic = new Dictionary<K, LinkedListNode<CacheItem<K, V>>>(maxSize);
                this.chacheList = new LinkedList<CacheItem<K, V>>();
            }
        }

        public V Get(K key)
        {
            LinkedListNode<CacheItem<K, V>> node;
            if (cacheDic.TryGetValue(key, out node))//0(1)   
            {
                V value = node.Value.Value;
                Interlocked.Add(ref  node.Value.UseCount, 1);
                //node.Value.UseCount++;  //这里按需要可改为原子递增操作
                if (node.Value.UseCount >= hot && node.Next != null)
                {
                    lock (chacheList)
                    {
                        chacheList.Remove(node); //O(1)            
                        chacheList.AddLast(node); //O(1) 
                    }

                }
                return value;
            }
            return default(V);
        }

        public void Add(K key, V val)
        {
            if (cacheDic.Count >= maxSize)
            {
                RemoveOldItem();
            }
            CacheItem<K, V> cacheItem = new CacheItem<K, V>(key, val);
            LinkedListNode<CacheItem<K, V>> node = new LinkedListNode<CacheItem<K, V>>(cacheItem);
            chacheList.AddLast(node);//O(1)          
            cacheDic.Add(key, node);//0(1) -->O(n);
        }

        public void ClearAll()
        {
            chacheList.Clear();
            cacheDic.Clear();
            init = false;
        }

        private void RemoveOldItem()
        {
            lock (chacheList)
            {
                //移除老的数据 链表头部都为老的数据
                LinkedListNode<CacheItem<K, V>> node = chacheList.First;
                if (node == null) return;
                // chacheList.Remove(node);
                chacheList.RemoveFirst();
                cacheDic.Remove(node.Value.Key);
            }
        }
    }
}
