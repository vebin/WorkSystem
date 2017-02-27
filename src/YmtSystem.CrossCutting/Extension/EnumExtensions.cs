namespace System
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    public static class EnumExtensions
    {
        [DebuggerStepThrough]
        public static string GetDescription(this Enum type, string defDesc = null)
        {
            var enumType = type.GetType();
            var name = Enum.GetName(enumType, type);
            var fieldInfo = enumType.GetField(name);
            var desc = Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute), false) as DescriptionAttribute;

            if (desc == null)
                return defDesc;
            return desc.Description;

        }
        [DebuggerStepThrough]
        public static bool Has<T>(this Enum type, T value)
        {
            try
            {
                return ((int)(object)type & (int)(object)value) == (int)(object)value;
            }
            catch
            {
                return false;
            }
        }

        [DebuggerStepThrough]
        public static bool Is<T>(this Enum type, T value)
        {
            try
            {
                return (int)(object)type == (int)(object)value;
            }
            catch
            {
                return false;
            }
        }

        [DebuggerStepThrough]
        public static T Add<T>(this Enum type, T value)
        {
            try
            {
                return (T)(object)((int)(object)type | (int)(object)value);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Format("不能为枚举 '{0}' 添加值 '{1}'！", typeof(T).Name, value), ex);
            }
        }

        [DebuggerStepThrough]
        public static T Remove<T>(this Enum type, T value)
        {
            try
            {
                return (T)(object)((int)(object)type & ~(int)(object)value);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Format("不能从枚举 '{0}' 移除值 '{1}'！", typeof(T).Name, value), ex);
            }
        }
    }
}
