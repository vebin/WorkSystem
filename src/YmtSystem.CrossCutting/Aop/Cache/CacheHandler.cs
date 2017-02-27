namespace YmtSystem.CrossCutting.Aop.Cache
{
    using System;
    using Microsoft.Practices.Unity.InterceptionExtension;
    using YmtSystem.CrossCutting;

    public class CacheHandler : ICallHandler
    {
        public CacheHandler(string key, int timeOut, string cachedNodeName)
        {
            this.Key = key;
            this.TimeOut = timeOut;
            this.CachedNodeName = cachedNodeName;
        }

        public int Order { get; set; }
        public string Key { get; set; }
        public int TimeOut { get; set; }
        public string CachedNodeName { get; set; }
        private IMethodReturn tmpReturnVal;

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {

            if (Key.IsEmpty())
                Key = input.Arguments.EnumerableItemToMd5();
            // MemcachedCached.Instance.Get<object>(Key, CachedNodeName);
            Console.WriteLine("开始拦截...");
            var returnVal = new object();
            returnVal = null;
            if (returnVal == null)
            {
                try
                {
                    //MemcachedCached.Instance.AsyncStore(Key, tmpReturnVal.ReturnValue, TimeOut, CachedNodeName);
                    tmpReturnVal = getNext()(input, getNext);
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
    }
}
