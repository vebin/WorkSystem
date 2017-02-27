using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YmtSystem.CrossCutting;
using NUnit.Framework;

namespace YmtSystem.CrossCutting.Test
{

    public enum AEnumTest
    {
        /// <summary>
        /// 初始化
        /// </summary>
        //[EnumDescription("初始化", "Ymatou.CrossCutting.Test.AEnumTest.A")]
        A = 0,
        /// <summary>
        /// 初始化值
        /// </summary>
        //[EnumDescription("初始化2", "Ymatou.CrossCutting.Test.AEnumTest.B")]
        B = 2
    }
    [TestFixture]
    public class ATEst
    {
        [Test]
        public void ShowDesc()
        {
            //var a = AEnumTest.A;
            ////var desc= a.GetDesc(AEnumTest.A, "sss");

            //Console.WriteLine(desc);
        }
    }
}
