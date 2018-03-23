using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace e3net.Common.Encrypt
{
    /// <summary>
    /// 非对称加密验证辅助类
    /// </summary>
    public class RSASecurityHelper
    {

        /// <summary>
        /// 公钥
        /// </summary>
        static string PublicKey = "122121";
        /// <summary>
        /// 对注册信息数据采用非对称加密的方式加密
        /// </summary>
        /// <param name="originalString">未加密的文本，如机器码</param>
        /// <param name="encrytedString">加密后的文本，如注册序列号</param>
        /// <returns>如果验证成功返回True，否则为False</returns>
        public static bool Validate(string originalString, string encrytedString)
        {
            bool bPassed = false;
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                try
                {
                    rsa.FromXmlString(PublicKey); //公钥
                    RSAPKCS1SignatureDeformatter formatter = new RSAPKCS1SignatureDeformatter(rsa);
                    formatter.SetHashAlgorithm("SHA1");

                    byte[] key = Convert.FromBase64String(encrytedString); //验证
                    SHA1Managed sha = new SHA1Managed();
                    byte[] name = sha.ComputeHash(ASCIIEncoding.ASCII.GetBytes(originalString));
                    if (formatter.VerifySignature(name, key))
                    {
                        bPassed = true;
                    }
                }
                catch
                {
                }
            }
            return bPassed;
        }


        /// <summary>
        /// HmacSha1  HmacSha2 签名
        /// </summary>
        /// <param name="text">输入</param>
        /// <param name="key">公钥</param>
        /// <returns>加密后的</returns>
        public static string HmacSha1Sign(string text, string key)
        {
            Encoding encode = Encoding.UTF8;
            byte[] byteData = encode.GetBytes(text);
            byte[] byteKey = encode.GetBytes(key);
            HMACSHA1 hmac = new HMACSHA1(byteKey);
            CryptoStream cs = new CryptoStream(Stream.Null, hmac, CryptoStreamMode.Write);
            cs.Write(byteData, 0, byteData.Length);
            cs.Close();
            return Convert.ToBase64String(hmac.Hash);
        }

    }
}