using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using YmtSystem.CrossCutting;

namespace YmtSystem.Infrastructure.Cache
{
    public class LocalCache<TKey, TValue>
    {
        private static readonly Lazy<LocalCache<TKey, TValue>> lazy = new Lazy<LocalCache<TKey, TValue>>(() => new LocalCache<TKey, TValue>());
        private static readonly object @lock = new object();
        private ConcurrentDictionary<TKey, CacheItem<TValue>> cache;
        private static int start = 0;
        private static Timer timer;

        public static LocalCache<TKey, TValue> LocalCacheServer
        {
            get
            {
                return lazy.Value;
            }
        }

        private LocalCache(int maxCount = 1024)
        {
            if (maxCount <= 0) throw new Exception<CacheException>("缓存项必须大于零");
            if (maxCount >= int.MaxValue) throw new Exception<CacheException>("缓存项不能大于等于int.MaxValue");
            this.MaxCount = maxCount * Environment.ProcessorCount * 10;
            cache = new ConcurrentDictionary<TKey, CacheItem<TValue>>(Environment.ProcessorCount, this.MaxCount);
            //RemoveExpiredTask();
        }

        private void RemoveExpiredTask()
        {
            if (start == 1) return;
            YmatouLoggingService.Debug("注册自动移除缓存项task");
            timer = new System.Threading.Timer(_ =>
            {
                var _cache = _ as ConcurrentDictionary<TKey, CacheItem<TValue>>;
                if (_cache == null) return;
                var expiredKey = _cache.AsParallel().Where(e => CheckExpired(e.Value.Expired)).Select(k => k.Key);
                //YmatouLoggingService.Debug("过期key , {0}", string.Join<TKey>(",", expiredKey.ToArray()));
                expiredKey.Each(k =>
                {
                    CacheItem<TValue> val;
                    _cache.TryRemove(k, out val);
                });

            }, cache, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(10));
            //timer.Change(System.Threading.Timeout.Infinite, 10000);
            System.Threading.Interlocked.CompareExchange(ref start, 1, 0);
        }

        public LocalCache<TKey, TValue> Add(TKey key, TValue value, TimeSpan expiredTs = default(TimeSpan), bool failThrowOut = false)
        {
            if (CacheItemCount >= MaxCount) return this;
            var val = new CacheItem<TValue> { Value = value, Expired = SetExpired(expiredTs) };
            var result = cache.TryAdd(key, val);
            if (!result && failThrowOut) throw new Exception<CacheException>();
            return this;
        }

        public async Task<LocalCache<TKey, TValue>> AsyncAdd(TKey key, TValue value, TimeSpan expiredTs = default(TimeSpan), bool failThrowOut = false)
        {
            if (CacheItemCount >= MaxCount) return this;
            var val = new CacheItem<TValue> { Value = value, Expired = SetExpired(expiredTs) };
            return await Task.Factory.StartNew(
                  () =>
                  {
                      var result = cache.TryAdd(key, val);
                      if (!result && failThrowOut) throw new Exception<CacheException>();
                      return this;
                  });
        }

        public LocalCache<TKey, TValue> Set(TKey key, TValue value, TimeSpan expiredTs, bool failThrowOut = false)
        {
            if (CacheItemCount >= MaxCount) return this;
            var val = new CacheItem<TValue> { Value = value, Expired = SetExpired(expiredTs) };
            cache.AddOrUpdate(key, val, (k, v) =>
            {
                CacheItem<TValue> item = null;
                Interlocked.CompareExchange(ref item, new CacheItem<TValue> { Value = value, Expired = SetExpired(expiredTs) }, null);
                return item;
            });
            return this;
        }

        public async Task<LocalCache<TKey, TValue>> AsyncSet(TKey key, TValue value, TimeSpan expiredTs, bool failThrowOut = false)
        {
            if (CacheItemCount >= MaxCount) return this;
            var val = new CacheItem<TValue> { Value = value, Expired = SetExpired(expiredTs) };
            return await Task.Factory.StartNew(() =>
             {
                 //cache.AddOrUpdate(key, val, (k, v) => val);
                 cache.AddOrUpdate(key, val, (k, v) =>
                 {
                     CacheItem<TValue> item = null;
                     Interlocked.CompareExchange(ref item, new CacheItem<TValue> { Value = value, Expired = SetExpired(expiredTs) }, null);
                     return item;
                 });
                 return this;
             });
        }

        public TValue Get(TKey key, TValue defVal = default(TValue))
        {
            CacheItem<TValue> val;
            cache.TryGetValue(key, out val);
            if (val == null) return defVal;
            var isExpired = CheckExpired(val.Expired);
            if (isExpired) return defVal;
            return val.Value;
        }

        public TValue GetOrAdd(TKey key, TimeSpan expiredTs, TValue defVal = default(TValue))
        {
            var result = cache.GetOrAdd(key, e =>
            {
                //解决可能出现的问题：http://msdn.microsoft.com/zh-cn/library/dd997369(v=vs.110).aspx
                CacheItem<TValue> item = null;
                Interlocked.CompareExchange(ref item, new CacheItem<TValue> { Value = defVal, Expired = SetExpired(expiredTs) }, null);
                //lock (@lock)
                //    return new CacheItem<TValue> { Value = defVal, Expired = CheckExpired(expiredTs) };
                return item;
            });
            var isExpired = CheckExpired(result.Expired);
            if (isExpired) return defVal;
            return result.Value;
        }

        public LocalCache<TKey, TValue> Remove(TKey key)
        {
            CacheItem<TValue> val;
            cache.TryRemove(key, out val);
            return this;
        }

        public TValue RemoveAndReturn(TKey key)
        {
            CacheItem<TValue> val;
            cache.TryRemove(key, out val);
            return val.Value;
        }

        private DateTime SetExpired(TimeSpan exTimeSpan)
        {
            if (exTimeSpan == default(TimeSpan))
                return DateTime.Now.Add(TimeSpan.FromSeconds(10));
            return DateTime.Now.Add(exTimeSpan);
        }
        /// <summary>
        /// 判断缓存项是否过期
        /// </summary>
        /// <param name="expiredTime">过期时间</param>
        /// <returns></returns>
        private bool CheckExpired(DateTime expiredTime)
        {
            return expiredTime <= DateTime.Now;
        }
        public int CacheItemCount { get { return cache.Count; } }
        public int MaxCount { get; set; }
    }
}
