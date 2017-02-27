namespace YmtSystem.Repository.EF.Factory
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using YmtSystem.CrossCutting;
    using YmtSystem.Infrastructure.Config;

    internal class DbConnectionFactory
    {
        public static DbConnection Builder(string contextName, string dbFileName = "db.config")
        {
            try
            {
                var cfg = DbConfigure.GetConfigure(contextName, dbFileName);
                var factory = DbProviderFactories.GetFactory(cfg.Provider);
                var connection = factory.CreateConnection();
                connection.ConnectionString = cfg.Connection;
                //LocalFileCfg.GetLocalCfgValueByFile<string>("db.cfg", ValueFormart.CJsv, @"Data Source=YMT-LIGUO\LIGUO;Initial Catalog=Ymt_Test_7;Persist Security Info=True;User ID=sa;Password=123@#$123asd");
                //TODO:这里可以设置其他链接属性
                return connection;
            }
            catch (Exception ex)
            {
                YmatouLoggingService.Error("创建链接对象异常 {0}", ex.ToString());
                throw;
            }
        }
        public static DbConnection Builder(DbConfigure cfg)
        {
            try
            {
                YmtSystemAssert.AssertArgumentNotNull(cfg, "DbConfigure 不能为空");
                var factory = DbProviderFactories.GetFactory(cfg.Provider);
                var connection = factory.CreateConnection();
                connection.ConnectionString = cfg.Connection;
                //LocalFileCfg.GetLocalCfgValueByFile<string>("db.cfg", ValueFormart.CJsv, @"Data Source=YMT-LIGUO\LIGUO;Initial Catalog=Ymt_Test_7;Persist Security Info=True;User ID=sa;Password=123@#$123asd");
                //TODO:这里可以设置其他链接属性
                return connection;
            }
            catch (Exception ex)
            {
                YmatouLoggingService.Error("创建链接对象异常 {0}", ex.ToString());
                throw;
            }
        }
    }
}
