namespace YmtSystem.Infrastructure.Container._Ninject.Bootstrapper
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using Ninject;
    using Ninject.Syntax;

    public static class UnityContainerExtensions
    {
        [DebuggerStepThrough]
        public static IBindingNamedWithOrOnSyntax<object> RegisterTypeAsSingleton(this IKernel instance, Type fromType, Type toType)
        {
            lock (instance)
                return instance.Bind(fromType).To(toType).InSingletonScope();
        }
        [DebuggerStepThrough]
        public static IBindingWithOrOnSyntax<object> RegisterTypeAsSingleton(this IKernel instance, Type fromType, Type toType, string name)
        {
            lock (instance)
                return instance.Bind(fromType).To(toType).InSingletonScope().Named(name);
        }      
    }
}
