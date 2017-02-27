


namespace YmtSystem.Infrastructure.Container.UnityWrapper
{
    using System;
    using Microsoft.Practices.Unity;

    public class LocalServiceLocator
    {
        public static T GetService<T>()
        {
            if (YmatouFramework.Status == YmatouFrameworkStatus.NotStarted)
            {
                if (YmatouFramework.LazyStart)
                {
                    YmatouFramework.Start();
                    if (YmatouFramework.Status == YmatouFrameworkStatus.Started)
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
            if (YmatouFramework.Status == YmatouFrameworkStatus.NotStarted)
            {
                if (YmatouFramework.LazyStart)
                {
                    YmatouFramework.Start();
                    if (YmatouFramework.Status == YmatouFrameworkStatus.Started)
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
                if (string.IsNullOrEmpty(name)) return YmatouFramework.Container.Resolve<T>();
                else return YmatouFramework.Container.Resolve<T>(name);
            }
            catch (Exception ex)
            {
                YmatouLoggingService.Error("YmatouFramework.LocalServiceLocator 不能解析 '{0}'，异常信息：{1}", typeof(T).FullName, ex.ToString());
                throw;
            }
        }
    }
}
