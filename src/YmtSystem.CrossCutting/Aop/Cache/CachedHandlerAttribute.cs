namespace YmtSystem.CrossCutting.Aop.Cache
{
    using System;
    using Microsoft.Practices.Unity.InterceptionExtension;
    using Microsoft.Practices.Unity;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CachedHandlerAttribute : HandlerAttribute
    {

        public CachedHandlerAttribute()
        {

        }

        public override ICallHandler CreateHandler(IUnityContainer container)
        {      
            return new CacheHandler(Key, TimeOut, CachedNodeName);
        }

        public string Key { get; set; }
        public int TimeOut { get; set; }
        public string CachedNodeName { get; set; }
    }
}
