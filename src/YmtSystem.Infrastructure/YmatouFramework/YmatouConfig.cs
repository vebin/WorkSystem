
namespace Ymatou.Infrastructure
{
    public class YmatouConfig
    {
        public string ApplicationName { get; set; }
        public bool ClearLocalLogWhenStart { get; set; }
        public LogLevel LocalLoggingServiceLevel { get; set; }

        public YmatouConfig()
        {
            ApplicationName = "*";
            ClearLocalLogWhenStart = false;
            LocalLoggingServiceLevel = LogLevel.Debug;
        }
    }
}
