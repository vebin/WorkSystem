using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YmtSystem.CrossCutting;

namespace YmtSystem.Infrastructure.Test
{
    public interface ITest
    {
        //[CachedHandler(CachedNodeName = "", Key = "12", TimeOut = 12)]
        void Show(string val);
    }

    public class TestImpl : ITest
    {
        public void Show(string val)
        {
            Console.WriteLine("{0}-{1}", DateTime.Now, val);
        }
    }

    public class TestTask : RegisterServiceBootstrapperTask
    {
        public TestTask(IUnityContainer container) : base(container) { }
        public override TaskContinuation Execute()
        {
            container.RegisterInstance<ITest>(new TestImpl());
            return TaskContinuation.Continue;
        }
    }
}
