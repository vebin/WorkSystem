using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity.InterceptionExtension;
using YmtSystem.CrossCutting;
using YmtSystem.Infrastructure.Cache;

namespace YmtSystem.Infrastructure.Aop.Cache
{
    public class LocalCacheCallHandler : ICacheCallHandlerWrapper
    {
        //private static readonly LocalCache<string, object> cache = new LocalCache<string, object>(1024);

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            if (Key.IsEmpty())
                Key = input.Arguments.EnumerableItemToMd5();
            Console.WriteLine("Cache开始拦截...");
            var returnVal = LocalCache<string, object>.LocalCacheServer.Get(this.Key);

            if (returnVal == null)
            {
                try
                {
                    tmpReturnVal = getNext()(input, getNext);
                    LocalCache<string, object>.LocalCacheServer.Add(this.Key, tmpReturnVal, this.TimeOut);
                    return tmpReturnVal;
                }
                finally
                {

                }
            }
            else
            {
                var arguments = new object[input.Arguments.Count];
                input.Arguments.CopyTo(arguments, 0);
                return new VirtualMethodReturn(input, returnVal, arguments);
            }
        }
        private IMethodReturn tmpReturnVal;
        public string Key { get; private set; }
        public TimeSpan TimeOut { get; private set; }
        public int Order { get; set; }

        public ICallHandler Init(string key, int timeOut, int order = 0)
        {
            this.Key = key;
            this.TimeOut = TimeSpan.FromSeconds(timeOut);
            this.Order = order;
            return this;
        }
    }
}
