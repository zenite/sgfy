using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using log4net.Core;
using log4net.Repository;

//[assembly: XmlConfigurator(ConfigFile = "log4net.config", ConfigFileExtension = "config", Watch = true)]
namespace javascripttest
{
    public class LogHelper
    {
        private static ILog log;

        static LogHelper()
        {
            ILoggerRepository repository = LogManager.CreateRepository("NETCoreRepository");
            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
            log = LogManager.GetLogger(repository.Name, "NETCorelog4net");
        }

        public static void Debug(object message)
        {
            log.Debug(message);
        }

        public static void DebugFormatted(string format, params object[] args)
        {
            log.DebugFormat(format, args);
        }
         public static void Info(object message)
        {
            log.Info(message);
        }

        public static void InfoFormatted(string format, params object[] args)
        {
            log.InfoFormat(format, args);
        }

        public static void Warn(object message)
        {
            log.Warn(message);
        }

        public static void Warn(object message, Exception exception)
        {
            log.Warn(message, exception);
        }

        public static void WarnFormatted(string format, params object[] args)
        {
            log.WarnFormat(format, args);
        }

        public static void Error(object message)
        {
            log.Error(message);
        }

        public static void Error(object message, Exception exception)
        {
            log.Error(message, exception);
        }

        public static void ErrorFormatted(string format, params object[] args)
        {
            log.ErrorFormat(format, args);
        }

        public static void Fatal(object message)
        {
            log.Fatal(message);
        }

        public static void Fatal(object message, Exception exception)
        {
            log.Fatal(message, exception);
        }

        public static void FatalFormatted(string format, params object[] args)
        {
            log.FatalFormat(format, args);
        }


        //public void WriteLog(string msg)
        //{
        //    string filePath = AppDomain.CurrentDomain.BaseDirectory + "Log";
        //    if (!Directory.Exists(filePath))
        //    {
        //        Directory.CreateDirectory(filePath);
        //    }
        //    string logPath = AppDomain.CurrentDomain.BaseDirectory + "Log\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
        //    try
        //    {
        //        using (StreamWriter sw = File.AppendText(logPath))
        //        {
        //            sw.WriteLine("消息：" + msg);
        //            sw.WriteLine("时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        //            sw.WriteLine("**************************************************");
        //            sw.WriteLine();
        //            sw.Flush();
        //            sw.Close();
        //            sw.Dispose();
        //        }
        //    }
        //    catch (IOException e)
        //    {
        //        using (StreamWriter sw = File.AppendText(logPath))
        //        {
        //            sw.WriteLine("异常：" + e.Message);
        //            sw.WriteLine("时间：" + DateTime.Now.ToString("yyy-MM-dd HH:mm:ss"));
        //            sw.WriteLine("**************************************************");
        //            sw.WriteLine();
        //            sw.Flush();
        //            sw.Close();
        //            sw.Dispose();
        //        }
        //    }
        //}

    }
}
