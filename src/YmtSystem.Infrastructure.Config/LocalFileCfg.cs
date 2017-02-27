using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ServiceStack.Text.Json;
using ServiceStack.Text;
using YmtSystem.CrossCutting;
using YmtSystem.Infrastructure.Cache;
namespace YmtSystem.Infrastructure.Config
{
    public static class LocalFileCfg
    {
        //private static readonly LocalCache<string, object> cache = new LocalCache<string, object>(100);
        public static T GetLocalCfgValueByFile<T>(this string cfgFileName, ValueFormart vf, T defVal = default(T), TimeSpan expiredTimeOut = default(TimeSpan))
        {
            if (vf == ValueFormart.JSON)
                return GetLocalCfgValueByFileJson(cfgFileName, defVal, expiredTimeOut);
            if (vf == ValueFormart.CJsv)
                return GetLocalCfgValueByFileJSV(cfgFileName, defVal, expiredTimeOut);
            throw new NotImplementedException("未实现");
        }
        /// <summary>
        /// 序列为CSV，并写入文件（如果文件已存在，则截断为零）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <param name="fileName"></param>
        public static void SerializeToStream<T>(T val, string fileName)
        {
            FileHelp.Write<T>(val, fileName);
        }
        /// <summary>
        /// 序列化为JSOn，并写入文件（如果文件已存在，则截断为零）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <param name="fileName"></param>
        public static void SerializeJSONToStream<T>(T val, string fileName)
        {
            FileHelp.WriteToJson<T>(val, fileName);
        }
        private static T GetLocalCfgValueByFileJSV<T>(this string cfgFilePath, T defVal = default(T), TimeSpan expiredTimeOut = default(TimeSpan))
        {
            var val = LocalCache<string, object>.LocalCacheServer.Get(cfgFilePath, null);
            if ((val != null))
            {
                return (T)val;
            }
            var rVal = FileHelp.ReadToEnd(cfgFilePath, string.Empty);
            try
            {
                var rTVal = TypeSerializer.DeserializeFromString<T>(rVal);
                var cacheTime = expiredTimeOut == default(TimeSpan) ? TimeSpan.FromHours(24) : expiredTimeOut;
                LocalCache<string, object>.LocalCacheServer.Add(cfgFilePath, rTVal, cacheTime);

                return rTVal;
            }
            catch (Exception ex)
            {
                ex.Handler(string.Format("序列化配置错误 {0},{1},缓存 {2},返回默认值：{3}", cfgFilePath, ex.ToString(), LocalCache<string, object>.LocalCacheServer == null, defVal.ToString()));
                return defVal;
            }
        }
        private static T GetLocalCfgValueByFileJson<T>(this string cfgFilePath, T defVal = default(T), TimeSpan expiredTimeOut = default(TimeSpan))
        {
            var val = LocalCache<string, object>.LocalCacheServer.Get(cfgFilePath, null);
            if ((val != null))
            {
                return (T)val;
            }
            var rVal = FileHelp.ReadToEnd(cfgFilePath, "");
            try
            {
                var rTVal = ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(rVal);
                var cacheTime = expiredTimeOut == default(TimeSpan) ? TimeSpan.FromHours(24) : expiredTimeOut;
                LocalCache<string, object>.LocalCacheServer.Add(cfgFilePath, rTVal, cacheTime);
                return rTVal;
            }
            catch (Exception ex)
            {
                ex.Handler();
            }
            return defVal;
        }
    }

    class FileHelp
    {
        internal static string ReadToEnd(string filePath, string defVal = "")
        {
            filePath = FileFullPath(filePath);            
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath)) return defVal;
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var reader = new StreamReader(fs, Encoding.UTF8))
            {
                var fileString= reader.ReadToEnd().Replace("\r\n", "");
                //YmatouLoggingService.Debug("redad file {0}", fileString);
                return fileString;
            }
        }
        internal static Stream ReadToEnd(string filePath)
        {
            filePath = FileFullPath(filePath);
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath)) return null;
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            //using (var reader = new StreamReader(fs, Encoding.UTF8))
            {
                var by = new byte[fs.Length];
                fs.Read(by, 0, by.Length);
                var stream = new MemoryStream(by);
                return stream;
            }
        }
        internal static void Write<T>(T val, string filePath)
        {
            filePath = FileFullPath(filePath);
            EnsureFileExists(filePath);
            using (var fs = new FileStream(filePath, FileMode.Truncate, FileAccess.Write, FileShare.ReadWrite))
            {
                TypeSerializer.SerializeToStream<T>(val, fs);
            }
        }
        internal static void WriteToJson<T>(T val, string filePath)
        {
            filePath = FileFullPath(filePath);
            EnsureFileExists(filePath);
            using (var fs = new FileStream(filePath, FileMode.Truncate, FileAccess.Write, FileShare.ReadWrite))
            {
                JsonSerializer.SerializeToStream<T>(val, fs);
            }
        }
        private static void EnsureFileExists(string filePath)
        {
            if (!File.Exists(filePath))
                using (File.Create(filePath)) ;
        }
        private static string FileFullPath(string filePath)
        {
            var rooted = Path.IsPathRooted(filePath);
            if (!rooted)
                filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", filePath);
            return filePath;
        }
    }
    public enum ValueFormart
    {
        JSON = 0,
        XML = 1,
        CJsv = 2,
    }
}
