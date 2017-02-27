namespace YmtSystem.Repository.EF
{
    using System;
    using System.Collections.Generic;
    using YmtSystem.Infrastructure.Config;
    using YmtSystem.CrossCutting;

    public class DbConfigure
    {
        private string _connection;
        public string Connection
        {
            get { return _connection; }
            set { if (string.IsNullOrEmpty(value))throw new ArgumentNullException("链接字符串不能为空"); _connection = value; }
        }

        private string _provider;
        public string Provider
        {
            get { return string.IsNullOrEmpty(_provider) ? "System.Data.SqlClient" : _provider; }
            set { _provider = value; }
        }
        public bool? OnSavedValidateEntity { get; set; }
        public int? CommondTimeOut { get; set; }
        public bool? LazyLoad { get; set; }
        public bool? AutoDetectChangesEnabled { get; set; }
        public bool MonitorSqlCommond { get; set; }
        public int MonitorSlowSqlRunTime { get; set; }
        public bool TransactionFailRetry { get; set; }
        public bool ConnectionFailRetry { get; set; }
        public DbContextLifeScope Scope { get; set; }
        /// <summary>
        /// ef 释放资源是否自动断开连接。
        /// <remarks>true:ef 释放资源时断开连接；false：由调用者自行是否连接</remarks>
        /// </summary>
        public bool ContextOwnsConnection { get; set; }


        public static void Save(Dictionary<string, DbConfigure> cfg)
        {
            LocalFileCfg.SerializeJSONToStream<Dictionary<string, DbConfigure>>(cfg, "db.config");
        }
        public static DbConfigure GetConfigure(string contextName, string dbFileName = "db.config")
        {
            try
            {
                var cfgDic = dbFileName.GetLocalCfgValueByFile<Dictionary<string, DbConfigure>>(
                    //"db.cfg" ,
                    ValueFormart.JSON
                    , null
                    , TimeSpan.FromHours(1));
                if (cfgDic == null || cfgDic.Count <= 0)
                    throw new NullReferenceException("DB库链接文件为空");
                var cfg = cfgDic.TryGetV(contextName, null, false);
                if (cfg == null)
                {
                    throw new KeyNotFoundException(contextName + " not find");
                }
                return cfg;
            }
            catch (Exception ex)
            {
                ex.Handler("获取数据库配置错误 ");
                throw;
            }
        }
    }
}
