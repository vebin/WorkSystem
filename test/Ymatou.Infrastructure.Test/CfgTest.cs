using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NUnit.Framework;
using YmtSystem.Infrastructure.Config;
using YmtSystem.CrossCutting;
using YmtSystem.Repository.EF;
namespace YmtSystem.Infrastructure.Test
{
    [TestFixture]
    public class CfgTest
    {
        [SetUp]
        public void SetUp()
        {
            //YmatouFramework.Start();
        }

        [TearDown]
        public void Stop()
        {
            //YmatouFramework.Stop();
        }

        [Test]
        public void FilePathTest()
        {
            Console.WriteLine(Path.IsPathRooted("D:\\a.json"));
            Assert.True(Path.IsPathRooted("D:\\a.json"));
            Assert.False(Path.IsPathRooted("a.txt"));
        }
        [Test]
        public void GetJsonValueTest()
        {
            var v = "db.cfg".GetLocalCfgValueByFile<IDictionary<string,DbConfigure>>(ValueFormart.JSON);
            Assert.AreEqual(DbContextLifeScope.New, v["default"].Scope);
            Assert.AreEqual(DbContextLifeScope.New, v["YmtUserContext"].Scope);
        }
        [Test]
        public void Save_DbCfg()
        {
            //DbConfigure.Save(DbConfigure.DefVal);
            //DbConfigure.Save(new Dictionary<string, DbConfigure> { { "default", DbConfigure.DefVal }, { "default2", DbConfigure.DefVal } });
        }
        [Test]
        public void Load_DbCfg()
        {
            var cfg = DbConfigure.GetConfigure("YmtUserContext");
            var conn = @"Data Source=YMT-LIGUO\LIGUO;Initial Catalog=Ymt_Test_7;Persist Security Info=True;User ID=sa;Password=123@#$123asd";
            Assert.AreEqual(conn, cfg.Connection);
        }
    }

    public class T
    {
        public string A { get; set; }
        public int B { get; set; }
    }
   
}
