
namespace YmtSystem.CrossCutting
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using log4net;
    using YmtSystem.CrossCutting.Utility;

    public partial class YmatouLoggingService
    {
        //private static readonly ILog log = LogManager.GetLogger(typeof(YmatouLoggingService));

        #region public

        public static void InitLogService()
        {
            //var logfile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("Config\\log4net.config"));
            //log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(logfile));
            //log4net.Config.XmlConfigurator.Configure(new FileInfo(logfile));
        }

        public static void Debug(string s)
        {
            Ymatou.CommonService.ApplicationLog.Debug(s);
            //log.Debug(s);
        }

        public static void Debug(string format, params object[] args)
        {
            //log.DebugFormat(format, args);
            var msg = string.Format(format, args);
            Ymatou.CommonService.ApplicationLog.Debug(msg);
        }

        public static void Info(string s)
        {
            //log.Info(s);
            Ymatou.CommonService.ApplicationLog.Info(s);
        }

        public static void Info(string format, params object[] args)
        {
            //log.InfoFormat(format, args);
            var msg = string.Format(format, args);
            Ymatou.CommonService.ApplicationLog.Info(msg);
        }

        public static void Warning(string s)
        {
            //log.Warn(s);
            Ymatou.CommonService.ApplicationLog.Warn(s);
        }

        public static void Warning(string s, Exception ex)
        {
            //log.Warn(s, ex);
            Ymatou.CommonService.ApplicationLog.Warn(s, ex);
        }

        public static void Error(string s)
        {
            //log.Error(s);
            Ymatou.CommonService.ApplicationLog.Error(s);
        }
        public static void Error(string message, Exception ex)
        {
            //log.Error(message, ex);
            Ymatou.CommonService.ApplicationLog.Error(message, ex);
        }
        public static void Error(string format, params object[] args)
        {
            //log.ErrorFormat(format, args);
            var msg = string.Format(format, args);
            Ymatou.CommonService.ApplicationLog.Error(msg);
        }
        public static void Fatal(string s, Exception ex)
        {
            Ymatou.CommonService.ApplicationLog.Fatal(s, ex);
        }
        public static void Fatal(string s)
        {
            Ymatou.CommonService.ApplicationLog.Fatal(s);
        }
        public static void Fatal(string formart, object[] args)
        {
            var msg = string.Format(formart, args);
            Ymatou.CommonService.ApplicationLog.Fatal(msg);
        }
        #endregion
    }
}
