using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using YmtSystem.Infrastructure.Container.Unity.Bootstrapper;

namespace YmtSystem.Infrastructure.Container.Unity.Bootstrapper.Test
{
    [TestFixture]
    public class UnityTest
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            YmatouBootstrapperFramework.Start();
        }
        [Test]
        public void Test1()
        {
            LocalServiceLocator.GetService<IA>().Show();
        }
        [TestFixtureTearDown]
        public void End()
        {
            YmatouBootstrapperFramework.Stop();
        }
    }
}
