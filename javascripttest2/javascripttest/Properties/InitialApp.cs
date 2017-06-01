using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using javascripttest.Properties;
using System.Threading.Tasks;

namespace javascripttest
{
    public  class InitialApp
    {
        public static void IntialDB()
        {
            //string dir = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData),Resources.CompanyName);
            string dir = Path.Combine(System.Windows.Forms.Application.StartupPath, Resources.CompanyName);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            string dbPath = Path.Combine(dir,Resources.UserDbName+Resources.DbExtension);
            if (!File.Exists(dbPath))
            {
                WriteFile(Resources.User, dbPath);
            }
        }
        private static bool WriteFile(byte[] bytes, string fileName)
        {
            FileStream FileStream = null;
            try
            {
                FileStream = new FileStream(fileName, FileMode.OpenOrCreate);
                FileStream.Write(bytes, 0, bytes.Length);                
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (FileStream != null)
                    FileStream.Close();
            }
            return true;
        }

        public bool writeDb(string fileName)
        {
            byte[] bytes = Resources.User;
            FileStream FileStream = null;
            try
            {
                FileStream = new FileStream(fileName, FileMode.OpenOrCreate);
                FileStream.Write(bytes, 0, bytes.Length);
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (FileStream != null)
                    FileStream.Close();
            }
            return true;                
        }
    }
}
