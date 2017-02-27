using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YmtSystem.Infrastructure.EventStore
{
    internal class Configure
    {
        //tb 映射
        private static readonly Dictionary<string, string> tbMapping = new Dictionary<string, string> 
        {
            {"YmtAuth.Domain.Model.LoginCredential.UserLoginCredential","ULC_201402"},
            {"YmtAuth.Domain.Model.User.YmtUser","UData_201402"}
        };
#if DEBUG
        // private static readonly string mongoUrl = "mongodb://192.168.1.5:27001,192.168.1.5:27002,192.168.1.5:27003,192.168.1.5:27004/?replicaSet=shard3&slaveOk=true&connectTimeoutMS=300000&w=1";
        private static readonly string mongoUrl = "mongodb://192.168.1.5:27005/?connectTimeoutMS=300000&w=1";//192.168.1.247:27001
#else
        private static readonly string mongoUrl = "mongodb://10.10.10.195:27001";
#endif
        private static readonly string dbName = "Ymt_Login_Auth_201402";
        private static readonly string defCollectionName = "ULC_201402";
        public static TimeSpan LocalCacheTimeOut { get; set; }
        public static String MongoUrl { get { return GetMongoUrl(); } }
        public static String MongoDatabaseName { get { return GetMongoDatabaseName(); } }
        public static String DefCollectionName { get { return defCollectionName; } }
        public static Dictionary<String, String> TBMapping { get { return tbMapping; } }
        public static bool EnableMappingTb { get { return true; } }
        public static string GetCollectionName(string typeName)
        {
            var defName = defCollectionName;
            if (tbMapping.TryGetValue(typeName, out defName))
                return defName;
            else throw new KeyNotFoundException(string.Format("未找到匹配的集合{0},", typeName));
        }
        private static String GetMongoUrl()
        {
            return mongoUrl;
            //return System.Configuration.ConfigurationManager.AppSettings["mongourl"] ?? mongoUrl;
        }
        private static String GetMongoDatabaseName()
        {
            return dbName;
        }
    }
}
