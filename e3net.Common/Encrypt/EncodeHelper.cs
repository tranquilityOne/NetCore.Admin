using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
//加密字符串,注意strEncrKey的长度为8位(如果要增加或者减少key长度,调整IV的长度就是了) 
//public string DesEncrypt(string strText, string strEncrKey) 

//解密字符串,注意strEncrKey的长度为8位(如果要增加或者减少key长度,调整IV的长度就是了) 
//public string DesDecrypt(string strText,string sDecrKey) 

//加密数据文件,注意strEncrKey的长度为8位(如果要增加或者减少key长度,调整IV的长度就是了) 
//public void DesEncrypt(string m_InFilePath,string m_OutFilePath,string strEncrKey) 

//解密数据文件,注意strEncrKey的长度为8位(如果要增加或者减少key长度,调整IV的长度就是了) 
//public void DesDecrypt(string m_InFilePath,string m_OutFilePath,string sDecrKey) 

//MD5加密 
//public string MD5Encrypt(string strText) 

namespace e3net.Common.Encrypt
{
    /// <summary>
    /// DES对称加解密、AES RijndaelManaged加解密、Base64加密解密、MD5加密等操作辅助类
    /// </summary>
    public sealed class EncodeHelper
    {
        /// <summary>
        /// 两个字符串混合 (如a="abc.." b="12345.." z="a1b2c3..
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string MixString(string a, string b)
        {

            char[] a1, b1;

            a1 = a.ToCharArray();
            b1 = b.ToCharArray();
            int aleng = a1.Length;
            int bleng = b1.Length;
            int Mleng = aleng > bleng ? aleng : bleng;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i <= Mleng; i++)
            {
                if (i <= aleng - 1)
                {
                    sb.Append(a1[i]);
                }
                if (i <= bleng - 1)
                {
                    sb.Append(b1[i]);
                }

            }
            return sb.ToString();
        }


        /// <summary>
        /// 前后一半进行 对位 异或,返回字节数组转16进制字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string YHString(string str)
        {
            bool isEq = str.Length % 2 == 0;

            int tleng = str.Length / 2;
            string af;
            if (isEq)
            {
                af = str.Substring(0, tleng);
            }
            else
            {
                af = str.Substring(0, tleng) + "0";
            }
            string bf = str.Substring(tleng, tleng);
            byte[] aBytes = System.Text.Encoding.Unicode.GetBytes(af);
            byte[] bytes = System.Text.Encoding.Unicode.GetBytes(bf);

            byte[] rBytes = new byte[tleng];
            for (int i = 0; i < tleng; i++)
            {
                rBytes[i] = (byte)(aBytes[i] ^ bytes[i]);

            }
            StringBuilder EnText = new StringBuilder();
            foreach (byte iByte in rBytes)
            {
                EnText.AppendFormat("{0:x2}", iByte);
            }
            return EnText.ToString();
        }

        #region Base64加密解密
        /// <summary>
        /// Base64是一種使用64基的位置計數法。它使用2的最大次方來代表僅可列印的ASCII 字元。
        /// 這使它可用來作為電子郵件的傳輸編碼。在Base64中的變數使用字元A-Z、a-z和0-9 ，
        /// 這樣共有62個字元，用來作為開始的64個數字，最後兩個用來作為數字的符號在不同的
        /// 系統中而不同。
        /// Base64加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Base64Encrypt(string str)
        {
            byte[] encbuff = System.Text.Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(encbuff);
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Base64Decrypt(string str)
        {
            byte[] decbuff = Convert.FromBase64String(str);
            return System.Text.Encoding.UTF8.GetString(decbuff);
        }
        #endregion

        #region MD5加密
        /// <summary> 
        /// MD5 Encrypt 
        /// </summary> 
        /// <param name="strText">text</param> 
        /// <returns>md5 Encrypt string</returns> 
        public static string MD5Encrypt(string strText)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(Encoding.Default.GetBytes(strText));
            return Encoding.Default.GetString(result);
        }

