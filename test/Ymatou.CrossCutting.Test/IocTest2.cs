using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YmtSystem.CrossCutting;
using NUnit.Framework;
using Microsoft.Practices.Unity;

namespace Ymatou.CrossCutting.Test
{

    [TestFixture]
    public class IocTest2
    {
        public IocTest2() { }
        [Dependency]
        private IA _ia { get; set; }
        //public IocTest2(IA ia) 
        //{
        //    _ia = ia;
        //}

        [TestFixtureSetUp]
        public void Setup()
        {
            YmatouFramework.Start();
        }
        [Test]
        public void S() 
        {
            _ia.S("sssssssss");
        }
    }

    public interface IA 
    {
        void S(string s);
    }

    public class AImp:IA 
    {
        public void S(string s) 
        {
            Console.WriteLine(s);
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
            container.RegisterType<IA, AImp>();
            container.RegisterType<IocTest2, IocTest2>();
            container.Resolve<IA>();
            return TaskContinuation.Continue;
        }
    }
}
