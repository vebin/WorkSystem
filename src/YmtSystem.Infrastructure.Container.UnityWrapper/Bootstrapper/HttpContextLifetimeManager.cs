namespace YmtSystem.Infrastructure.Container.UnityWrapper
{
    using Microsoft.Practices.Unity;
    using System;
    using System.Collections.Generic;
    using System.Web;

    public class HttpContextLifetimeManager<T> : HttpContextLifetimeManager, IDisposable
    {
    }

    public class HttpContextLifetimeManager : LifetimeManager, IDisposable
    {
        private readonly Guid key;

        public HttpContextLifetimeManager()
        {
            key = Guid.NewGuid();
        }

        public void Dispose()
        {
            RemoveValue();
        }

        public override object GetValue()
        {
            return HttpContext.Current.Items[key];
        }

        public override void RemoveValue()
        {
            HttpContext.Current.Items.Remove(key);
        }

        public override void SetValue(object newValue)
        {
            HttpContext.Current.Items[key]
                = newValue;
        }
    }

    //public class HttpContextLifetimeManager<T> : LifetimeManager, IDisposable
    //{
    //    public override object GetValue()
    //    {
    //        return HttpContext.Current.Items[typeof(T).AssemblyQualifiedName];
    //    }
    //    public override void RemoveValue()
    //    {
    //        HttpContext.Current.Items.Remove(typeof(T).AssemblyQualifiedName);
    //    }
    //    public override void SetValue(object newValue)
    //    {
    //        HttpContext.Current.Items[typeof(T).AssemblyQualifiedName] = newValue;
    //    }
    //    public void Dispose()
    //    {
    //        RemoveValue();
    //    }
    //}
}
