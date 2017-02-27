using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YmtSystem.CrossCutting;
using NUnit.Framework;
namespace YmtSystem.CrossCutting.Test
{
    public interface IIocTest
    {
        void Show();
        void Handler(Action handler = null);
    }
    public interface IAppService
    {
        IIocTest Ioc { get; set; }
    }
    public class IocTest2 : IIocTest
    {


        public IocTest2()
        {
            Console.WriteLine("被执行 IocTest2。。。{0}", System.Threading.Thread.CurrentThread.ManagedThreadId);
        }
        public void Show()
        {
            Console.WriteLine("显示..{0}", System.Threading.Thread.CurrentThread.ManagedThreadId);
        }
        public void Handler(Action handler = null)
        {
            if (handler != null)
                handler();
        }
    }
    public class IocTest : IIocTest
    {


        public IocTest()
        {
            Console.WriteLine("被执行 IocTest。。。{0}", System.Threading.Thread.CurrentThread.ManagedThreadId);
        }
        public void Show()
        {
            Console.WriteLine("显示..{0}", System.Threading.Thread.CurrentThread.ManagedThreadId);
        }
        public void Handler(Action handler = null)
        {
            if (handler != null)
                handler();
        }
    }

    public class IocTest3 : IAppService
    {
        [Dependency("B")]
        public IIocTest Ioc { get; set; }
    }
    [TestFixture]
    public class IocImpTest
    {
        public IocImpTest() { }
       [TestFixtureSetUp]
        public void Setup()
        {
            YmatouFramework.Start();
        }
        [Dependency]
        public IocTest3 IocTest { get; set; }
        [Test]
        public void ShowTest()
        {

            Console.WriteLine(IocTest == null);
            //var list = new List<Task>();
            //for (var i = 0; i < 10; i++)
            //{
            //    var task = Task.Factory.StartNew(() =>
            //      {
            var ioc2 = LocalServiceLocator.GetService<IAppService>();
            Console.WriteLine(ioc2.Ioc == null);
            var ioc = LocalServiceLocator.GetService<IIocTest>("B");
            //YmatouFramework.Container.BuildUp(ioc, "B");
            ioc.Show();
            ioc.Handler();
            //      });
            //    list.Add(task);
            //}

            //Task.WaitAll(list.ToArray());
        }
    }

    public class RegisterServiceTask : RegisterServiceBootstrapperTask
    {
        public RegisterServiceTask(IUnityContainer container)
            : base(container)
        {
        }

        public override TaskContinuation Execute()
        {
            //ContainerControlledLifetimeManager:
            //容器控制生命周期管理，这个生命周期管理器是RegisterInstance默认使用的生命周期管理器，
            //也就是单件实例，UnityContainer会维护一个对象实例的强引用，每次调用的时候都会返回同一对象
            // container.RegisterInstance<IIocTest>(new IocTest(), new ContainerControlledLifetimeManager());
            //
            //
            //PerResolveLifetimeManager:
            //这个生命周期是为了解决循环引用而重复引用的生命周期
            //第一调用的时候会创建一个新的对象，而再次通过循环引用访问到的时候就会返回先前创建的对象实例（单件实例）
            //container.RegisterInstance<IIocTest>(new IocTest(), new PerResolveLifetimeManager());
            //container.RegisterTypeAsPerResolve<IIocTest, IocTest>(); 
            //container.RegisterTypeAsSingleton<IIocTest, IocTest>();
            //RegisterType 生命周期为 TransientLifetimeManager
            container.RegisterType<IIocTest, IocTest>("A");
            container.RegisterType<IIocTest, IocTest2>("B");
            container.RegisterType<IAppService, IocTest3>();
            container.RegisterType<IocImpTest>(/*new InjectionProperty("Ioc")*/);

            //HierarchicalLifetimeManager:
            //
            //
            //container.RegisterInstance<IIocTest>(new IocTest(),new HierarchicalLifetimeManager());
            //container.RegisterInstanceAsTransient<IIocTest>(new IocTest());
            //container.RegisterTypeAsSingleton<IocTest, IocTest>();
            //每次 Resolve 都会调用 ITO的构造函数 生命周期为 TransientLifetimeManager
            //
            //
            //container.RegisterTypeAsTransient<IIocTest, IocTest>();
            //注册为单例 只调用一次ITO的构造函数 生命周期W为 ContainerControlledLifetimeManager
            // container.RegisterTypeAsSingleton<IIocTest, IocTest>();
            //container.RegisterTypeAsSingleton(typeof(IIocTest), typeof(IocTest), InjectionMember { });
            // container.RegisterType(typeof(IIocTest), typeof(IocTest), new ContainerControlledLifetimeManager());
            //PerThreadLifetimeManager，每线程生命周期管理器，就是保证每个线程返回同一实例
            return TaskContinuation.Continue;
        }
    }
}
