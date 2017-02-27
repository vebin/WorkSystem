
using System;
using Microsoft.Practices.Unity;

namespace Ymatou.Infrastructure
{
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

        private static T GetInstance<T>()
        {
            try
            {
                T instance = YmatouFramework.Container.Resolve<T>();
                return instance;
            }
            catch (Exception ex)
            {
                LocalLoggingService.Error("YmatouFramework.LocalServiceLocator 不能解析 '{0}'，异常信息：{1}", typeof(T).FullName, ex.ToString());
                throw;
            }
        }
    }
}
