using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Security.Cryptography;
using System;

namespace UDK
{
    public static class StringUtil
    {
        public static bool ContainChinese(string str)
        {
            char[] unicode = str.ToCharArray();
            for(var i = 0; i < unicode.Length; i += 2)
            {
                if(unicode[i] > 0x4e00 && unicode[i] <= 0x9fbb)
                {
                    return true;
                }
            }
            return false;
        }

        public static uint ToUInt32(string str)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(str);
            byte[] md5Data = md5.ComputeHash(data, 0, data.Length);
            md5.Clear();
            return BitConverter.ToUInt32(md5Data, 0);
        }

        public static string ToBase64(string value)
		{
			return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(value == null ? "" : value));
		}

        public static string FromBase64(string value)
        {
            return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(value == null ? "" : value));
        }
    }
}


