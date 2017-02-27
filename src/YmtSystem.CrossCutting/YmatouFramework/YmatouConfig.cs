
namespace YmtSystem.CrossCutting
{
    public class YmatouConfig
    {
        public string ApplicationName { get; set; }
        public bool ClearLocalLogWhenStart { get; set; }
        public LogLevel LocalLoggingServiceLevel { get; set; }
        public bool EnableAssert { get; set; }
        public bool LogTypeFile { get; set; }
        public YmatouConfig()
        {
            ApplicationName = "*";
            ClearLocalLogWhenStart = false;
            LocalLoggingServiceLevel = LogLevel.Debug;
            EnableAssert = false;
            LogTypeFile = false;
        }
    }
}
