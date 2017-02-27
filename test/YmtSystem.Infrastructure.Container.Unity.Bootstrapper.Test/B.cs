using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using _BootstrapperTask = YmtSystem.Infrastructure.Container._Ninject.Bootstrapper.BootstrapperTask;
namespace YmtSystem.Infrastructure.Container.Unity.Bootstrapper.Test
{
    public interface IB
    {
        void Show();
    }

    public class B : IB
    {

        public void Show()
        {
            Console.WriteLine("bbb");
        }
    }
    //class ControllerModule : StandardModule {
    //  public override Load() {
    //    Bind<B>().ToSelf();

    //  }
    //}
    public class NinjectStartTask : _BootstrapperTask
    {
        //public NinjectStartTask(IKernel kernel) : base(kernel) { }

        public override _Ninject.Bootstrapper.TaskContinuation Execute()
        {
            Bind<IB>().To<B>().InTransientScope();
            return _Ninject.Bootstrapper.TaskContinuation.Continue;
        }
    }
}
