namespace YmtSystem.Infrastructure.Container._Ninject.Bootstrapper
{
    using System;
    using YmtSystem.CrossCutting;
    using Ninject;

    public class LocalServiceLocator
    {
        public static T GetService<T>()
        {
            if (YmatouBootstrapperFramework.Status == BootstrapperStatus.NotStarted)
            {
                if (YmatouBootstrapperFramework.LazyStart)
                {
                    YmatouBootstrapperFramework.Start();
                    if (YmatouBootstrapperFramework.Status == BootstrapperStatus.Started)
                    {
                        return GetInstance<T>();
                    }
                }
                throw new Exception("YmatouFramework尚未启动,可选择延迟启动！");
            }
            else
            {
                return GetInstance<T>();
            }
        }

        public static T TryGetService<T>(T defVal = default(T))
        {
            try
            {
                return GetService<T>();
            }
            catch
            {
                return defVal;
            }
        }
        public static T TryGetService<T>(string name = "", T defVal = default(T))
        {
            try
            {
                return GetService<T>(name);
            }
            catch
            {
                return defVal;
            }
        }
        public static T GetService<T>(string name)
        {
            if (YmatouBootstrapperFramework.Status == BootstrapperStatus.NotStarted)
            {
                if (YmatouBootstrapperFramework.LazyStart)
                {
                    YmatouBootstrapperFramework.Start();
                    if (YmatouBootstrapperFramework.Status == BootstrapperStatus.Started)
                    {

                        return GetInstance<T>(name);

                    }
                }
                throw new Exception("YmatouFramework尚未启动,可选择延迟启动！");
            }
            else
            {
                return GetInstance<T>(name);
            }
        }
        private static T GetInstance<T>(string name = "")
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                    return YmatouBootstrapperFramework.Container.Get<T>();
                else
                    return YmatouBootstrapperFramework.Container.Get<T>(name);
            }
            catch (Exception ex)
            {
                YmatouLoggingService.Error("YmatouFramework.LocalServiceLocator 不能解析 '{0}'，异常信息：{1}", typeof(T).FullName, ex.ToString());
                throw;
            }
        }
    }
}
