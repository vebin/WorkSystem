using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using YmtSystem.Domain.Shard;
namespace YmtSystem.Domain.Test
{
    [TestFixture]
    public class ValueObjectTest
    {
        [Test]
        public void Comparable()
        {
            var a1 = new A();
            a1.AId = 1;
            a1.Name = "ss";

            var a2 = new A();
            a2.AId = 1;
            a2.Name = "ss3";
            var eq = a1.Equals(a2);
            Assert.IsTrue(eq);
        }
    }

    public class A : ValueObject<A>
    {
        public int AId { get; set; }
        public string Name { get; set; }

        protected override IEnumerable<object> EqualityComponents()
        {
            yield return this.AId;
           // yield return this.Name;
        }
    }
}
