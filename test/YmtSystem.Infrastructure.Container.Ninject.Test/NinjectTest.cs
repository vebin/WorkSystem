using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LocalServiceLocator2 = YmtSystem.Infrastructure.Container._Ninject.Bootstrapper.LocalServiceLocator;
using YmatouBootstrapperFramework2 = YmtSystem.Infrastructure.Container._Ninject.Bootstrapper.YmatouBootstrapperFramework;
using Ninject;
using NUnit.Framework;
using Ninject.Modules;

namespace YmtSystem.Infrastructure.Container.Unity.Bootstrapper.Test
{

    [TestFixture]
    public class NinjectTest
    {
        //private static readonly IKernel _Kernel = new StandardKernel(/*new FirstModule()*/);
        [TestFixtureSetUp]
        public void Setup()
        {
            YmatouBootstrapperFramework2.Start();
        }
        [Test]
        public void Show()
        {
            //_Kernel.Bind<IB>().To<B>();
            //_Kernel.Get<IB>().Show();
            //var person = _Kernel.Get<IPerson>();
            //Console.WriteLine(person);
            LocalServiceLocator2.GetService<IB>().Show();
            //YmatouBootstrapperFramework2.Stop();
        }
        [TestFixtureTearDown]
        public void End() 
        {
            YmatouBootstrapperFramework2.Stop();
        }
    }

    public interface IPerson
    {
    }
    public class ZhangFei : IPerson
    {
        public override string ToString()
        {
            return "I am 张飞.";
        }
    }
    public class GuanYu : IPerson
    {
        public override string ToString()
        {
            return "I am 关羽.";
        }
    }
    public class Simpkan : IPerson
    {
        public override string ToString()
        {
            return "I am Simpkan.";
        }
    }
    //class FirstModule : NinjectModule
    //{
    //    public override void Load()
    //    {
    //        Bind<IPerson>().To<ZhangFei>();
    //    }
    //}
    
}
