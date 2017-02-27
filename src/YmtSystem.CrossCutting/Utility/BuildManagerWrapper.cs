namespace YmtSystem.CrossCutting.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Web.Compilation;
    public class BuildManagerWrapper
    {
        private static readonly BuildManagerWrapper current = new BuildManagerWrapper();
        private IEnumerable<Assembly> referencedAssemblies;
        private IEnumerable<Type> publicTypes;
        private IEnumerable<Type> concreteTypes;

        public static BuildManagerWrapper Current
        {
            [DebuggerStepThrough]
            get
            {
                return current;
            }
        }

        public virtual IEnumerable<Assembly> Assemblies
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    var allAssemblies = new List<Assembly>();
                    //主意：使用nunit测试的时候， Assembly.GetEntryAssembly() 可能返回为空，如果IOC注入在.exe 程序中且使用nunit测试
                    //则无法获取到注入的对象，测试会抛出异常。
                    //
                    //MSDN:从非托管应用程序加载托管程序集后，GetEntryAssembly 方法可返回 null。 
                    //例如，如果一个非托管应用程序创建了使用 C# 编写的一个 COM 组件的实例，则从该 C# 组件调用 GetEntryAssembly 方法将返回 null，因为该进程的入口点是非托管代码而不是托管程序集
                    var path = AppDomain.CurrentDomain.BaseDirectory;
                    var exeAssembly = Assembly.GetEntryAssembly();
                    if (exeAssembly != null)
                        allAssemblies.Add(exeAssembly);
                    else
                    {
#if DEBUG
                        //解决.exe 环境下nunit测试Assembly.GetEntryAssembly() 为空问题
                        Directory.GetFiles(path, "*.exe").TryEach(e =>
                        {
                            allAssemblies.Add(Assembly.LoadFrom(e));
                        });
#endif
                    }
                    Directory.GetFiles(path, "*.dll").TryEach(e =>
                    {
                        allAssemblies.Add(Assembly.LoadFrom(e));
                    }, parallel: true
                    , handle: err =>
                    {
                        YmatouLoggingService.Error(err.ToString());
                    });
                    //
                    //var currentAss = AppDomain.CurrentDomain.GetAssemblies().Where(e => !e.GlobalAssemblyCache);
                    return referencedAssemblies ??
                        (referencedAssemblies = allAssemblies.Where(assembly =>
                            assembly != null
                            && !assembly.GlobalAssemblyCache)
                        .Distinct(new LambdaComparer<Assembly>((x, y) => x.FullName == y.FullName)));
                }
                else
                {
                    return referencedAssemblies ?? (referencedAssemblies = BuildManager.GetReferencedAssemblies().Cast<Assembly>().Where(assembly => !assembly.GlobalAssemblyCache).ToList());
                }
            }
        }

        public IEnumerable<Type> PublicTypes
        {
            get
            {
                return publicTypes ?? (publicTypes = Assemblies.PublicTypes());
            }
        }

        public IEnumerable<Type> ConcreteTypes
        {
            get
            {
                return concreteTypes ?? (concreteTypes = Assemblies.ConcreteTypes());
            }
        }
    }
}
