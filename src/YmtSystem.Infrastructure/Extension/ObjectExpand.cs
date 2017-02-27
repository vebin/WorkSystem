
namespace Adhesive.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    
    using System.Xml;
    using System.Collections.Specialized;
    //using System.Threading.Tasks;

    public static class ObjectExpand
    {
        public static T VerifyAndReturnT<T>(this T t, Func<T, bool> fun, bool throwOut = false, string msg = null, T defVal = default(T))
        {

            if (t == null)
            {
                if (throwOut) throw new ArgumentNullException(msg);
            }
            else
            {
                return t;
            }
            if (fun(t)) return t;
            else
            {
                if (throwOut) throw new ArgumentNullException(string.Format("{0}", msg));
                else return defVal;
            }
        }
        public static DateTime VerifyTimeRange(this DateTime dt, DateTime start, DateTime end, DateTime defVal)
        {
            if (dt >= start && dt <= end) return dt;
            return defVal;
        }
        public static TimeSpan StringFromToTimeSpan(this string v, string type)
        {
            v.IsEmpty(true);
            if (type == "YYYY")
            {
                return TimeSpan.FromSeconds(v.ConvertChangType<int>() * (3600 * 24 * 365));
            }
            else if (type == "MM")
            {
                return TimeSpan.FromSeconds(v.ConvertChangType<int>() * (3600 * 24 * 30));
            }
            else if (type == "DD")
            {
                return TimeSpan.FromSeconds(v.ConvertChangType<int>() * (3600 * 24));
            }
            else if (type == "HH")
            {
                return TimeSpan.FromSeconds(v.ConvertChangType<int>() * (3600));
            }
            else if (type == "mm")
            {
                return TimeSpan.FromSeconds(v.ConvertChangType<int>() * (60));
            }
            else if (type == "ss")
            {
                return TimeSpan.FromSeconds(v.ConvertChangType<int>() * (1));
            }
            throw new ArgumentException("type 错误无法转换为时间戳");
        }
        
        public static TimeSpan ConvertDateTimeSubtractToTimeSpan(this DateTime l, DateTime r,int value, bool verifyTimeFomat = false)
        {
            if (verifyTimeFomat)
                CheckLTimeLessthanRTime(l, r, false, true);
            var sub = r.Subtract(l);

            if (sub.Days >= 365) return new TimeSpan(365, 0, 0, 0, 0);
            else if (sub.Days >= 30) return new TimeSpan(30, 0, 0, 0, 0);
            else if (sub.Days > 1 && sub.Days <= 31) return new TimeSpan(value, 0, 0, 0, 0);
            else if (sub.Days == 1) return new TimeSpan(value, 0, 0, 0);
            else if (sub.Days == 1 && sub.Hours > 1 && sub.Hours <= 24) return new TimeSpan(value, 0, 0);
            else if (sub.Days == 1 && sub.Hours <= 1 && sub.Minutes >= 0 && sub.Minutes <= 60) return new TimeSpan(0, value, 0);
            else return new TimeSpan(0, 0, value);
        }


        public static string GetFormatTime(this DateTime? t, string format,DateTime? defAvl=null)
        {
            if (format.IsEmpty(false)) format = "yyyyMMdd HH:mm:ss,fff";
            if (t.HasValue)
            {
                return t.Value.ToString(format);
            }
            if (defAvl != null && defAvl.HasValue)
            {
                return defAvl.Value.ToString(format);
            }
            return DateTime.MaxValue.ToShortTimeString();
        }

        public static string ConnectList<T>(this IEnumerable<T> v, Func<T, string> fn)
        {
            if (v.IsEmpty()) return string.Empty;
            var str = new StringBuilder();
            foreach (var t in v)
            {
                str.Append(fn(t));
            }
            return str.ToString();
        }

        public static  Dictionary<string, object> RequestQueryStringMapDic(this NameValueCollection queryKeys)
        {
            if (queryKeys == null || queryKeys.Count <= 0) return new Dictionary<string, object>();
            var keys = queryKeys.Keys.OfType<string>();
            var dic = new Dictionary<string, object>();
            foreach (var keyItem in keys)
            {
                dic.TryAddOrSet(keyItem, queryKeys[keyItem]);
            }
            return dic;
        }

        public static List<string> RTimeSubtractRTime(this DateTime r, DateTime l, string subtractType
            , string returnFormat = null, bool verifyDateTime = false)
        {
            if (verifyDateTime)
                l.CheckLTimeLessthanRTime(r, false, true);
            subtractType.IsEmpty(true);

            if (returnFormat.IsEmpty())
                returnFormat = subtractType;
            var list = new List<string>();
            lock (list)
            {
                if (subtractType == "yyyyMM")
                {
                    var subTime = Convert.ToInt32(r.Subtract(l).TotalDays / 30);
                    while (subTime >= 0)
                    {
                        try
                        {
                            list.Add(l.AddMonths(subTime).ToString(returnFormat));
                        }
                        finally
                        {
                            subTime -= 1;
                        }
                    }
                    return list;
                }
                else
                {
                    throw new NotImplementedException("为实现其他扩展");
                }
            }
        }

        public static bool CheckLTimeLessthanRTime(this DateTime l, DateTime r ,bool filterDefaultTime=false,bool throwOut=false )
        {
            if (!filterDefaultTime)
            {
                if (l == DateTime.MinValue || r == DateTime.MinValue
                    || l == DateTime.MaxValue || r == DateTime.MaxValue)
                {
                    if (throwOut) throw new ArgumentException(string.Format("{0}>{1}", l, r));
                    return false;
                }
                else
                {
                    if (l <= r) return true;
                    else
                    {
                        if (throwOut) throw new ArgumentException(string.Format("{0}>{1}", l, r));
                        else return false;
                    }

                }
            }
            else
            {
                if (l <= r) return true;
                else
                {
                    if (throwOut) throw new ArgumentException(string.Format("{0}>{1}", l, r));
                    else return false;
                }
            }
        }

        public static string AsString(this byte[] o)
        {
            return UTF8Encoding.UTF8.GetString(o);
        }

        public static T AsT<T>(this string strJson)
        {
            try
            {
                if (string.IsNullOrEmpty(strJson)) return default(T);
                var js = new JavaScriptSerializer();
                js.MaxJsonLength = int.MaxValue;
                js.RecursionLimit = 100;
                return js.Deserialize<T>(strJson);
            }
            catch (Exception e)
            {
                return default(T);
            }
        }

        public static List<T> AddTo<T>(this T v, List<T> list, Func<T, bool> fn = null)
        {
            if (fn != null)
            {
                if (fn(v))
                    list.Add(v);
            }
            else
            {
                if (!v.Equals(default(T)))
                    list.Add(v);
            }
            return list;
        }     

        public static Array ConvertToTwoArray<T>(this IEnumerable<T> v,ref int row, int column)
        {           
            if (row <= 0 || column <= 0) throw new ArgumentException("row ，column 必须大于零");
            v.IsEmpty(true);
            lock (v)
            {
                var tmpList = v.ToArray();
                var vLen = tmpList.Length;
                var rc = row * column;
                var tmpRow = (vLen / column) + (vLen % column > 0 ? 1 : 0);
                var tmpCol = column; //(vLen / column) + (vLen % column > 0 ? 1 : 0);
                row = tmpRow;
               
                var towArr = Array.CreateInstance(typeof(T), tmpRow, tmpCol);
                var index = 0;
                for (var r = 0; r < tmpRow; r++)
                {
                    for (var c = 0; c < tmpCol; c++)
                    {
                        if (index < vLen)
                        {                           
                            towArr.SetValue(tmpList[index], r, c);                           
                        }
                        else
                        {
                            break;
                            //towArr.SetValue(default(T), r, c);
                            //--column;
                        }
                      
                        index++;
                    }
                }
                return towArr;
            }
        }

        public static void AddTo<T>(this IEnumerable<T> v, List<T> targetList, Func<T, bool> fn = null)
        {
            if (fn != null)
            {
                foreach (var item in v)
                {
                    if (fn(item))
                        targetList.Add(item);
                }
            }
            else
            {
                if (!v.IsEmpty())
                {
                    targetList.AddRange(v);                   
                }
            }
        }

        public static IList<T> Add<T>(this IList<T> v, T val, Func<T, bool> filter)
        {
            if (filter(val))
                v.Add(val);
            return v;
        }

        public static T AsT<T>(this byte[] v)
        {
            var value = v.AsString();
            var t = AsT<T>(value);
            return t;
        }

        //public static IEnumerable<T> AsTArray<T>(this IEnumerable<string> array)
        //{
        //    var js = new JavaScriptSerializer();
        //    js.MaxJsonLength = int.MaxValue;
        //    js.RecursionLimit = 100;
        //    var listValue = new List<T>();

        //    var result = Parallel.ForEach(array, e =>
        //    {
        //        var o = js.Deserialize<T>(e);
        //        listValue.Add(o);
        //    });
        //    while (!result.IsCompleted) ;
        //    return listValue;
        //}

        public static byte[] AsByteArray(this string o)
        {
            return UTF8Encoding.UTF8.GetBytes(o);
        }

        public static bool CheckDateTimeleftLTERight(this DateTime? left, DateTime? right,bool throwOut=false)
        {
            if (!left.HasValue || !right.HasValue)
            {
                if (!throwOut) throw new ArgumentNullException("leftValue ,rightValue 不能为空");
                return false; 
            }
            if (left.Value <= right.Value)
            {
                return true;
            }
            else 
            {
                if (!throwOut) throw new ArgumentException(string.Format("leftValue {0} 大于等于 rightValue {1} ", left.Value, right.Value));
            return false;
            }
        }

        public static byte[] AsByteArray(this object o)
        {
            try
            {
                var js = new JavaScriptSerializer();
                js.MaxJsonLength = int.MaxValue;
                js.RecursionLimit = 100;
                var json = js.Serialize(o);
                var by = UTF8Encoding.UTF8.GetBytes(json);
                return by;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public static DateTime AsTime(this long l)
        {
            try
            {
                DateTime dt = new DateTime(l);
                return dt;
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        public static IEnumerable<string> AsStringList(this string strPath, char split)
        {
            return strPath.Split(new char[] { split }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static T ConvertChangType<T>(this object v,T defV=default(T))
        {
            if (v == null) return defV;
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
                    return defV;
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
                        return defV;
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
                        return defV;
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
                    else if (Enum.GetNames(type).Select(e => e.ToLower()).Contains(v.ToString().ToLower()) ||
                        Enum.GetValues(type).Cast<int>().Select(e => e.ToString()).Contains(v.ToString()))
                        return (T)Enum.Parse(type, v.ToString(), true);
                    return defV;

                }
                catch
                {
                    return defV;
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
                    return defV;
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
                    return defV;
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
                    return defV;
                }
            }
            try
            {
                return (T)Convert.ChangeType(v, type);
            }
            catch
            {
                return defV;
            }
        }
    }
}
