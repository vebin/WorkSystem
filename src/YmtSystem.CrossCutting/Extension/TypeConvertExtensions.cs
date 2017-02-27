using System;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
namespace System
{
    public static class TypeConvertExtensions
    {
        public static IEnumerable<T> ConvertTo<T>(this IEnumerable<object> v)
        {
            foreach (var item in v)
                yield return item.ConvertTo<T>();
        }
        public static T ConvertTo<T>(this object v, T defVal = default(T))
        {
            if (v == null) return defVal;
            //目标类型
            var type = typeof(T);
            var valueType = v.GetType();
            if (type == typeof(String))
            {
                return (T)((Object)v.ToString());
            }
            else if (type == typeof(TimeSpan))
            {
                TimeSpan ts;
                if (TimeSpan.TryParse(v.ToString(), out ts))
                {
                    return (T)((object)ts);
                }
                else
                {
                    return defVal;
                }
            }
            else if (type == typeof(DateTime))
            {
                if (valueType == typeof(Int64))
                {
                    try
                    {
                        return (T)((Object)Convert.ToDateTime(Convert.ToInt64(v)));
                    }
                    catch
                    {
                        return defVal;
                    }
                }
                if (valueType == typeof(string))
                {
                    try
                    {
                        DateTime tmpTime;
                        if (DateTime.TryParse(v.ToString(), out tmpTime))
                        {
                            return (T)((Object)tmpTime);
                        }
                    }
                    catch
                    {
                        return defVal;
                    }
                }
            }
            else if (type.IsEnum)
            {
                try
                {
                    var flagsAttribute = type.GetCustomAttributes(false).OfType<FlagsAttribute>().SingleOrDefault();
                    if (flagsAttribute != null)
                        return (T)Enum.Parse(type, v.ToString(), true);
                    else
                        return (T)Enum.Parse(type, v.ToString(), true);

                }
                catch
                {

                    return defVal;
                }
            }
            else if (type == typeof(bool))
            {
                bool b;
                if (bool.TryParse(v.ToString(), out b))
                {
                    return (T)((object)b);
                }
                else
                {
                    return defVal;
                }
            }
            else if (type == typeof(Type))
            {
                try
                {
                    return (T)((Object)Type.GetType(valueType.FullName));
                }
                catch
                {
                    return defVal;
                }
            }
            else if (type == typeof(XmlDocument))
            {
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(v.ToString());
                    return (T)((Object)xmlDoc);
                }
                catch
                {
                    return defVal;
                }
            }
            try
            {
                return (T)Convert.ChangeType(v, type);
            }
            catch
            {
                return defVal;
            }
        }
    }
}
