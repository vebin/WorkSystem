using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using NUnit.Framework;
using YmtSystem.Infrastructure.MPAC.Reactive;

namespace YmtSystem.Infrastructure.Test
{
    [TestFixture]
    public class RxxTest
    {
        [Test]
        public void RxxQueue()
        {
            RxxQueue<int>.Instance.Production(1)
                .Production(2).Production(3).Production(4).Production(5)
                .Consumption(TimeSpan.FromMilliseconds(1),e => Console.WriteLine(e));
        }
        [Test]
        public void LocalServiceLocatorTest() 
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterType<IA, A>();
            var ia= container.Resolve<IA>();
            ia.Show();
        }
    }
    public interface IA 
    {
        void Show();
    }

    public class A : IA 
    {
        int a;
        public A() 
        {
            Console.WriteLine("执行了。。。{0}",a++);
        }
        public void Show() {

            Console.WriteLine("sss");
        }
    }
}
