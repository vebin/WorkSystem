namespace YmtSystem.Repository.EF
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Migrations;

    /// <summary>
    /// 创建，迁移（变动）数据初始化
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    /// <typeparam name="TConfiguration"></typeparam>
    class CreateAndMigrateDatabaseInitializer<TContext, TConfiguration>
    : CreateDatabaseIfNotExists<TContext>, IDatabaseInitializer<TContext>
        where TContext : DbContext
        where TConfiguration : DbMigrationsConfiguration<TContext>, new()
    {

        private readonly DbMigrationsConfiguration _configuration;
        public CreateAndMigrateDatabaseInitializer()
        {
            _configuration = new TConfiguration();
        }
        public CreateAndMigrateDatabaseInitializer(string connection)
        {
            _configuration = new TConfiguration
            {
                TargetDatabase = new DbConnectionInfo(connection)
            };
        }
        void IDatabaseInitializer<TContext>.InitializeDatabase(TContext context)
        {
            var doseed = context.Database.Exists();
            //&& new DatabaseTableChecker().AnyModelTableExists(context);
            // check to see if to seed - we 'lack' the 'AnyModelTableExists'
            // ...could be copied/done otherwise if needed...

            var migrator = new DbMigrator(_configuration);
            //if (!doseed || !context.Database.CompatibleWithModel(false))
            //获取已在程序集中定义但尚未应用于目标数据库的所有迁移
            if (migrator.Configuration.AutomaticMigrationsEnabled
                && migrator.GetPendingMigrations().Any())
                migrator.Update();

            // move on with the 'CreateDatabaseIfNotExists' for the 'Seed'
            if (!doseed)
                base.InitializeDatabase(context);
            //如果数据库不存在
            if (!doseed)
            {
                Seed(context);
                context.SaveChanges();
            }
        }

        protected override void Seed(TContext context)
        {
            //创建db
            context.Database.CreateIfNotExists();
        }
    }

    internal sealed class Configuration<TContext> : DbMigrationsConfiguration<TContext>
        where TContext : DbContext
    {
        public Configuration()
        {
            //获取或设置指示迁移数据库时是否可使用自动迁移的值
            //ef 的自动迁移很麻烦，如果有实体增加字段，修改类型等操作，直接在数据库执行修改脚本，比用自动迁移方便多了
            //2014.3.26
            this.AutomaticMigrationsEnabled = false;
            ContextKey = "YmtSystem.Domain.Repository.EF.YmatouDbContext";
        }

        protected override void Seed(TContext context)
        {
        }
    }
}
