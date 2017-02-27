namespace YmtSystem.Infrastructure.Container._Ninject.Bootstrapper
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using YmtSystem.CrossCutting.Utility;
    using YmtSystem.CrossCutting;
    using Ninject;
  
    public class BootstrapperProxy : Disposable
    {
        private IKernel container;

        public BootstrapperProxy(IKernel container)
        {
            this.container = container;

            BuildManagerWrapper
                                .Current
                                .ConcreteTypes
                                .Where(type => typeof(BootstrapperTask).IsAssignableFrom(type))
                                .Each(type => container.Bind(typeof(BootstrapperTask)).To(type).InSingletonScope());
        }


        public bool Execute()
        {
            bool successful = true;
            var tasks = container.GetAll<BootstrapperTask>().OrderBy(t => t.Order).ToList();

            foreach (var task in tasks)
            {
                YmatouLoggingService.Debug("YmatouFramework.Bootstrapper 开始执行 '{0}' ({1})", task.GetType().FullName, task.Description);
                try
                {
                    //if (task.Execute() == TaskContinuation.Break)
                    task.Load();
                    {
                        YmatouLoggingService.Error("YmatouFramework.Bootstrapper 执行中断 '{0}' ({1})", task.GetType().FullName, task.Description);
                        //successful = false;
                        //break;
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
            container
                .GetAll<BootstrapperTask>()
                .OrderByDescending(t => t.Order)
                .Each(task =>
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
