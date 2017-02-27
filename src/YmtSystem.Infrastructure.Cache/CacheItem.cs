using System;

namespace YmtSystem.Infrastructure.Cache
{
    internal class CacheItem<TValue>
    {
        public TValue Value { get; set; }
        public DateTime Expired { get; set; }
    }
}
