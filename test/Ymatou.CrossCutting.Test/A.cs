using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YmtSystem.CrossCutting;
using NUnit.Framework ;
namespace YmtSystem.CrossCutting.Test
{
    [TestFixture]
    public class A1
    {
        [Test]
        public void SubString() 
        {
            var s = "13564695629".SubString(11);
            var sss = "LG".SubString(50);
        }
    }
}
