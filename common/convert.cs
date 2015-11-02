/*
 * Created by SharpDevelop.
 * User: emm
 * Date: 18.02.2015
 * Time: 7:15
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Text;

namespace Convert
{
	public static class convert
	{
        public static void SetDecimalSeparatorPoint() 
        {
            System.Globalization.CultureInfo customCulture =
            (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();

            customCulture.NumberFormat.NumberDecimalSeparator = ".";

            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = customCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
        } 

		public static bool IsString(string s) {
			if ((s != null) && 
				(s.Length > 0)) return true;
			return false;
		}

        public static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        public static string decodeUTF8(string str)
        {
            return Encoding.UTF8.GetString( GetBytes(str) );
        }

        public static StreamReader StringToStream(string s) 
        {
            byte[] byteArray = Encoding.ASCII.GetBytes(s);
            MemoryStream stream = new MemoryStream(byteArray);
            return new StreamReader(stream);
        }
		
	}
}
