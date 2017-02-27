namespace YmtSystem.CrossCutting
{
    using System;
    using System.Linq;
    using Microsoft.Practices.Unity;
    using YmtSystem.CrossCutting.Utility;
    using System.Collections.Generic;

    public class Bootstrapper : Disposable
    {
        private IUnityContainer container;

        public Bootstrapper(IUnityContainer container)
        {
            this.container = container;

            //container.RegisterInstance<IUnityContainer>(container);
            YmatouLoggingService.Debug("load BootstrapperTask...");
            BuildManagerWrapper.Current.ConcreteTypes
                       .Where(type => typeof(BootstrapperTask).IsAssignableFrom(type))
                       .Each(type => container.RegisterMultipleTypesAsSingleton(typeof(BootstrapperTask), type));
            YmatouLoggingService.Debug("load BootstrapperTask end...");
        }


        public bool Execute()
        {
            bool successful = true;
            var tasks = container.ResolveAll<BootstrapperTask>().OrderBy(t => t.Order).ToList();

            foreach (var task in tasks)
            {
                YmatouLoggingService.Debug("YmatouFramework.Bootstrapper 开始执行 '{0}' ({1})", task.GetType().FullName, task.Description);
                try
                {
                    if (task.Execute() == TaskContinuation.Break)
                    {
                        YmatouLoggingService.Error("YmatouFramework.Bootstrapper 执行中断 '{0}' ({1})", task.GetType().FullName, task.Description);
                        successful = false;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    successful = false;
                    YmatouLoggingService.Error("YmatouFramework.Bootstrapper 执行出错 '{0}'，异常信息：{1}", task.GetType().FullName, ex.ToString());
                }
            };
            return successful;
        }

        protected override void InternalDispose()
        {
            container.ResolveAll<BootstrapperTask>().OrderByDescending(t => t.Order).Each(task =>
            {
                try
                {
                    YmatouLoggingService.Debug("YmatouFramework.Bootstrapper 开始清理 '{0}' ({1})", task.GetType().FullName, task.Description);
                    task.Dispose();
                }
                catch (Exception ex)
                {
                    YmatouLoggingService.Error("YmatouFramework.Bootstrapper 清理出错 '{0}'，异常信息：{1}", task.GetType().FullName, ex.ToString());
                }
            });
        }
    }
}
