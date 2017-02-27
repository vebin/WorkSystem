using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Ymt.Trading.Domain.Model.Table;
using YmtSystem.CrossCutting;
using YmtSystem.Domain.Shard;
namespace YmtSystem.Domain.Test
{
    [TestFixture]
    public class EntityTest
    {
        [Test]
        public void IdTest()
        {
            var entity = new Entity1("5");
          
            var entity2 = new Entity1("12");
          
            Assert.AreEqual(false, entity.Equals(entity2));
        }
        [Test]
        public void Equal_Entity2()
        {
            var entity = new Entity2("12");
            entity.EId = 12;
            entity.Name = "a";

            var entity2 = new Entity2("12");
            entity2.EId = 12;
            entity2.Name = "a";
            Assert.True(entity2.Validator().Success);
            Assert.True(entity.Equals(entity2));

            var entity3 = new Entity2("12");
            entity3.EId = 12;
            entity3.Name = "1a";

            Assert.False(entity.Equals(entity3));
        }
    }
}
