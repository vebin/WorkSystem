
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace System
{
    public static class TypeExtensions
    {
        [DebuggerStepThrough]
        public static IEnumerable<Type> PublicTypes(this Assembly instance)
        {
            IEnumerable<Type> types = null;

            if (instance != null)
            {
                try
                {
                    types = instance.GetTypes().Where(type => (type != null) && type.IsPublic && type.IsVisible).ToList();
                }
                catch (ReflectionTypeLoadException e)
                {
                    types = e.Types;
                }
            }

            return types ?? Enumerable.Empty<Type>();
        }

        [DebuggerStepThrough]
        public static IEnumerable<Type> PublicTypes(this IEnumerable<Assembly> instance)
        {
            return (instance == null) ?
                   Enumerable.Empty<Type>() :
                   instance.SelectMany(assembly => assembly.PublicTypes());
        }

        [DebuggerStepThrough]
        public static IEnumerable<Type> ConcreteTypes(this Assembly instance)
        {
            return (instance == null) ?
                   Enumerable.Empty<Type>() :
                   instance.PublicTypes()
                           .Where(type => (type != null) && type.IsClass && !type.IsAbstract && !type.IsInterface && !type.IsGenericType).ToList();
        }

        [DebuggerStepThrough]
        public static IEnumerable<Type> ConcreteTypes(this IEnumerable<Assembly> instance)
        {
            return (instance == null) ?
                   Enumerable.Empty<Type>() :
                   instance.SelectMany(assembly => assembly.ConcreteTypes());
        }

        [DebuggerStepThrough]
        public static T VerifyAndReturn<T>(this T val, Func<T, bool> via, T defVal = default(T), bool notViaThrowOut = false) where T : class
        {
            if (val == null)
            {
                return defVal;
            }
            var tmpVia = via(val);
            if (tmpVia) return val;
            if (notViaThrowOut) throw new ArgumentException("verify fail");
            else return defVal;
        }
    }
}

