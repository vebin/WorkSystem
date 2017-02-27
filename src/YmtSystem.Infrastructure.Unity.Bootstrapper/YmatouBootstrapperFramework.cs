namespace YmtSystem.Infrastructure.Container.Unity.Bootstrapper
{
    using System;
    using Microsoft.Practices.Unity;
    using System.Diagnostics;
    using YmtSystem.CrossCutting;

    public class YmatouBootstrapperFramework
    {
        private static readonly IUnityContainer container = new UnityContainer();
        private static Bootstrapper bootstrapper;
        private const string Version = "2014-06-29 0.0.0.3.1";//年-月-日-时

        public YmatouBootstrapperFramework()
        {
            Status = BootstrapperStatus.NotStarted;
        }
        //Ninject
        public static IUnityContainer Container
        {
            get
            {
                return container;
            }
        }
        public static bool LazyStart { get; internal set; }
        public static BootstrapperStatus Status { get; internal set; }


        public static void Start(bool lazyStart = false)
        {

            if (lazyStart)
            {
                LazyStart = true;
                YmatouLoggingService.Debug("延迟启动...");
                return;
            }
            Status = BootstrapperStatus.Starting;
            var watch = Stopwatch.StartNew();
            YmatouLoggingService.Debug("YmatouFramework开始启动...内部版本号：{0}", Version);

            bootstrapper = new Bootstrapper(container);
            if (bootstrapper.Execute())
            {
                Status = BootstrapperStatus.Started;
                YmatouLoggingService.Debug("YmatouFramework启动完成！,耗时：{0} 秒...内部版本号：{1}", watch.ElapsedMilliseconds / 1000.0, Version);
            }
            else
            {
                Status = BootstrapperStatus.FailedToStart;
                throw new Exception(string.Format("YmatouFramework启动失败！，耗时 {0} 秒 {1}", watch.ElapsedMilliseconds / 1000.0, Version));
            }
        }

        public static void Stop()
        {
            Status = BootstrapperStatus.Ending;

            YmatouLoggingService.Debug("YmatouFramework开始清理");
            var watch = Stopwatch.StartNew();
            bootstrapper.Dispose();
            Status = BootstrapperStatus.Ended;

            YmatouLoggingService.Debug("YmatouFramework清理完成！耗时 ：{0}秒 ", watch.ElapsedMilliseconds / 1000.0);
        }
    }
}
