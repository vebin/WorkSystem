using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.Practices.Unity;
using YmtSystem.CrossCutting;

namespace YmtSystem.Infrastructure.Aop.Cache
{
    public class CacheHandlerAttribute : HandlerAttribute
    {
        private static readonly Dictionary<CacheProviderType, ICacheCallHandlerWrapper> dic = new Dictionary<CacheProviderType, ICacheCallHandlerWrapper>(3) 
        {
            {CacheProviderType.Local,new LocalCacheCallHandler()}
        };
        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            return dic[this.ProviderType].Init(this.Key, this.TimeOut, this.Order);
        }

        public string Key { get; set; }
        public int TimeOut { get; set; }
        public CacheProviderType ProviderType { get; set; }
    }
}
