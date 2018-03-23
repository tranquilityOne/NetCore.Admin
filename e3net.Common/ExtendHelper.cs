using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e3net.Common
{
    public static class ExtendHelper
    {
        private static string email_expression = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
        public static string ToAppSetting(this string key)
        {
            var ret = "";
            try
            {
                ret=System.Configuration.ConfigurationManager.AppSettings[key];
            }
            catch (Exception ex)
            {
                
               
            }
            return ret;
        }

        public static bool IsEmail(this string str_Email)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_Email, email_expression);
        }

        public static int ToSDBMHash(this string key)
        {
            int hash = 0;
            var arr = key.ToCharArray();
            foreach (var item in arr)
            {
                hash = 65599 * hash + item;
            }

            return hash;
        }


        public static int ToBKDRHash(this string key)
        {

            int hash = 0;
            var arr = key.ToCharArray();
            foreach (var item in arr)
            {
                hash = hash * 131 + item;
            }
            return hash;
        }


        public static string UnicodeToChinese(this string value)
        {
            var outStr = "";
            if (!string.IsNullOrEmpty(value))
            {
                string[] strlist = value.Replace("//", "").Replace("\\", "").Split('u');
                int i = 1;
                try
                {
                    for (; i < strlist.Length; i++)
                    {
                        //将unicode字符转为10进制整数，然后转为char中文字符  
                        outStr += (char)int.Parse(strlist[i], System.Globalization.NumberStyles.HexNumber);
                    }
                }
                catch (FormatException ex)
                {
                    outStr += strlist[i];
                }
            }
            return outStr;
        }
        public static String PadLeft(this int value, int length)
        {
            return value.ToString().PadLeft(length, '0');
        }

        public static String ToHexPadLeft(this int value, int length)
        {
            return value.ToString("X").PadLeft(length, '0');
        }
        public static String ToHexPadLeft(this long value, int length)
        {
            return value.ToString("X").PadLeft(length, '0');
        }

        public static String ToHexPadLeft(this short value, int length)
        {
            return value.ToString("X").PadLeft(length, '0');
        }

        public static String ToStrPadLeft(this int value, int length)
        {
            return value.ToString().PadLeft(length, '0');
        }

        public static String ToStrPadLeft(this short value, int length)
        {
            return value.ToString().PadLeft(length, '0');
        }

        public static String ToHexString(this byte[] byts)
        {
            String result = "";
            try
            {
                foreach (var item in byts)
                {
                    result += item.ToString("X").PadLeft(2, '0');
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return result;
        }

        public static String ToHexString(this byte[] byts, int StartIndex, int Length)
        {
            String result = "";
            try
            {
                for (int i = StartIndex; i < Length+StartIndex; i++)
                {
                    result += byts[i].ToString("X").PadLeft(2, '0');
                }


            }
            catch (Exception ex)
            {

                throw;
            }

            return result;
        }
        public static String ToHexStringReverse(this byte[] byts)
        {
            String result = "";
            for (int i = 0; i < byts.Count() / 2; i++)
            {
                result += (byts[i * 2 + 1].ToString("X").PadLeft(2, '0') + byts[i * 2].ToString("X").PadLeft(2, '0'));
            }
            return result;
        }
        public static String ToHexString(this String value)
        {
            
            String result = "";
            foreach (var item in value)
            {
                result += ((int)item).ToHexPadLeft(2);
            }
            return result;
        }

        public static String ToHexBinaryString(this String value)
        {
          
            String result = "";
            foreach (var item in value)
            {
                result += Convert.ToString(item, 2).PadLeft(8, '0');
            }
            return result;
        }
        public static String HexToString(this String value)
        {
            if (value.Length % 2 != 0)
            {
                return "";
            }
            String re = "";
            for (int i = 0; i < value.Length / 2; i++)
            {
                String tem = value.Substring(i * 2, 2);
                if (tem != "00")
                {
                    re += (char)Convert.ToInt32(tem, 16);
                }

            }
            return re;
        }

        public static byte[] HexStringToByte(this String value)
        {

            byte[] b = new byte[value.Length / 2];
            for (int i = 0; i < value.Length / 2; i++)
            {
                String tem = value.Substring(i * 2, 2);
                b[i] = byte.Parse(tem, System.Globalization.NumberStyles.AllowHexSpecifier);
            }
            return b;
        }

        public static int ToInt(this String value)
        {
            return Convert.ToInt32(value);
        }


        public static int ToInt32Little(this byte[] byts, int StartIndex = 0)
        {
            int value = (int)byts[StartIndex];
            for (int i = StartIndex + 1; i < StartIndex + 4; i++)
            {
                value = (value << 8) | (byts[i] & 0xFF);
            }
            return value;
        }

    }

}
