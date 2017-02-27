using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using YmtSystem.CrossCutting;
namespace YmtSystem.CrossCutting.Test
{
    [TestFixture]
    public class Log4netTest
    {
        [Test]
        public void Info() 
        {
            YmatouLoggingService.InitLogService();
            YmatouLoggingService.Debug("{0}", "test log4net debug");
            YmatouLoggingService.Info("{0}", "test log4net debug");
            YmatouLoggingService.Error("{0}", "test log4net debug");
        }
    }
}
