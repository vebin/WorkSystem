
using System;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace YmtSystem.Infrastructure.Aop.Cache
{
    public interface ICacheCallHandlerWrapper : ICallHandler
    {
        ICallHandler Init(string key, int timeOut, int order = 0);
    }
}
