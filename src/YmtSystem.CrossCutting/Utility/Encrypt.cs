using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace YmtSystem.CrossCutting.Utility
{
    /// <summary>
    /// xLobo的加密（用于对BillCode加解密）需要跟xLobo项目中保持一致
    /// </summary>
    public class DesEncryptor
    {
        //密钥 
        private const string sKey = "qJzGEh6hESZDVJeCnFPGuxzaiB7NLQM3";
        //矢量，矢量可以为空 
        private const string sIV = "qcDY6X+aPLw=";
        //构造一个对称算法 
        private SymmetricAlgorithm mCSP = new TripleDESCryptoServiceProvider();
        public DesEncryptor() { }
        #region public string EncryptString(string Value)
        /// <summary> 
        /// 加密字符串 
        /// </summary> 
        /// <param name="Value">输入的字符串</param> 
        /// <returns>加密后的字符串</returns> 
        public string EncryptString(string Value)
        {
            ICryptoTransform ct;
            MemoryStream ms;
            CryptoStream cs;
            byte[] byt;
            mCSP.Key = Convert.FromBase64String(sKey);
            mCSP.IV = Convert.FromBase64String(sIV);
            //指定加密的运算模式 
            mCSP.Mode = System.Security.Cryptography.CipherMode.ECB;
            //获取或设置加密算法的填充模式 
            mCSP.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
            ct = mCSP.CreateEncryptor(mCSP.Key, mCSP.IV);
            byt = Encoding.UTF8.GetBytes(Value);
            ms = new MemoryStream();
            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();
            cs.Close();
            return Convert.ToBase64String(ms.ToArray());
        }
        #endregion
        #region public string DecryptString(string Value)
        /// <summary> 
        /// 解密字符串 
        /// </summary> 
        /// <param name="Value">加过密的字符串</param> 
        /// <returns>解密后的字符串</returns> 
        public string DecryptString(string Value)
        {
            ICryptoTransform ct;
            MemoryStream ms;
            CryptoStream cs;
            byte[] byt;
            mCSP.Key = Convert.FromBase64String(sKey);
            mCSP.IV = Convert.FromBase64String(sIV);
            mCSP.Mode = System.Security.Cryptography.CipherMode.ECB;
            mCSP.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
            ct = mCSP.CreateDecryptor(mCSP.Key, mCSP.IV);
            byt = Convert.FromBase64String(Value);
            ms = new MemoryStream();
            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();
            cs.Close();
            return Encoding.UTF8.GetString(ms.ToArray());
        }
        #endregion
    }

    public class Encrypt
    {

        #region Fields
        private string key_64 = "laskolee";
        private string iv_64 = "yangmato";
        private string iv_128 = "l0i5z1h7e8n4y1u9";
        private string key_128 = "lasko@ymatou.com";
        #endregion

        #region Properties
        /// <summary>
        /// Key 
        /// </summary>
        public string KEY_64
        {
            get { return key_64; }
            set { key_64 = value; }
        }
        /// <summary>
        /// Key 64bit
        /// </summary>
        public string IV_64
        {
            get { return iv_64; }
            set { iv_64 = value; }
        }
        /// <summary>
        /// 128bits initialization vector
        /// </summary>
        public string Iv_128
        {
            get { return this.iv_128; }
            set { this.iv_128 = value; }
        }
        /// <summary>
        /// 128bits secret key
        /// </summary>
        public string Key_128
        {
            get { return this.key_128; }
            set { this.key_128 = value; }
        }

        /// <summary>
        /// AES Provider
        /// </summary>
        SymmetricAlgorithm AesProvider
        {
            get
            {
                SymmetricAlgorithm aesProvider = Rijndael.Create();
                aesProvider.Key = Encoding.ASCII.GetBytes(Iv_128);
                aesProvider.IV = Encoding.ASCII.GetBytes(Iv_128);
                return aesProvider;
            }
        }

        /// <summary>
        /// DES Provider
        /// </summary>
        DESCryptoServiceProvider DesProvider
        {
            get
            {
                return new DESCryptoServiceProvider
                           {
                               Key = Encoding.ASCII.GetBytes(key_64),
                               IV = Encoding.ASCII.GetBytes(iv_64)
                           };
            }
        }
        #endregion

        public Encrypt()
        {
        }

        #region Encrypt Method

        /// <summary>
        /// DES Encrypt 
        /// </summary>
        /// <param name="pToEncrypt">param to encrypt</param>
        /// <returns>encrypted string</returns>
        /// ------------------------------------------------
        /// Change History:
        /// Date			Who		            Changes Made
        /// 2010-05-11		Lasko li		    Mark method
        ///-------------------------------------------------
        public string DesEncrypt(string pToEncrypt)
        {
            return DesEncrypt(pToEncrypt, true);
        }

        /// <summary>
        /// DES Encrypt include hex convert
        /// </summary>
        /// <param name="pToEncrypt">param to encrypt</param>
        /// <param name="isHex">covert hex</param>
        /// <returns>encrypted string</returns>
        /// ------------------------------------------------
        /// Change History:
        /// Date			Who		            Changes Made
        /// 2010-05-11		Lasko li		    Mark method
        ///-------------------------------------------------
        public string DesEncrypt(string pToEncrypt, bool isHex)
        {
            string str = "";
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, DesProvider.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    if (isHex)
                    {
                        byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                        cs.FlushFinalBlock();
                        StringBuilder ret = new StringBuilder();
                        foreach (byte b in ms.ToArray())
                        {
                            //Format  as  hex  
                            ret.AppendFormat("{0:X2}", b);
                        }
                        str = ret.ToString();
                    }
                    else
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(pToEncrypt);
                            sw.Flush();
                            cs.FlushFinalBlock();
                            str = Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
                        }
                    }
                }
            }
            return str;
        }

        /// <summary>
        /// AES Encrypt
        /// </summary>
        /// <param name="pToEncrypt">param to encrypt</param>
        /// <returns>encrypted string</returns>
        /// ------------------------------------------------
        /// Change History:
        /// Date			Who		            Changes Made
        /// 2010-05-11		Lasko li		    Mark method
        ///-------------------------------------------------
        public string AesEncrypt(string pToEncrypt)
        {
            return AesEncrypt(pToEncrypt, false);
        }

        /// <summary>
        /// AES  Encrypt include hex convert
        /// </summary>
        /// <param name="pToEncrypt">param to encrypt</param>
        /// <param name="isHex">convert hex</param>
        /// <returns>encrypted result</returns>
        /// ------------------------------------------------
        /// Change History:
        /// Date			Who		            Changes Made
        /// 2010-05-11		Lasko li		    Mark method
        ///-------------------------------------------------
        public string AesEncrypt(string pToEncrypt, bool isHex)
        {
            string str = "";
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, AesProvider.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    if (isHex)
                    {
                        byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
                        StringBuilder ret = new StringBuilder();
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                        cs.FlushFinalBlock();
                        foreach (byte b in ms.ToArray())
                        {
                            ret.AppendFormat("{0:X2}", b);
                        }
                        str = ret.ToString();
                    }
                    else
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(pToEncrypt);
                            sw.Flush();
                            cs.FlushFinalBlock();


                            str = Convert.ToBase64String(ms.ToArray());
                        }
                    }
                }

            }
            return str;
        }

        /// <summary>
        /// MD5 Encrypt
        /// </summary>
        /// <param name="pToEncrypt">param to encrypt</param>
        /// <returns>encrypted string</returns>
        /// ------------------------------------------------
        /// Change History:
        /// Date			Who		            Changes Made
        /// 2010-05-11		Lasko li		    Mark method
        ///-------------------------------------------------
        public string Md5Encrypt(string pToEncrypt)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string md5Str = BitConverter.ToString(md5.ComputeHash(System.Text.Encoding.ASCII.GetBytes(pToEncrypt)));
            md5.Clear();
            md5Str = md5Str.Replace("-", "");
            return md5Str;
        }


        /// <summary>
        /// MD5 Encrypt Method with format
        /// </summary>
        /// <param name="pToEncrypt">param to encrypt</param>
        /// <param name="encryptForm">md5 encrypt format</param>
        /// <returns>encrypt string</returns>
        /// ------------------------------------------------
        /// Change History:
        /// Date			Who		            Changes Made
        /// 2010-05-11		Lasko li		    Mark method
        ///-------------------------------------------------
        public string Md5Encrypt(string pToEncrypt, int encryptForm)
        {
            string str = "";
            if (encryptForm == 16)
            {
                str = Md5Encrypt(pToEncrypt).Substring(8, 16);
            }
            else
            {
                str = Md5Encrypt(pToEncrypt);
            }
            return str;
        }

        /// <summary>
        /// MD5 Hash encrypt
        /// </summary>
        /// <param name="pToEncrypt">param to encrypt</param>
        /// <returns>encrypted hash string</returns>
        /// ------------------------------------------------
        /// Change History:
        /// Date			Who		            Changes Made
        /// 2010-05-11		Lasko li		    Mark method
        ///-------------------------------------------------
        public string Md5HashEncrypt(string pToEncrypt)
        {
            //get param's byte array
            byte[] bytesParam = Encoding.Unicode.GetBytes(pToEncrypt); //Encoding.UTF8.GetBytes(pToEncrypt);

            //Instance MD5CryptoServiceProvider
            MD5CryptoServiceProvider md5Provider = new MD5CryptoServiceProvider();

            // convert byte array to byte md5
            byte[] bytesMd5 = md5Provider.ComputeHash(bytesParam);

            //convert md5 bytes to string

            StringBuilder sb = new StringBuilder();
            foreach (byte b in bytesMd5)
            {
                sb.AppendFormat("{0:X2}", b);
            }
            return sb.ToString();
        }

        #endregion

        #region Decrypt Method

        /// <summary>
        /// DES Decrypt
        /// </summary>
        /// <param name="pToDecrypt">param to decrypt</param>
        /// <returns>decrypted string</returns>
        /// ------------------------------------------------
        /// Change History:
        /// Date			Who		            Changes Made
        /// 2010-05-11		Lasko li		    Mark method
        ///-------------------------------------------------
        public string DesDecrypt(string pToDecrypt)
        {
            return DesDecrypt(pToDecrypt, false);
        }

        /// <summary>
        /// DES Decrypt include hex convert
        /// </summary>
        /// <param name="pToDecrypt">param to decrypt</param>
        /// <param name="isHex">convert hex</param>
        /// <returns>decrypted string</returns>
        /// ------------------------------------------------
        /// Change History:
        /// Date			Who		            Changes Made
        /// 2010-05-11		Lasko li		    Mark method
        ///-------------------------------------------------
        public string DesDecrypt(string pToDecrypt, bool isHex)
        {
            string str = "";

            //Put the input string into the byte array 

            if (isHex)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, DesProvider.CreateDecryptor(), CryptoStreamMode.Write))
                    {

                        byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
                        for (int x = 0; x < pToDecrypt.Length / 2; x++)
                        {
                            int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                            inputByteArray[x] = (byte)i;
                        }
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                        cs.FlushFinalBlock();
                        str = System.Text.Encoding.Default.GetString(ms.ToArray());

                    }
                }
            }
            else
            {
                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(pToDecrypt)))
                {
                    using (CryptoStream cst = new CryptoStream(ms, DesProvider.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cst))
                        {
                            str = sr.ReadToEnd();
                        }
                    }
                }
            }
            return str;
        }

        /// <summary>
        /// AES Decryp
        /// </summary>
        /// <param name="pToDecrypt">param to decrypt</param>
        /// <returns>decrypted string </returns>
        /// ------------------------------------------------
        /// Change History:
        /// Date			Who		            Changes Made
        /// 2010-05-11		Lasko li		    Mark method
        ///-------------------------------------------------
        public string AesDecrypt(string pToDecrypt)
        {
            return AesDecrypt(pToDecrypt, false);
        }

        /// <summary>
        /// AES Decryp include hex convert
        /// </summary>
        /// <param name="pToDecrypt">param to decrypt</param>
        /// <param name="isHex">convert hex</param>
        /// <returns>decrypted string</returns>
        /// ------------------------------------------------
        /// Change History:
        /// Date			Who		            Changes Made
        /// 2010-05-11		Lasko li		    Mark method
        ///-------------------------------------------------
        public string AesDecrypt(string pToDecrypt, bool isHex)
        {
            string str = "";
            if (isHex)
            {
                byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
                for (int x = 0; x < pToDecrypt.Length / 2; x++)
                {
                    int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                    inputByteArray[x] = (byte)i;
                }

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream encStream = new CryptoStream(ms, AesProvider.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        encStream.Write(inputByteArray, 0, inputByteArray.Length);
                        encStream.FlushFinalBlock();
                        str = System.Text.Encoding.Default.GetString(ms.ToArray());
                    }
                }

            }
            else
            {
                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(pToDecrypt)))
                {
                    using (CryptoStream encStream = new CryptoStream(ms, AesProvider.CreateDecryptor(), CryptoStreamMode.Read))
                    {

                        using (StreamReader sr = new StreamReader(encStream))
                        {
                            str = sr.ReadToEnd();
                        }
                    }
                }
            }
            return str;
        }

        #endregion

        /// <summary>
        /// ProduceStochasticCode
        /// </summary>
        /// <returns>产生指定长度随机数</returns>
        /// ------------------------------------------------
        /// Change History:
        /// Date			Who		            Changes Made
        /// 2010-05-11		Lasko li		    Mark method
        ///-------------------------------------------------
        public string StochasticCode(int strLength)
        {
            //声明要返回的字符串
            string tmpstr = "";
            //密码中包含的字符数组
            string pwdchars = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ@#";
            //数组索引随机数
            int iRandNum;
            //随机数生成器
            Random rnd = new Random();
            for (int i = 0; i < strLength; i++)
            {
                //Random类的Next方法生成一个指定范围的随机数
                iRandNum = rnd.Next(pwdchars.Length);
                //tmpstr随机添加一个字符
                tmpstr += pwdchars[iRandNum];
            }
            return tmpstr;
        }

        /// <summary>
        /// Ymatou user password encrypt method AES + MD5
        /// </summary>
        /// <param name="pToEncrypt">密码字串</param>
        /// <returns>用户密码密字</returns>
        /// ------------------------------------------------
        /// Change History:
        /// Date			Who		            Changes Made
        /// 2010-05-11		Lasko li		    Mark method
        ///-------------------------------------------------
        public string EncryptUserPassword(string pToEncrypt)
        {
            string strEncrypt = Md5Encrypt((AesEncrypt(pToEncrypt, true)));
            return strEncrypt;
        }

        /// <summary>
        /// Ymtou trading password encrypt method 3DES +MD5
        /// </summary>
        /// <param name="pToEncrypt">交易信息字串</param>
        /// <returns>加密交易信息字串</returns>
        /// ------------------------------------------------
        /// Change History:
        /// Date			Who		            Changes Made
        /// 2010-05-11		Lasko li		    Mark method
        ///-------------------------------------------------
        public string EncryptTradingPassword(string pToEncrypt)
        {
            string strEncrypt = DesEncrypt(DesEncrypt(DesEncrypt(pToEncrypt, true), true), true);
            strEncrypt = Md5Encrypt(strEncrypt);
            return strEncrypt;
        }
    }
}
