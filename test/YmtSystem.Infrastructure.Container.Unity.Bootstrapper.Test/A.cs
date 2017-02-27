using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YmtSystem.Infrastructure.Container.Unity.Bootstrapper;
using Microsoft.Practices.Unity;

namespace YmtSystem.Infrastructure.Container.Unity.Bootstrapper.Test
{
    public interface IA
    {
        void Show();
    }

    public class A : IA
    {
        public void Show()
        {
            Console.WriteLine("sss");
        }
    }

    public class UnityBootstrapper : BootstrapperTask
    {
        public UnityBootstrapper(IUnityContainer container) : base(container) { }

        public override TaskContinuation Execute()
        {
            container.RegisterType<IA, A>();
            return TaskContinuation.Continue;
        }
    }
}
