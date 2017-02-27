using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;
using YmtSystem.CrossCutting;

namespace YmtSystem.Infrastructure.Config
{
    public static class LocalCfg
    {
        /// <summary>
        /// 获取web.config配置文件中的指定key的值，并转为对应的类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cfgName"></param>
        /// <param name="defVal"></param>
        /// <returns></returns>
        public static T GetLocalCfgValue<T>(this string cfgName, T defVal = default(T))
        {
            if (string.IsNullOrEmpty(cfgName)) return defVal;
            var val = ConfigurationManager.AppSettings[cfgName];
            if (!string.IsNullOrEmpty(val))
            {
                return val.ConvertTo<T>(defVal);
            }
            else
            {
                val = ConfigurationManager.ConnectionStrings[cfgName].ConnectionString;
                if (!string.IsNullOrEmpty(val))
                    return val.ConvertTo<T>(defVal);
                else
                    return defVal;
            }
        }

        public static T GetLocalCfgValue<T>(this string cfgName, Func<string, T> customConvert, T defVal = default(T))
        {
            if (string.IsNullOrEmpty(cfgName)) return defVal;
            var val = ConfigurationManager.AppSettings[cfgName];
            if (!string.IsNullOrEmpty(val))
            {
                return Convert<T>(customConvert, defVal, val);
            }
            else
            {
                val = ConfigurationManager.ConnectionStrings[cfgName].ConnectionString;
                if (!string.IsNullOrEmpty(val))
                {
                    return Convert<T>(customConvert, defVal, val);
                }
                else
                {
                    return defVal;
                }
            }
        }


        private static T Convert<T>(Func<string, T> customConvert, T defVal, string val)
        {
            try
            {
                return customConvert(val);
            }
            catch
            {
                return defVal;
            }
        }
    }
}
