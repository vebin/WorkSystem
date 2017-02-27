using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace YmtSystem.CrossCutting.Utility
{
    public class RSAEncrypt
    {
        /// <summary>
        /// RSA 加密
        /// </summary>
        /// <param name="val">要加密的字符</param>
        public static string Encrypt(string val)
        {
            string KeyContainerName = "MyKeyContainer";

            //Create a new key and persist it in 
            //the key container.
            RSAPersistKeyInCSP(KeyContainerName);

            //Create a UnicodeEncoder to convert between byte array and string.
            UnicodeEncoding ByteConverter = new UnicodeEncoding();

            //Create byte arrays to hold original, encrypted, and decrypted data.
            byte[] dataToEncrypt = ByteConverter.GetBytes(val);
            byte[] encryptedData;
          

            //Pass the data to ENCRYPT, the name of the key container,
            //and a boolean flag specifying no OAEP padding.
            encryptedData = RSAEncrypt_(dataToEncrypt, KeyContainerName, false);

            RSADeleteKeyInCSP(KeyContainerName);
            return BitConverter.ToString(encryptedData).Replace("-","");
            //Pass the data to DECRYPT, the name of the key container,
            //and a boolean flag specifying no OAEP padding.
           

            //Display the decrypted plaintext to the console. 
            //Console.WriteLine("Decrypted plaintext: {0}", ByteConverter.GetString(decryptedData));

           
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="val">要解密的字符串</param>
        /// <returns></returns>
        public static string Decrypt(string val) 
        {
            string KeyContainerName = "MyKeyContainer";
            //Create a new key and persist it in 
            //the key container.
            RSAPersistKeyInCSP(KeyContainerName);
            UnicodeEncoding ByteConverter = new UnicodeEncoding();
            byte[] encryptedData = ByteConverter.GetBytes(val);
           
            byte[] decryptedData  = RSADecrypt(encryptedData, KeyContainerName, false);
            RSADeleteKeyInCSP(KeyContainerName);
            return BitConverter.ToString(decryptedData);
        }
        public static void RSAPersistKeyInCSP(string ContainerName)
        {
            try
            {
                // Create a new instance of CspParameters.  Pass
                // 13 to specify a DSA container or 1 to specify
                // an RSA container.  The default is 1.
                CspParameters cspParams = new CspParameters();

                // Specify the container name using the passed variable.
                cspParams.KeyContainerName = ContainerName;

                //Create a new instance of RSACryptoServiceProvider to generate
                //a new key pair.  Pass the CspParameters class to persist the 
                //key in the container.  Pass an intger of 2048 to specify the 
                //key-size.
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider(2048, cspParams);

                //Indicate that the key was persisted.
                Console.WriteLine("The RSA key with a key-size of {0} was persisted in the container, \"{1}\".",
                                  RSAalg.KeySize, ContainerName);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

            }
        }

        public static void RSADeleteKeyInCSP(string ContainerName)
        {
            try
            {
                // Create a new instance of CspParameters.  Pass
                // 13 to specify a DSA container or 1 to specify
                // an RSA container.  The default is 1.
                CspParameters cspParams = new CspParameters();

                // Specify the container name using the passed variable.
                cspParams.KeyContainerName = ContainerName;

                //Create a new instance of DSACryptoServiceProvider. 
                //Pass the CspParameters class to use the 
                //key in the container.
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider(cspParams);

                //Delete the key entry in the container.
                RSAalg.PersistKeyInCsp = false;

                //Call Clear to release resources and delete the key from the container.
                RSAalg.Clear();

                //Indicate that the key was persisted.
                Console.WriteLine("The RSA key was deleted from the container, \"{0}\".", ContainerName);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

            }
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="DataToEncrypt"></param>
        /// <param name="ContainerName"></param>
        /// <param name="DoOAEPPadding"></param>
        /// <returns></returns>
        private static byte[] RSAEncrypt_(byte[] DataToEncrypt, string ContainerName, bool DoOAEPPadding)
        {
            try
            {
                // Create a new instance of CspParameters.  Pass
                // 13 to specify a DSA container or 1 to specify
                // an RSA container.  The default is 1.
                CspParameters cspParams = new CspParameters();

                // Specify the container name using the passed variable.
                cspParams.KeyContainerName = ContainerName;

                //Create a new instance of DSACryptoServiceProvider.
                //Pass the CspParameters class to use the key 
                //from the key in the container.
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider(cspParams);

                //Encrypt the passed byte array and specify OAEP padding.  
                //OAEP padding is only available on Microsoft Windows XP or
                //later.  
                return RSAalg.Encrypt(DataToEncrypt, DoOAEPPadding);
            }
            //Catch and display a CryptographicException  
            //to the console.
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return null;
            }

        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="DataToDecrypt"></param>
        /// <param name="ContainerName"></param>
        /// <param name="DoOAEPPadding"></param>
        /// <returns></returns>
        private static byte[] RSADecrypt(byte[] DataToDecrypt, string ContainerName, bool DoOAEPPadding)
        {
            try
            {
                // Create a new instance of CspParameters.  Pass
                // 13 to specify a DSA container or 1 to specify
                // an RSA container.  The default is 1.
                CspParameters cspParams = new CspParameters();

                // Specify the container name using the passed variable.
                cspParams.KeyContainerName = ContainerName;

                //Create a new instance of DSACryptoServiceProvider.
                //Pass the CspParameters class to use the key 
                //from the key in the container.
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider(cspParams);

                //Decrypt the passed byte array and specify OAEP padding.  
                //OAEP padding is only available on Microsoft Windows XP or
                //later.  
                return RSAalg.Decrypt(DataToDecrypt, DoOAEPPadding);
            }
            //Catch and display a CryptographicException  
            //to the console.
            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());

                return null;
            }

        }
    }
}