        public static string MD5EncryptHash(String input)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            //the GetBytes method returns byte array equavalent of a string
            byte[] res = md5.ComputeHash(Encoding.Default.GetBytes(input), 0, input.Length);
            char[] temp = new char[res.Length];
            //copy to a char array which can be passed to a String constructor
            Array.Copy(res, temp, res.Length);
            //return the result as a string
            return new String(temp);
        }

        public static string MD5EncryptHashHex(String input)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            //the GetBytes method returns byte array equavalent of a string
            byte[] res = md5.ComputeHash(Encoding.Default.GetBytes(input), 0, input.Length);

            String returnThis = string.Empty;

            for (int i = 0; i < res.Length; i++)
            {
                returnThis += Uri.HexEscape((char)res[i]);
            }
            returnThis = returnThis.Replace("%", "");
            returnThis = returnThis.ToLower();

            return returnThis;
        }

        /// <summary>
        /// MD5 三次加密算法.计算过程: (QQ使用)
        /// 1. 验证码转为大写
        /// 2. 将密码使用这个方法进行三次加密后,与验证码进行叠加
        /// 3. 然后将叠加后的内容再次MD5一下,得到最终验证码的值
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string EncyptMD5_3_16(string s)
        {
            MD5 md5 = MD5CryptoServiceProvider.Create();
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(s);
            byte[] bytes1 = md5.ComputeHash(bytes);
            byte[] bytes2 = md5.ComputeHash(bytes1);
            byte[] bytes3 = md5.ComputeHash(bytes2);

            StringBuilder sb = new StringBuilder();
            foreach (var item in bytes3)
            {
                sb.Append(item.ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString().ToUpper();
        }
        #endregion


        #region SHA
        /// <summary>
        /// 对字符串进行SHA1加密
        /// </summary>
        /// <param name="strIN">需要加密的字符串</param>
        /// <returns>密文</returns>
        public static string SHA1_Encrypt(string Source_String)
        {
            byte[] StrRes = Encoding.Default.GetBytes(Source_String);
            HashAlgorithm iSHA = new SHA1CryptoServiceProvider();
            StrRes = iSHA.ComputeHash(StrRes);
            StringBuilder EnText = new StringBuilder();
            foreach (byte iByte in StrRes)
            {
                EnText.AppendFormat("{0:x2}", iByte);
            }
            return EnText.ToString();
        }
        /// <summary>
        /// SHA256加密，不可逆转
        /// </summary>
        /// <param name="str">string str:被加密的字符串</param>
        /// <returns>返回加密后的字符串</returns>
        private static string SHA256Encrypt(string str)
        {
            System.Security.Cryptography.SHA256 s256 = new System.Security.Cryptography.SHA256Managed();
            byte[] byte1;
            byte1 = s256.ComputeHash(Encoding.Default.GetBytes(str));
            s256.Clear();
            return Convert.ToBase64String(byte1);//返回长度为44字节的字符串
        }

        /// <summary>
        /// SHA384加密，不可逆转
        /// </summary>
        /// <param name="str">string str:被加密的字符串</param>
        /// <returns>返回加密后的字符串</returns>
        private static string SHA384Encrypt(string str)
        {
            System.Security.Cryptography.SHA384 s384 = new System.Security.Cryptography.SHA384Managed();
            byte[] byte1;
            byte1 = s384.ComputeHash(Encoding.Default.GetBytes(str));
            s384.Clear();
            return Convert.ToBase64String(byte1);
        }


        /// <summary>
        /// SHA512加密，不可逆转
        /// </summary>
        /// <param name="str">string str:被加密的字符串</param>
        /// <returns>返回加密后的字符串</returns>
        private static string SHA512Encrypt(string str)
        {
            System.Security.Cryptography.SHA512 s512 = new System.Security.Cryptography.SHA512Managed();
            byte[] byte1;
            byte1 = s512.ComputeHash(Encoding.Default.GetBytes(str));
            s512.Clear();
            return Convert.ToBase64String(byte1);
        }

        #endregion



        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string EncryptString(string input)
        {
            return MD5Util.AddMD5Profix(Base64Util.Encrypt(MD5Util.AddMD5Profix(input)));
            //return Base64.Encrypt(MD5.AddMD5Profix(Base64.Encrypt(input)));
        }

        /// <summary>
        /// 解密加过密的字符串
        /// </summary>
        /// <param name="input"></param>
        /// <param name="throwException">解密失败是否抛异常</param>
        /// <returns></returns>
        public static string DecryptString(string input, bool throwException)
        {
            string res = "";
            try
            {
                res = input;// Base64.Decrypt(input);
                if (MD5Util.ValidateValue(res))
                {
                    return MD5Util.RemoveMD5Profix(Base64Util.Decrypt(MD5Util.RemoveMD5Profix(res)));
                }
                else
                {
                    throw new Exception("字符串无法转换成功！");
                }
            }
            catch
            {
                if (throwException)
                {
                    throw;
                }
                else
                {
                    return "";
                }
            }
        }
    }
}