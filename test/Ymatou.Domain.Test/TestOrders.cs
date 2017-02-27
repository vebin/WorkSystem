using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using YmtSystem.CrossCutting;
using YmtSystem.Domain.Shard;


namespace YmtSystem.Domain.Test
{
    [TestFixture]
    public class TestOrders
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            YmatouFramework.Start();
        }
        [TestFixtureTearDown]
        public void Stop()
        {
            YmatouFramework.Stop();
        }

        [Test]
        public void ValidatorOrder()
        {
            var order = new Order("a");

            order.Name = "";
            order.Price = 50;
            order.Status = 1;



            Assert.AreEqual(false, order.Validator().Success);
            Console.WriteLine("输出验证结果消息：");
            order.Validator().ResultData.Message.Each(Console.WriteLine);
        }


    }
}
