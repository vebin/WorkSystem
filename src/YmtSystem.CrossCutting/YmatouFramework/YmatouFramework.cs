using System;
using Microsoft.Practices.Unity;
using System.Diagnostics;

namespace YmtSystem.CrossCutting
{
    public class YmatouFramework
    {
        private static readonly IUnityContainer container = new UnityContainer();
        private static Bootstrapper bootstrapper;
        private const string Version = "2014-06-29 0.0.0.3.1";//年-月-日-时

        public YmatouFramework()
        {
            Status = YmatouFrameworkStatus.NotStarted;
        }

        public static IUnityContainer Container
        {
            get
            {
                return container;
            }
        }
        public static bool LazyStart { get; internal set; }
        public static YmatouFrameworkStatus Status { get; internal set; }


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

            bootstrapper = new Bootstrapper(container);
            if (bootstrapper.Execute())
            {
                Status = YmatouFrameworkStatus.Started;
                YmatouLoggingService.Debug("YmatouFramework启动完成！,耗时：{0} 秒...内部版本号：{1}", watch.ElapsedMilliseconds / 1000.0, Version);
            }
            else
            {
                Status = YmatouFrameworkStatus.FailedToStart;
                throw new Exception(string.Format("YmatouFramework启动失败！，耗时 {0} 秒 {1}", watch.ElapsedMilliseconds / 1000.0, Version));
            }
        }

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
