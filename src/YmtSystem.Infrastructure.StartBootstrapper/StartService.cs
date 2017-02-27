namespace YmtSystem.Infrastructure.Bootstrapper
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using YmtSystem.CrossCutting;

    public class StartService
    {
        public static YmatouFrameworkStatus Status { get; internal set; }
        public static bool LazyStart { get; internal set; }

        private static BootStrapper bootstrapper;
        private const string Version = "2014-10-02 0.0.3.3";//年-月-日-时

        public StartService()
        {
            Status = YmatouFrameworkStatus.NotStarted;
        }

        /// <summary>
        /// start all service 
        /// </summary>
        /// <param name="lazyStart">是否延迟启动</param>
        public static void Start(bool lazyStart = false)
        {

            if (lazyStart)
            {
                LazyStart = true;
                YmatouLoggingService.Debug("延迟启动...");
                return;
            }
            Status = YmatouFrameworkStatus.Starting;
            var watch = Stopwatch.StartNew();
            YmatouLoggingService.Debug("YmatouFramework开始启动...内部版本号：{0}", Version);

            bootstrapper = new BootStrapper();
            string errMsg = string.Empty;
            if (bootstrapper.Execute(out errMsg))
            {
                Status = YmatouFrameworkStatus.Started;
                YmatouLoggingService.Debug("YmatouFramework启动完成！,耗时：{0} 秒...内部版本号：{1}", watch.ElapsedMilliseconds / 1000.0, Version);
            }
            else
            {
                Status = YmatouFrameworkStatus.FailedToStart;
                throw new Exception(string.Format("YmatouFramework启动失败！，耗时 {0} 秒 {1}，{2}", watch.ElapsedMilliseconds / 1000.0, Version, errMsg));
            }
        }

        /// <summary>
        /// stop all service
        /// </summary>
        public static void Stop()
        {
            Status = YmatouFrameworkStatus.Ending;

            YmatouLoggingService.Debug("YmatouFramework开始清理");
            var watch = Stopwatch.StartNew();
            bootstrapper.Dispose();
            Status = YmatouFrameworkStatus.Ended;

            YmatouLoggingService.Debug("YmatouFramework清理完成！耗时 ：{0}秒 ", watch.ElapsedMilliseconds / 1000.0);
        }
    }
}
