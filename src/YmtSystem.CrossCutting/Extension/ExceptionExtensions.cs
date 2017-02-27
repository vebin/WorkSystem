namespace System
{
    using System;
    using System.Threading.Tasks;
    using YmtSystem.CrossCutting;
    public static class ExceptionExtensions
    {
        public static void Handler(this Exception ex)
        {
            YmatouLoggingService.Error("Handler ", ex);
        }
        public static void Handler(this Exception ex, string message = "")
        {
            YmatouLoggingService.Error(message, ex);
        }
        public static void Handler(this Exception ex, string formart, params object[] args)
        {
            YmatouLoggingService.Error(formart + "，" + ex.ToString(), args);
        }
    }
}
