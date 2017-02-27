using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

using YmtSystem.CrossCutting;
namespace YmtSystem.Infrastructure.Test
{
    [TestFixture]
    public class IocTest
    {
        [TestFixtureSetUp]
        public void Start()
        {
            YmatouFramework.Start(); 
        }

        [Test]
        public void Show()
        {
          
            LocalServiceLocator.GetService<ITest>().Show("hell ...");
        }

        [TestFixtureTearDown]
        public void End()
        {
            YmatouFramework.Stop();
        }
    }
}
