using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Security;
using System.Security.Cryptography;
using System.IO;

namespace javascripttest.BLL
{
    class SoftReg
    {
        public  string Decrypt(string string_0, string string_1)
        {
            string str2;
            try
            {
                if (string.IsNullOrEmpty(string_0))
                {
                    return "";
                }
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                int num = (int)Math.Round((double)((((double)string_0.Length) / 2.0) - 1.0));
                byte[] buffer = new byte[num + 1];
                int num2 = num;
                for (int i = 0; i <= num2; i++)
                {
                    int num4 = 0;
                    try
                    {
                        if (((i * 2) + 2) <= string_0.Length)
                        {
                            num4 = Convert.ToInt32(string_0.Substring(i * 2, 2), 0x10);
                        }
                    }
                    catch
                    {
                        break;
                    }
                    buffer[i] = (byte)num4;
                }
                provider.Key = Encoding.ASCII.GetBytes(string_1);
                provider.IV = Encoding.ASCII.GetBytes(string_1);
                MemoryStream stream = new MemoryStream();
                CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(), CryptoStreamMode.Write);
                stream2.Write(buffer, 0, buffer.Length);
                try
                {
                    stream2.FlushFinalBlock();
                    stream2.Close();
                    str2 = Encoding.Default.GetString(stream.ToArray());
                }
                catch
                {
                    str2 = "";
                }
            }
            catch (Exception exception)
            {                
                str2 = exception.ToString();                
            }
            return str2;
        }

        public  string Encrypt(string string_0, string string_1)
        {
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            byte[] bytes = Encoding.Default.GetBytes(string_0);
            provider.Key = Encoding.ASCII.GetBytes(string_1);
            provider.IV = Encoding.ASCII.GetBytes(string_1);
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(), CryptoStreamMode.Write);
            stream2.Write(bytes, 0, bytes.Length);
            stream2.FlushFinalBlock();
            StringBuilder builder = new StringBuilder();
            foreach (byte num2 in stream.ToArray())
            {
                builder.AppendFormat("{0:X2}", num2);
            }
            return builder.ToString();
        }

        public  long GetHardNum(string string_0)
        {
            long num4;
            long num = 0L;
            long num2 = 0L;
            uint num3 = 0;
            try
            {
                ManagementObjectCollection.ManagementObjectEnumerator enumerator;
                ManagementObject current;
                string str;
                ManagementClass class2 = new ManagementClass("Win32_Processor");
                using (enumerator = class2.GetInstances().GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        current = (ManagementObject)enumerator.Current;
                        str = current.Properties["ProcessorId"].Value.ToString().Substring(0, 8);
                        if (!string.IsNullOrEmpty(str))
                        {
                            goto Label_007A;
                        }
                    }
                    goto Label_0093;
                Label_007A:
                    num2 = getstingcount(str);
                }
            Label_0093:
                class2 = new ManagementClass("Win32_DiskDrive");
                using (enumerator = class2.GetInstances().GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        current = (ManagementObject)enumerator.Current;
                        str = current.Properties["signature"].Value.ToString();
                        if (!string.IsNullOrEmpty(str))
                        {
                            goto Label_00F0;
                        }
                    }
                    goto Label_011D;
                Label_00F0:
                    num3 = Convert.ToUInt32(str);
                    if (num3 == 0L)
                    {
                        num3 = 0;
                    }
                }
            Label_011D:
                num4 = num2 + long.Parse(num3.ToString());
            }
            catch (Exception)
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
                foreach (ManagementObject obj3 in searcher.Get())
                {
                    long.Parse(obj3["size"].ToString());
                }
                num4 = num;
            }
            return num4;
        }
        private  long getstingcount(string string_0)
        {
            long num = 0L;
            byte[] bytes = new ASCIIEncoding().GetBytes(string_0);
            for (int i = 0; i <= (bytes.Length - 1); i++)
            {
                num += bytes[i];
            }
            return num;
        }
    }
}
