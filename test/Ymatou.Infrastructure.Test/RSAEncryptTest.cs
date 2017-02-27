using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using YmtSystem.CrossCutting.Utility;

namespace YmtSystem.Infrastructure
{
    [TestFixture]
    public class RSAEncryptTest
    {
        [TestCase("aa")]
        [TestCase("bb")]
        public void Encrypt(string val)
        {
            //RSACryptoServiceProvider oRSA = new RSACryptoServiceProvider();
            //string privatekey = oRSA.ToXmlString(true);//私钥 
            //string publickey = oRSA.ToXmlString(false);//公钥 
            ////这两个密钥需要保存下来 
            //byte[] messagebytes = Encoding.UTF8.GetBytes(val); //需要加密的数据 

            ////公钥加密 
            //RSACryptoServiceProvider oRSA1 = new RSACryptoServiceProvider();
            //oRSA1.FromXmlString(publickey); //加密要用到公钥所以导入公钥 
            //byte[] AOutput = oRSA1.Encrypt(messagebytes, false); //AOutput 加密以后的数据 
            var str =RSAEncrypt.Encrypt(val); 
            Console.WriteLine(str);
            str = RSAEncrypt.Decrypt(val);
            Console.WriteLine(str);
        }
    }
}
