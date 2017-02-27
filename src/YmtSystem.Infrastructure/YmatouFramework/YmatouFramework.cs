
using System;
using Microsoft.Practices.Unity;

namespace Ymatou.Infrastructure
{
    public class YmatouFramework
    {
        private static readonly IUnityContainer container = new UnityContainer();
        private static Bootstrapper bootstrapper;
        private const string Version = "2012-12-19";//年-月-日-时

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
                LocalLoggingService.Debug("延迟启动...");
                return;
            }
            Status = YmatouFrameworkStatus.Starting;

            LocalLoggingService.Info("YmatouFramework开始启动...内部测试版本号：{0}", Version);

            bootstrapper = new Bootstrapper(container);
            if (bootstrapper.Execute())
            {
                Status = YmatouFrameworkStatus.Started;
                LocalLoggingService.Info("YmatouFramework启动完成！...内部测试版本号：{0}", Version);
            }
            else
            {
                Status = YmatouFrameworkStatus.FailedToStart;
                throw new Exception(string.Format("YmatouFramework启动失败！ {0} {1}", Version, ""));
            }
        }

        public static void Stop()
        {
            Status = YmatouFrameworkStatus.Ending;

            LocalLoggingService.Info("YmatouFramework开始清理...内部测试版本号：{0}", Version);

            bootstrapper.Dispose();
            Status = YmatouFrameworkStatus.Ended;

            LocalLoggingService.Info("YmatouFramework清理完成！...内部测试版本号：{0}", Version);
            LocalLoggingService.Close();
        }
    }
}
