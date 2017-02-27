using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YmtSystem.Infrastructure.Aop.Cache;
using YmtSystem.Infrastructure.Aop.Monitor;
using NUnit.Framework;
using YmtSystem.CrossCutting;
namespace YmtSystem.Infrastructure.Test
{

    public interface IAopTest
    {
        [CacheHandler(ProviderType = CacheProviderType.Local, TimeOut = 5)]
        [MethodRunTimeMonitorHandler(Order = 1)]        
        string Find(string val);
    }

    public class AopTest : IAopTest
    {
        public string Find(string val)
        {
            Console.WriteLine("{0}-{1}", DateTime.Now, val);
            return val;
        }
    }

    [TestFixture]
    public class AopTestImp
    {
        [SetUp]
        public void SetUp()
        {
            YmatouFramework.Start();
        }

        [TearDown]
        public void Stop()
        {
            YmatouFramework.Stop();
        }

        [Test]
        public void CacheTest()
        {
            LocalServiceLocator.GetService<IAopTest>().Find("aop...");
        }
    }
}
