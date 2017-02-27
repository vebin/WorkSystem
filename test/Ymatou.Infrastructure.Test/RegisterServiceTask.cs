using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YmtSystem.CrossCutting;

namespace YmtSystem.Infrastructure.Test
{
    public class RegisterServiceTask : RegisterServiceBootstrapperTask
    {

        public RegisterServiceTask(IUnityContainer container) : base(container) { }

        public override TaskContinuation Execute()
        {
            Console.WriteLine("执行注入1");
            container
                .AddNewExtension<Interception>()
                .RegisterTypeAsSingleton<IAopTest, AopTest>()
                .Configure<Interception>()
                .SetInterceptorFor<IAopTest>(new InterfaceInterceptor());

           

            return TaskContinuation.Continue;

        }

        protected override void InternalDispose()
        {
            Console.WriteLine("开始清理");
            base.InternalDispose();
        }
    }
}
