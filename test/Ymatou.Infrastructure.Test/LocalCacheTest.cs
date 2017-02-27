using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using YmtSystem.Infrastructure.Cache;

namespace YmtSystem.Infrastructure.Test
{
    [TestFixture]
    public class LocalCacheTest
    {
        //public static readonly LocalCache<int, int> cache = new LocalCache<int, int>(100);
        [Test]
        public void AddItem()
        {
            for (var i = 1; i < 130; i++)
            {
                LocalCache<int, int>.LocalCacheServer.Set(i, i, TimeSpan.FromSeconds(i * 5));
            }
        }
        [Test]
        public async void AsyncAddItem()
        {
            for (var i = 0; i < 100; i++)
            {
                await LocalCache<int, int>.LocalCacheServer.AsyncAdd(i, i, TimeSpan.FromSeconds(3000));
            }
        }
        [Test]
        public void ItemCount()
        {
            var count = LocalCache<int, int>.LocalCacheServer.CacheItemCount;
            Assert.AreEqual(100, count);
        }
        [Test]
        public void Find()
        {
            for (var i = 1; i < 130; i++)
            {
                var result = LocalCache<int, int>.LocalCacheServer.Get(i, -1);
                Console.WriteLine(result);
            }
        }
    }
}
